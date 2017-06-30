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
    public partial class Scores : ContentPage
    {
        public Scores()
        {
            InitializeComponent();
            NavigationPage.SetHasBackButton(this, false);
            this.Title = "All Scores";

            var toolbarItem = new ToolbarItem
            {
                Text = "New Game"
            };
            toolbarItem.Clicked += async (sender, e) =>
            {
                await Navigation.PushAsync(new GamePage());
            };
            ToolbarItems.Add(toolbarItem);
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();
            AllScores.ItemsSource = await App.Database.GetScoresAsync();
        }
    }
}