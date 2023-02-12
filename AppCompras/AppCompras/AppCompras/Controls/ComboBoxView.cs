using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Xamarin.CommunityToolkit.UI.Views;
using Xamarin.CommunityToolkit.Extensions;
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration;

namespace AppCompras.Controls
{
    public class ComboBoxView : ContentView
    {
        //public static readonly System. EditStateChangedEvent = ComboBoxView.EditStateChangedEvent.AddOwner(typeof(ComboBoxView));
        //VisualElement//
        public static readonly BindableProperty ItemsSourceProperty =
        BindableProperty.Create(nameof(ItemsSource), typeof(IEnumerable), typeof(ComboBoxView), default(IEnumerable<object>),
                        BindingMode.TwoWay, null, propertyChanged: (bindable, oldvalue, newvalue) =>
                        {
                            if (bindable != null)
                            {
                                ((ComboBoxView)bindable).OnPropertyChanged();
                                
                            }
                        });

        public IEnumerable ItemsSource
        {
            get { return (IEnumerable)GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }

        public static readonly BindableProperty ItemSelectedProperty =
       BindableProperty.Create(nameof(ItemSelected), typeof(object), typeof(ComboBoxView), null, BindingMode.TwoWay, null, propertyChanged: (bindable, oldvalue, newvalue) =>
       {
           if (bindable != null)
           {
               ((ComboBoxView)bindable).OnPropertyChanged();
           }
       });
        public object ItemSelected
        {
            get { return (object)GetValue(ItemSelectedProperty); }
            set { SetValue(ItemSelectedProperty, value); }
        }

        public static readonly BindableProperty CornerRadiusProperty =
        BindableProperty.Create(nameof(CornerRadius), typeof(int), typeof(ComboBoxView), 0,
                        BindingMode.TwoWay, null, propertyChanged: (bindable, oldvalue, newvalue) =>
                        {
                            if (bindable != null)
                            {
                                ((ComboBoxView)bindable).OnPropertyChanged();
                            }
                        });

        public int CornerRadius
        {
            get { return (int)GetValue(CornerRadiusProperty); }
            set { SetValue(CornerRadiusProperty, value); }
        }
        public static readonly BindableProperty TitleProperty =
        BindableProperty.Create(
                    "Title", typeof(string), typeof(ComboBoxView), "Title", BindingMode.TwoWay);



        public string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }


        public static readonly BindableProperty TitleColorProperty =
        BindableProperty.Create(
                    "TitleColor", typeof(Color), typeof(ComboBoxView), Color.Black, BindingMode.TwoWay);



        public Color TitleColor
        {
            get { return (Color)GetValue(TitleColorProperty); }
            set { SetValue(TitleColorProperty, value); }
        }


        public new static readonly BindableProperty BackgroundColorProperty =
        BindableProperty.Create(
                    "BackgroundColor", typeof(Color), typeof(ComboBoxView), Color.White, BindingMode.TwoWay);


        public new Color BackgroundColor
        {
            get { return (Color)GetValue(BackgroundColorProperty); }
            set { SetValue(BackgroundColorProperty, value); }
        }


        public static readonly BindableProperty IsSelectedProperty =
         BindableProperty.Create("IsSelected", typeof(bool), typeof(ComboBoxView), false, BindingMode.TwoWay, null, propertyChanged: (bindable, oldvalue, newvalue) =>
         {
             if (bindable != null)
             {
                 ((ComboBoxView)bindable).OnPropertyChanged();
             }
         });
        public bool IsSelected
        {
            get { return (bool)GetValue(IsSelectedProperty); }
            set { SetValue(IsSelectedProperty, value); }
        }

        public static readonly BindableProperty ItemDataTemplateProperty =
         BindableProperty.Create("ItemDataTemplate", typeof(DataTemplate), typeof(ComboBoxView), null, BindingMode.TwoWay, null);
        public DataTemplate ItemDataTemplate
        {
            get { return (DataTemplate)GetValue(ItemDataTemplateProperty); }
            set { SetValue(ItemDataTemplateProperty, value); }
        }
        public static readonly BindableProperty LinearItemLayoutProperty =
        BindableProperty.Create("ItemDataTemplate", typeof(LinearItemsLayout), typeof(ComboBoxView), null, BindingMode.TwoWay, null);
        public LinearItemsLayout LinearItemLayout
        {
            get { return (LinearItemsLayout)GetValue(LinearItemLayoutProperty); }
            set { SetValue(LinearItemLayoutProperty, value); }
        }

        public ComboBoxView()
        {
            Padding = Padding == new Thickness(0) ? new Thickness(0, 10, 0, 0) : Padding;
            Content = GetGrid();
            LinearItemsLayoutDefauld.ItemSpacing = 10;

        }

