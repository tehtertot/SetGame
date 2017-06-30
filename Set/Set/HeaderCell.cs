using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Set
{
    public class HeaderCell : ViewCell
    {
        public HeaderCell()
        {
            this.Height = 30;
            var title = new Label
            {
                Font = Font.SystemFontOfSize(NamedSize.Small, FontAttributes.Bold),
                TextColor = Color.White,
                VerticalOptions = LayoutOptions.Center
            };

            title.SetBinding(Label.TextProperty, "Key");

            View = new StackLayout
            {
                HorizontalOptions = LayoutOptions.FillAndExpand,
                HeightRequest = 15,
                BackgroundColor = Color.ForestGreen,
                Padding = 5,
                Orientation = StackOrientation.Horizontal,
                Children = { title }
            };
        }
    }
}
