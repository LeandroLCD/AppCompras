using AppCompras.Models.Product;
using AppCompras.Views.Home;
using Plugin.SharedTransitions;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace AppCompras.ViewModels.Home

{
    public class HomePageViewModel: BaseViewModel
    {
        
        #region Fields
        private bool _isVisibleBasket;
        private StackLayout _collectionProducts;
        private static Products _productSelected;
        private List<Products> _getProdducts;
        #endregion

        #region Ctor
        public HomePageViewModel()
        {
            CollectionProducts = CollectionProduct(new List<Products>());
        }
        #endregion

        #region Command
        public Command IsBaskedCommand => new Command(() =>
        {
            IsVisibleBasked = !IsVisibleBasked;
        });
        #endregion

        #region Properties

        public StackLayout CollectionProducts
        {
            get => _collectionProducts;
            set => SetProperty(ref _collectionProducts, value);
        }
        /// <summary>
        /// Controla la visiblidad del carrito de compras
        /// </summary>
        public bool IsVisibleBasked
        { 
            get => _isVisibleBasket;
            set => SetProperty(ref _isVisibleBasket, value);
        }
        /// <summary>
        /// Selecciona un producto para enviarlo a la vista AddBasket
        /// </summary>
        public static Products ProductSelected
        {
            get => _productSelected;
            set => _productSelected = value;

        }
        public List<Products> GetProducts
        {
            get => _getProdducts;
            set => SetProperty(ref _getProdducts, value);

        }
        #endregion

        #region Methods
        /// <summary>
        /// Retorna un StackLayout con con coleccion d eproductos para la vista.
        /// </summary>
        /// <param name="Models">Es una lista de tipo Product</param>
        /// <returns></returns>
        private StackLayout CollectionProduct(List<Products> Models)
        {
            if(Models.Count == 0)
                return new StackLayout
                {
                    
                    Children = { new Label 
                    { 
                        Text = $"Sin productos en esta categoría.",
                        FontSize = 15,
                        VerticalOptions = LayoutOptions.CenterAndExpand,
                        HorizontalOptions = LayoutOptions.CenterAndExpand

                    } }

                };
            StackLayout Left = new StackLayout() { Padding= new Thickness(10)};

            StackLayout Rigth = new StackLayout() { Padding = new Thickness(10) } ;

            Rigth.Children.Add((new BoxView() { HeightRequest = 60}));

            foreach (var item in Models)
            {
                ProductPlot(item, Models.IndexOf(item), Left, Rigth);
            }

            return new StackLayout 
            {
                Orientation = StackOrientation.Horizontal,
                Children= {Left, Rigth}
            
            };
        }

        private void ProductPlot(Products item, int index, StackLayout Left, StackLayout Rigth)
        {
            var _islayout = Convert.ToBoolean(index % 2);

            var _layout = _islayout ? Left : Rigth;

            var stack = new StackLayout
            {
                Children=
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
                        Text = $"{item.TotalPrice:C2}",
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
                ProductSelected = item;
                await Shell.Current.GoToAsync(nameof(AddBaskedPage));

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
