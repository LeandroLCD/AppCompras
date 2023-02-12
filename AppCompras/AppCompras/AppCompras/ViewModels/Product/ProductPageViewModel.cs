using AppCompras.Models.Product;
using AppCompras.Views.Home;
using AppCompras.Views.Products;
using Plugin.SharedTransitions;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace AppCompras.ViewModels.Product
{
    public class ProductPageViewModel: BaseViewModel
    {
        #region Fields

        private Grid _collectionProducts;
        private static Products _newProduct;
        private List<Products> _getProdducts;
        private bool _isVisibleModal;
        #endregion

        #region Ctor
        public ProductPageViewModel()
        {
            InicializeProperties();
        }
        #endregion

        #region Properties
        public Grid CollectionProducts
        {
            get => _collectionProducts;
            set => SetProperty(ref _collectionProducts, value);
        }
        /// <summary>
        /// Selecciona un producto para enviarlo a la vista AddBasket
        /// </summary>
        public static Products ProductSelect
        {
            get => _newProduct;
            set => _newProduct = value;

        }
        public List<Products> GetProducts
        {
            get => _getProdducts;
            set => SetProperty(ref _getProdducts, value);

        }
        public bool IsVicibleModal
        {
            get => _isVisibleModal;
            set => SetProperty(ref _isVisibleModal, value);
        }
        
        #endregion

        #region Command

        public Command AddProductCommand { get; set; }

        public Command ShowModalCommand { get; set; }
        #endregion

        #region Methods
        private async void InicializeProperties()
        {
            #region Command
            ShowModalCommand = new Command(() => IsVicibleModal = !IsVicibleModal);
            AddProductCommand = new Command(async () =>
            {
                
                await Shell.Current.GoToAsync(nameof(AddProductPage));
            } );

            #endregion
            var result = await _serviceFireBase.GetProducts();
            if(result.Success)
            {
                GetProducts = new List<Products>((IEnumerable<Products>)result.Object);
                CollectionProduct(GetProducts);
            }
            
        }


        /// <summary>
        /// Carga el Grid con la coleccion d eproductos para la vista.
        /// </summary>
        /// <param name="Models">Es una lista de tipo Product</param>

        private void CollectionProduct(List<Products> Models)
        {
            if (Models.Count == 0)
                CollectionProducts.Children.Add( new StackLayout
                {

                    Children = { new Label
                    {
                        Text = $"Sin productos en Creados.",
                        FontSize = 15,
                        VerticalOptions = LayoutOptions.CenterAndExpand,
                        HorizontalOptions = LayoutOptions.CenterAndExpand

                    } }

                });

            StackLayout Left = new StackLayout() { Padding = new Thickness(10) };

            StackLayout Rigth = new StackLayout() { Padding = new Thickness(10), Margin= new Thickness(0,60,0,0) };


            foreach (var item in Models)
            {
                ProductPlot(item, Models.IndexOf(item), Left, Rigth);
            }

            CollectionProducts.Children.Add(new StackLayout
            {
                Orientation = StackOrientation.Horizontal,
                Children = { Left, Rigth }

            });
        }

        private void ProductPlot(Products item, int index, StackLayout Left, StackLayout Rigth)
        {
            var _islayout = Convert.ToBoolean(index % 2);

            var _layout = _islayout ? Left : Rigth;

            var stack = new StackLayout
            {
                Children =
                {
                    new Image
                    {
                        Source = item.GetImageSource[0].Source,
                        HeightRequest = 100,
                        HorizontalOptions = LayoutOptions.Center,
                        Margin = new Thickness(0, 5)
                    },
                    new Label
                    {
                        Text = "$" + item.PriceBruto,
                        FontAttributes = FontAttributes.Bold,
                        FontSize = 22,
                        Margin = new Thickness(0, 6),
                        TextColor = Color.FromHex("#333333")
                    },
                     new Label
                     {
                        Text = item.Description,
                        FontSize = 16,
                        TextColor = Color.Black,
                        CharacterSpacing = 1
                     },
                     new Label
                     {
                        Text = $"{item.Conten}-{item.UdM}",
                        FontSize = 13,
                        TextColor = Color.FromHex("#CCCCCC"),
                        CharacterSpacing = 1,
                        Margin = new Thickness(0, 0, 0, 5)
                     },

                }
            };

            var tap = new TapGestureRecognizer();

            tap.Tapped += async (object sender, EventArgs e) =>
            {
                var page = (App.Current.MainPage as SharedTransitionShell).CurrentPage;
                SharedTransitionShell.SetBackgroundAnimation(page, BackgroundAnimation.SlideFromRight);
                SharedTransitionShell.SetTransitionDuration(page, 1000);
                SharedTransitionShell.SetTransitionSelectedGroup(page, item.Id);
                ProductSelect = item;
                await Shell.Current.GoToAsync(nameof(AddProductPage));

            };

            stack.GestureRecognizers.Add(tap);

            _layout.Children.Add(new Frame
            {
                HeightRequest = 200,
                CornerRadius = 10,
                Margin = 8,
                HasShadow = false,
                BackgroundColor = Color.White,
                Padding = 30,
                Content = stack
            });

        }
        #endregion
    }
}
