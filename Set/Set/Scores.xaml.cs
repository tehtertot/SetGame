using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        public ObservableCollection<ScoreGrouping<string, Score>> ScoresGrouped { get; set; }

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
                await Navigation.PushAsync(new MainPage());
            };
            ToolbarItems.Add(toolbarItem);
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();
            //AllScores.ItemsSource = await App.Database.GetSpeedScoresAsync();
            var All = await App.Database.GetAllScoresAsync();
            var sorted = from score in All
                         orderby score.OrderBy
                         group score by score.Type into scoreGroup
                         select new ScoreGrouping<string, Score>(scoreGroup.Key, scoreGroup);
            ScoresGrouped = new ObservableCollection<ScoreGrouping<string, Score>>(sorted);

            AllScores.ItemsSource = ScoresGrouped;
            AllScores.IsGroupingEnabled = true;
            AllScores.GroupDisplayBinding = new Binding("Key");
            AllScores.GroupHeaderTemplate = new DataTemplate(typeof(HeaderCell));
            AllScores.HasUnevenRows = true;
        }
    }
}