using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.CommunityToolkit.Extensions;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AppCompras.Views.Products
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddProductPage : ContentPage
    {
        public AddProductPage()
        {
            InitializeComponent();


        }

        private void ComboBoxView_Focused(object sender, FocusEventArgs e)
        {

        }

        private void ListView_Focused(object sender, FocusEventArgs e)
        {

        }
    }
}