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
        }

        private async void StartGame(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new GamePage());
        }
    }
}
