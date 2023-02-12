using AppCompras.ViewModels.Home;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AppCompras.Views.Home
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class HomePage : ContentPage
	{
        private HomePageViewModel _viewModel;

        public HomePage ()
		{
			InitializeComponent ();
		}
        protected override void OnAppearing()
        {
            base.OnAppearing();
            _viewModel = (HomePageViewModel)BindingContext;
            Container.Children.Add(_viewModel.CollectionProducts);
        }
        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();
            Container.Children.Clear();
            if(_viewModel != null ) 
                Container.Children.Add(_viewModel.CollectionProducts);
        }
        protected override void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            base.OnPropertyChanged(propertyName);
            if(propertyName == "CollectionProducts")
             {
                Container.Children.Clear();
                Container.Children.Add(_viewModel.CollectionProducts);

            }

        }
    }
}