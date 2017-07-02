using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Set
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            this.Title = "Set!";
            var toolbarItem = new ToolbarItem
            {
                Text = "My Scores"
            };
            toolbarItem.Clicked += async (sender, e) =>
            {
                await Navigation.PushAsync(new Scores());
            };
            ToolbarItems.Add(toolbarItem);
        }

        private async void StartSpeedGame(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new Selection("speed"), true);
            
        }

        private async void StartTimedGame(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new Selection("timed"), true);
        }

        private async void ShowInstructions(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new Instructions(), true);
        }
    }
}
