using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;

namespace Set
{
    public partial class App : Application
    {

        static ScoreDatabase database;

        public App()
        {
            InitializeComponent();

            MainPage = new NavigationPage(new MainPage());
        }

        public static ScoreDatabase Database
        {
            get
            {
                if (database == null)
                {
                    database = new ScoreDatabase(DependencyService.Get<IFileHelper>().GetLocalFilePath("scores.db3"));
                }
                return database;
            }
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
