using AppCompras.ViewModels.Home;
using AppCompras.ViewModels.Product;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AppCompras.Views.Products
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CategoryPage : ContentPage
    {
        private CategoryPageViewModel _viewModel;

        public CategoryPage()
        {
            InitializeComponent();
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            _viewModel = (CategoryPageViewModel)BindingContext;
            _viewModel.CollectionCategory(container);
        }
    }
}