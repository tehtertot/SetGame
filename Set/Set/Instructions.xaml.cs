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
	public partial class Instructions : ContentPage
	{
		public Instructions ()
		{
			InitializeComponent ();
            this.Title = "Set: Instructions";
		}
	}
}