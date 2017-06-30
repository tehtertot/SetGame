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
        bool inProgress;
        public int Points = 0;
        DateTime gameStartTime;

        public GamePage()
        {
            InitializeComponent();
            inProgress = true;
            gameStartTime = DateTime.Now;
            Device.StartTimer(TimeSpan.FromSeconds(61), () =>
            {
                SaveScore();
                inProgress = false;
                return inProgress;
            });
            Device.StartTimer(TimeSpan.FromSeconds(1), () =>
            {
                this.Title = (DateTime.Now - gameStartTime).ToString(timeFormat) + "    Sets: " + Points;
                return inProgress;
            });

            Deck.CreateDeck();
            Deck.Shuffle(2);

            SetInPlay();
            SetWithSolution();
            DisplayAllCards();
        }

        public void SetInPlay()
        {
            for (int i = 0; i < 12; i++)
            {
                Deck.InPlay[i] = Deck.DrawCard();
            }
        }

        public void DisplayCardImage(int idx, int col, int row)
        {
            var tapImage = new TapGestureRecognizer();
            tapImage.Tapped += CheckCard;

            Image i = new Image
            {
                VerticalOptions = LayoutOptions.CenterAndExpand,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                BackgroundColor = Color.Black,
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
                    DisplayCardImage(cardNum, c, r);
                    cardNum++;
                }
            }
        }

        private List<string> selectedImages = new List<string>();

        private async void CheckCard(object sender, EventArgs e)
        {
            Image img = sender as Image;
            img.BackgroundColor = Color.LawnGreen;

            if (!selectedImages.Contains(img.ClassId))  //if this card hasn't already been selected
            {
                selectedImages.Add(img.ClassId);
            }
            else
            {
                selectedImages.Remove(img.ClassId);
                img.BackgroundColor = Color.Black;
            }
            if (selectedImages.Count == 3)  //check the 3 cards selected 
            {
                //replace each card
                foreach (string r in selectedImages)
                {
                    //get the index of the card InPlay to replace
                    string[] rstring = r.Split('-');
                    string rep = rstring[rstring.Length - 1];
                    int replace = Convert.ToInt32(rep);

                    if (ValidateChoices(selectedImages))
                    {
                        //discard the set, replace the card
                        Deck.InPlay[replace] = Deck.DrawCard();
                        if (r.Equals(selectedImages[0]))
                        {
                            Points++;
                        }
                    }
                    else
                    {
                        if (r.Equals(selectedImages[2]))
                        {
                            await DisplayAlert("Nope", "It's not a set.", "Try again");
                        }
                    }
                    //display the new card or card with black background
                    int col = replace % 3;
                    int row;
                    if (replace <= 2)
                    {
                        row = 0;
                    }
                    else if (replace <= 5)
                    {
                        row = 1;
                    }
                    else if (replace <= 8)
                    {
                        row = 2;
                    }
                    else
                    {
                        row = 3;
                    }
                    DisplayCardImage(replace, col, row);
                }

                SetWithSolution();
                selectedImages.Clear();
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

        private async void SaveScore()
        {
            await DisplayAlert("Time's Up!", $"You got {Points} sets in 1 minute.", "OK");
            Score s = new Score
            {
                NumSets = Points,
                CreatedAt = DateTime.Now,
                Type = "Timed"
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
            }
        }
    }
}