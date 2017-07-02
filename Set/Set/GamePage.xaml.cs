using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Set
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class GamePage : ContentPage
    {
        const string timeFormat = @"%m\:ss";
        string gameType;
        int gameValue;
        int[] hint;
        bool inProgress;
        public int Points = 0;
        DateTime gameStartTime;

        public GamePage(string type, int val)
        {
            InitializeComponent();
            inProgress = true;
            gameType = type;
            gameValue = val;
            hint = new int[3];
            gameStartTime = DateTime.Now;
            if (gameType == "speed")
            {
                Device.StartTimer(TimeSpan.FromMinutes(gameValue), () =>
                {
                    SaveScore(Points, "speed");
                    inProgress = false;
                    return inProgress;
                });
            }

            //show timer
            Device.StartTimer(TimeSpan.FromSeconds(1), () =>
            {
                this.Title = (DateTime.Now - gameStartTime).ToString(timeFormat) + "    Sets: " + Points;
                return inProgress;
            });

            var toolbarItem = new ToolbarItem
            {
                Text = "Hint"
            };
            toolbarItem.Clicked += async (sender, e) =>
            {
                bool takeAHint = await DisplayAlert("Take a hint?", "That'll cost you a set!", "Yes, please!", "Nevermind");
                if (takeAHint)
                {
                    foreach (int i in hint)
                    {
                        int col = i % 3;
                        int row = GetRow(i);
                        DisplayCardImage(i, col, row, Color.BlueViolet);
                    }
                    Points--;
                }
            };
            ToolbarItems.Add(toolbarItem);

            Deck.CreateDeck();
            Deck.Shuffle(2);

            SetInPlay();
            SetWithSolution();
            DisplayAllCards();
        }

        private int GetRow(int idx)
        {
            int row;
            if (idx <= 2)
            {
                row = 0;
            }
            else if (idx <= 5)
            {
                row = 1;
            }
            else if (idx <= 8)
            {
                row = 2;
            }
            else
            {
                row = 3;
            }
            return row;
        }
        public void SetInPlay()
        {
            for (int i = 0; i < 12; i++)
            {
                Deck.InPlay[i] = Deck.DrawCard();
            }
        }

        public void DisplayCardImage(int idx, int col, int row, Color color)
        {
            var tapImage = new TapGestureRecognizer();
            tapImage.Tapped += CheckCard;

            Image i = new Image
            {
                VerticalOptions = LayoutOptions.CenterAndExpand,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                BackgroundColor = color,
                Source = Deck.InPlay[idx].GetCardImage(),
                ClassId = Deck.InPlay[idx].GetCardText() + "-" + idx.ToString(),
            };
            i.GestureRecognizers.Add(tapImage);
            cardLayout.Children.Add(i, col, row);
        }

        public void DisplayAllCards()
        {
            int cardNum = 0;

            for (int r = 0; r < 4; r++)
            {
                for (int c = 0; c < 3; c++)
                {
                    DisplayCardImage(cardNum, c, r, Color.Black);
                    cardNum++;
                }
            }
        }

        private List<string> selectedImages = new List<string>();

        private async void CheckCard(object sender, EventArgs e)
        {
            Image img = sender as Image;

            if (!selectedImages.Contains(img.ClassId))  //if this card hasn't already been selected
            {
                selectedImages.Add(img.ClassId);
                img.BackgroundColor = Color.DarkSeaGreen;
            }
            else
            {
                selectedImages.Remove(img.ClassId);
                img.BackgroundColor = Color.Black;
            }
            if (selectedImages.Count == 3)  //check the 3 cards selected 
            {
                if (ValidateChoices(selectedImages)) {
                    foreach (string r in selectedImages)
                    {
                        string[] rstring = r.Split('-');
                        string rep = rstring[rstring.Length - 1];
                        int replace = Convert.ToInt32(rep);
                        Deck.InPlay[replace] = Deck.DrawCard();

                        int col = replace % 3;
                        int row = GetRow(replace);
                        DisplayCardImage(replace, col, row, Color.Black);
                    }
                    Points++;
                    SetWithSolution();
                    selectedImages.Clear();
                }
                else
                {
                    foreach (string r in selectedImages)
                    {
                        string[] rstring = r.Split('-');
                        string rep = rstring[rstring.Length - 1];
                        int replace = Convert.ToInt32(rep);

                        int col = replace % 3;
                        int row = GetRow(replace);
                        DisplayCardImage(replace, col, row, Color.Black);
                    }
                    await DisplayAlert("Nope", "It's not a set.", "Try again");
                    selectedImages.Clear();
                }

                if (Points == gameValue && gameType == "timed")
                {
                    int seconds = (int)(DateTime.Now - gameStartTime).TotalSeconds;
                    SaveScore(seconds, "timed");
                    inProgress = false;
                }
            }
        }

        private bool ValidateChoices(List<string> resp)
        {
            bool isValid = true;
            string[] Card1 = resp[0].Split('-');
            string[] Card2 = resp[1].Split('-');
            string[] Card3 = resp[2].Split('-');

            //loop through each category (num, pattern, shape, color)
            for (int i = 0; i < 4; i++)
            {
                if ((Card1[i].Equals(Card2[i]) && Card2[i].Equals(Card3[i])))
                {
                    //all same
                }
                else if ((!Card1[i].Equals(Card2[i]) && !Card2[i].Equals(Card3[i]) && !Card3[i].Equals(Card1[i])))
                {
                    //all different
                }
                else
                {
                    return false;
                }
            }

            return isValid;
        }

        private async void SaveScore(int score, string type)
        {
            int Order = score;
            string Display = "";
            string Category = "";
            if (type == "speed")
            {
                await DisplayAlert("Time's Up!", $"You got {score} sets in {gameValue} minutes.", "OK");
                Display = score.ToString() + " sets";
                Order = score * -1;
                Category = "Speed: " + gameValue.ToString() + " minutes";
            }
            else if (type == "timed")
            {
                await DisplayAlert("Done!", $"You got {gameValue} sets in {score} seconds.", "OK");
                TimeSpan t = TimeSpan.FromSeconds(score);
                Display = t.ToString(@"mm\:ss");
                Category = "Timed: " + gameValue.ToString() + " sets";
                
            }
            Score s = new Score
            {
                NumSets = score,
                CreatedAt = DateTime.Now,
                Type = Category,
                OrderBy = Order,
                ScoreDisplay = Display
            };
            await App.Database.SaveScore(s);
            await Navigation.PushAsync(new Scores());
        }

        private bool HasSolution()
        {
            bool isValid = false;
            for (int i = 0; i < Deck.InPlay.Length; i++)
            {
                for (int j = i + 1; j < Deck.InPlay.Length; j++)
                {
                    for (int k = j + 1; k < Deck.InPlay.Length; k++)
                    {
                        List<string> test = new List<string> { Deck.InPlay[i].GetCardText(), Deck.InPlay[j].GetCardText(), Deck.InPlay[k].GetCardText() };
                        if (ValidateChoices(test))
                        {
                            hint[0] = i;
                            hint[1] = j;
                            hint[2] = k;
                            return true;
                        }
                    }
                }
            }
            return isValid;
        }

        private void SetWithSolution()
        {
            while (!HasSolution())
            {
                //add inplay cards back to deck
                for (int i = 0; i < 12; i++)
                {
                    Deck.AllCards.Add(Deck.InPlay[i]);
                }
                Deck.Shuffle(2);
                SetInPlay();
                DisplayAllCards();
            }
        }
    }
}