        private Label LabelTitle;
        private Grid GridParen;
        private Label LabelIcon;
        private CollectionView Collection;
        private Frame FrameCollection;
        private DataTemplate DefaulDataTemplate = new DataTemplate(() =>
        {
            Label nameLabel = new Label
            {
                FontAttributes = FontAttributes.Bold,
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.Center,
                TextColor = Color.Black
            };
            nameLabel.SetBinding(Label.TextProperty, "Name");

            var _frame = new Frame()
            {
                HasShadow = false,
                CornerRadius = 15,
                BackgroundColor = Color.FromRgb(252, 188, 15),
                Padding = 5,
                VerticalOptions = LayoutOptions.Center,
                Content = nameLabel

            };
            var _grid = new Grid()
            {
                Margin = new Thickness(0, 2),
            };
            _grid.Children.Add(_frame);

            return _grid;
        });
        private LinearItemsLayout LinearItemsLayoutDefauld = new LinearItemsLayout(ItemsLayoutOrientation.Horizontal);

        protected override async void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            base.OnPropertyChanged(propertyName);
            if (propertyName == nameof(ItemsSource) && ItemsSource != null && LabelTitle.TranslationY == 0) 
            {
                    await Task.WhenAll(
                                        LabelTitle.ScaleTo(0.8, 10, Easing.Linear),
                                        LabelTitle.TranslateTo((LabelTitle.Width / 2) * (1 - LabelTitle.Scale) / 2, ((GridParen.Height / 2) - GridParen.Padding.Top) * -1, 10, Easing.Linear)
                                        );
                Collection.ItemsSource = ItemsSource;
                Collection.ItemTemplate =  ItemDataTemplate == null ? DefaulDataTemplate : ItemDataTemplate;
            }
            else if(propertyName == nameof(ItemSelected))
            {
                Collection.SelectedItem = ItemSelected;
            }
            else if(propertyName == nameof(CornerRadius))
            {
                FrameCollection.CornerRadius= CornerRadius;
            }
        }

        private Grid GetGrid()
        {
            GridParen = new Grid() { BackgroundColor = BackgroundColor };
            LabelTitle = new Label
            {
                Text = Title,
                Margin = new Thickness(10, 0),
                
                TextColor = TitleColor,
                Background = BackgroundColor,
                Scale = ItemsSource == null ? 1 : 0.8,
                TranslationY = ItemsSource == null ? 0 : ((Height - GridParen.Padding.Top) / 2) * -1,
                FontSize = 15,
                HorizontalOptions = LayoutOptions.Start,
                VerticalOptions = LayoutOptions.Center,
            };
            LabelTitle.TranslationX = ItemsSource == null ? 0 : (LabelTitle.Width * 0.2) / 2;

            var tapLabel = new TapGestureRecognizer();
            
            tapLabel.Tapped += async (sender, e) =>
            {
                if(LabelTitle.TranslationY == 0)
                await Task.WhenAll(
                    LabelTitle.ScaleTo(0.8, 10, Easing.Linear),
                    LabelTitle.TranslateTo((LabelTitle.Width / 2) * (1 - LabelTitle.Scale) / 2, ((GridParen.Height / 2) - GridParen.Padding.Top )* -1, 10, Easing.Linear)
                    );
            };
            
            Collection = new CollectionView
            {
                ItemsUpdatingScrollMode = ItemsUpdatingScrollMode.KeepLastItemInView,
                ItemsLayout = LinearItemLayout == null ? LinearItemsLayoutDefauld : LinearItemLayout,
                SelectionMode = SelectionMode.None,
                Margin = new Thickness(Height * 0.1, Height * 0.1, Height * 0.3, Height * 0.1),
                
                BackgroundColor = Color.Transparent

            };
           
            GridParen.Children.Add(Collection);

            LabelIcon = new Label
            {
                Text = Font.IconFont.Plus,
                FontSize = 20,
                FontAttributes = FontAttributes.Bold,
                FontFamily = "fonticons",
                HorizontalOptions = LayoutOptions.End,
                VerticalOptions = LayoutOptions.Center,
                IsTabStop = true,

            };


            FrameCollection = new Frame
            {
                HeightRequest = Height - 10,
                HasShadow = false,
                BorderColor = Color.Black,
                Padding = 5,
                WidthRequest = Width,
                Content = Collection,
                

            };
            FrameCollection.SetBinding(Frame.CornerRadiusProperty, nameof(CornerRadius));
            GridParen.Children.Add(FrameCollection);
            LabelTitle.GestureRecognizers.Add(tapLabel);
            GridParen.Children.Add(LabelTitle);

            LabelIcon.GestureRecognizers.Add(tapLabel);
            GridParen.Children.Add(LabelIcon);
            
            return GridParen;
        }


    }

}
