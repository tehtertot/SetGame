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
	public partial class Selection : ContentPage
	{
        public string gameType;

		public Selection (string type)
		{
			InitializeComponent ();
            this.Title = "Select Game Duration";
            gameType = type;
            if (gameType == "speed")
            {
                inputLabel.Text = "How many sets can you collect in";
                inputLabel2.Text = "minutes?";
            }
            else if (gameType == "timed")
            {
                inputLabel.Text = "How fast can you find";
                inputLabel2.Text = "sets?";
            }
		}

        private async void StartGame(object sender, EventArgs e)
        {
            try
            {
                int gameVal = int.Parse(gameValue.Text);
                if (gameVal < 1)
                {
                    await DisplayAlert("Invalid Entry", "Positive numbers only.", "OK");
                }
                else
                {
                    if (gameType == "speed")
                    {
                        await Navigation.PushAsync(new GamePage("speed", gameVal), true);
                    }
                    else if (gameType == "timed")
                    {
                        await Navigation.PushAsync(new GamePage("timed", gameVal), true);
                    }
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Invalid Entry", ex.Message, "OK");
            }  
        }
    }
}