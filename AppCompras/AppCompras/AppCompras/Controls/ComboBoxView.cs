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
using System.Windows.Input;
using DevExpress.Data.Helpers;
using Xamarin.Forms.PancakeView;

namespace AppCompras.Controls
{
    public class ComboBoxView : ContentView
    {
        #region Fields
        private Label LabelTitle;
        private Grid GridParen;
        private Button ButtonIcon;
        private CollectionView Collection;
        private PancakeView PancakeViewCollection;
        private readonly DataTemplate DefaulDataTemplate = new DataTemplate(() =>
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
        private TapGestureRecognizer TapAddItem = new TapGestureRecognizer();

        #endregion

        #region Constructor

        public ComboBoxView()
        {
            Padding = Padding == new Thickness(0) ? new Thickness(0, 10, 0, 0) : Padding;
            Content = GetGrid();
            LinearItemsLayoutDefauld.ItemSpacing = 10;
        }
        #endregion

        #region Properties
        public static readonly BindableProperty ItemsSourceProperty =
        BindableProperty.Create(nameof(ItemsSource), typeof(IEnumerable), typeof(ComboBoxView), default(IEnumerable<object>),
                        BindingMode.TwoWay);

     

        public IEnumerable ItemsSource
        {
            get { return (IEnumerable)GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }

        public static readonly BindableProperty ItemSelectedProperty =
       BindableProperty.Create(nameof(ItemSelected), typeof(object), typeof(ComboBoxView), null, BindingMode.TwoWay, null);
        public object ItemSelected
        {
            get { return (object)GetValue(ItemSelectedProperty); }
            set { SetValue(ItemSelectedProperty, value); }
        }

        public static readonly BindableProperty CornerRadiusProperty =
        BindableProperty.Create(nameof(CornerRadius), typeof(CornerRadius), typeof(ComboBoxView), default(CornerRadius),
                        BindingMode.TwoWay, null, propertyChanged: (bindable, oldvalue, newvalue) =>
            {
                if (bindable != null)
                {
                    ((ComboBoxView)bindable).OnCornerRadiusChanged((CornerRadius)newvalue);
                }
            });

        

        public CornerRadius CornerRadius
        {
            get { return (CornerRadius)GetValue(CornerRadiusProperty); }
            set { SetValue(CornerRadiusProperty, value); }
        }

        public static readonly BindableProperty TitleProperty =
        BindableProperty.Create("Title", typeof(string), typeof(ComboBoxView), "Title", BindingMode.TwoWay, null,
            propertyChanged: (bindable, oldvalue, newvalue) =>
                    {
                        if (bindable != null)
                        {
                            ((ComboBoxView)bindable).OnTitleChanged((string)newvalue);
                        }
                    });
        

        public string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }


        public static readonly BindableProperty TitleColorProperty = BindableProperty.Create("TitleColor", typeof(Color), typeof(ComboBoxView), Color.Black, BindingMode.TwoWay, null, 
            propertyChanged: (bindable, oldvalue, newvalue) =>
                    {
                        if (bindable != null)
                        {
                            ((ComboBoxView)bindable).OnPropertyChanged();
                        }
                    });



        public Color TitleColor
        {
            get { return (Color)GetValue(TitleColorProperty); }
            set { SetValue(TitleColorProperty, value); }
        }


        public new static readonly BindableProperty BackgroundColorProperty = BindableProperty.Create("BackgroundColor", typeof(Color), typeof(ComboBoxView), Color.White, BindingMode.TwoWay, null, 
            propertyChanged: (bindable, oldvalue, newvalue) =>
                    {
                        if (bindable != null)
                        {
                            ((ComboBoxView)bindable).OnPropertyChanged();
                        }
                    });


        public new Color BackgroundColor
        {
            get { return (Color)GetValue(BackgroundColorProperty); }
            set { SetValue(BackgroundColorProperty, value); }
        }

        public static readonly BindableProperty BackgroundColorIconProperty = BindableProperty.Create("BackgroundColorIcon", 
            typeof(Color), typeof(ComboBoxView), Color.Red, BindingMode.TwoWay, null, propertyChanged: (bindable, oldvalue, newvalue) =>
            {
                if (bindable != null)
                {
                    ((ComboBoxView)bindable).OnColorIconChanged(bindable, oldvalue, newvalue);
                }
            });

        public Color BackgroundColorIcon
        {
            get { return (Color)GetValue(BackgroundColorIconProperty); }
            set { SetValue(BackgroundColorIconProperty, value); }
        }

        public static readonly BindableProperty IsSelectedProperty = BindableProperty.Create("IsSelected", typeof(bool), typeof(ComboBoxView), false, BindingMode.TwoWay, null, propertyChanged: (bindable, oldvalue, newvalue) =>
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

        public static readonly BindableProperty ItemDataTemplateProperty = BindableProperty.Create("ItemDataTemplate", 
            typeof(DataTemplate), typeof(ComboBoxView), null, BindingMode.TwoWay, null);

        

        public DataTemplate ItemDataTemplate
        {
            get { return (DataTemplate)GetValue(ItemDataTemplateProperty); }
            set { SetValue(ItemDataTemplateProperty, value); }
        }
        public static readonly BindableProperty LinearItemLayoutProperty = BindableProperty.Create("LinearItemLayout", typeof(LinearItemsLayout), typeof(ComboBoxView), null, BindingMode.TwoWay, null, propertyChanged: (bindable, oldvalue, newvalue) =>
        {
            if (bindable != null)
            {
                ((ComboBoxView)bindable).OnPropertyChanged();
            }
        });
        public LinearItemsLayout LinearItemLayout
        {
            get { return (LinearItemsLayout)GetValue(LinearItemLayoutProperty); }
            set { SetValue(LinearItemLayoutProperty, value); }
        }

        public static readonly BindableProperty SelectedItemProperty =
            BindableProperty.Create(nameof(SelectedItem), typeof(object), typeof(SelectableItemsView), default(object),
                defaultBindingMode: BindingMode.TwoWay);
        public object SelectedItem
        {
            get => GetValue(SelectedItemProperty);
            set => SetValue(SelectedItemProperty, value);
        }
        #endregion

        #region Command
        public static readonly BindableProperty SelectionChangedCommandProperty =
        BindableProperty.Create("SelectionChangedCommand", typeof(ICommand), typeof(ComboBoxView), null, BindingMode.TwoWay);
        public ICommand SelectionChangedCommand
        {
            get { return (ICommand)GetValue(SelectionChangedCommandProperty); }
            set { SetValue(SelectionChangedCommandProperty, value); }
        }

        public static readonly BindableProperty AddItemCommadProperty =
           BindableProperty.Create(nameof(AddItemCommad), typeof(ICommand), typeof(ComboBoxView), null, BindingMode.TwoWay);

        public ICommand AddItemCommad
        {
            get => (ICommand)GetValue(AddItemCommadProperty);
            set => SetValue(AddItemCommadProperty, value);
        }

        //public ICommand AddItemCommad
        //{
        //    get { return (ICommand)GetValue(AddItemCommadProperty); }
        //    set { SetValue(AddItemCommadProperty, value); }
        //}

        public static readonly BindableProperty SelectionChangedCommadParameterProperty = BindableProperty.Create(nameof(SelectionChangedParameter), typeof(object), typeof(ComboBoxView), null, BindingMode.TwoWay, null);
        public object SelectionChangedParameter
        {
            get { return (object)GetValue(SelectionChangedCommadParameterProperty); }
            set { SetValue(SelectionChangedCommadParameterProperty, value); }
        }


        #endregion

        #region Methods


        private Grid GetGrid()
        {
            GridParen = new Grid();
            var gridFrame = new Grid()
            {
                ColumnDefinitions =
                {
                new ColumnDefinition(),
                new ColumnDefinition(){ Width = new GridLength(30) }
                }
            };
            GridParen.SetBinding(Grid.BackgroundColorProperty, nameof(BackgroundColor));
            gridFrame.SetBinding(Grid.BackgroundColorProperty, nameof(BackgroundColor));
            LabelTitle = new Label
            {
                Text = Title,
                Margin = new Thickness(10, 0),
                Scale = ItemsSource == null ? 1 : 0.8,
                TranslationY = ItemsSource == null ? 0 : ((Height - GridParen.Padding.Top) / 2) * -1,
                FontSize = 15,
                HorizontalOptions = LayoutOptions.Start,
                VerticalOptions = LayoutOptions.Center,
            };
            LabelTitle.SetBinding(Label.TextProperty, nameof(Title));
            LabelTitle.TranslationX = ItemsSource == null ? 0 : (LabelTitle.Width * 0.2) / 2;

            
            var tapLabelTitle = new TapGestureRecognizer();
            tapLabelTitle.Tapped += async (sender, e) =>
            {
                if (LabelTitle.TranslationY == 0)
                    await Task.WhenAll(
                        LabelTitle.ScaleTo(0.8, 10, Easing.Linear),
                        LabelTitle.TranslateTo((LabelTitle.Width / 2) * (1 - LabelTitle.Scale) / 2, ((GridParen.Height / 2) - GridParen.Padding.Top) * -1, 10, Easing.Linear)
                        );
            };

             

             Collection = new CollectionView
            {
                ItemsUpdatingScrollMode = ItemsUpdatingScrollMode.KeepLastItemInView,
                ItemsLayout = LinearItemLayout == null ? LinearItemsLayoutDefauld : LinearItemLayout,
                ItemTemplate = ItemDataTemplate == null ? DefaulDataTemplate : ItemDataTemplate,
                SelectionMode = SelectionMode.Single,
                BackgroundColor = Color.Transparent,
                Margin = new Thickness(5, 10, 5, 5)

             };
            Collection.BindingContext = this;
            Collection.SetBinding(CollectionView.ItemsSourceProperty, nameof(ComboBoxView.ItemsSource));
            Collection.SetBinding(CollectionView.SelectedItemProperty, nameof(ComboBoxView.SelectedItem));
            
            gridFrame.Children.Add(Collection);

            ButtonIcon = new Button
            {
                
                Text = Font.IconFont.Plus,
                FontSize = 15,
                Padding=0,
                TextTransform = TextTransform.None,
                FontFamily = "fonticons",
            };
            ButtonIcon.BindingContext = this;
            ButtonIcon.SetBinding(Button.CommandProperty, nameof(AddItemCommad), BindingMode.TwoWay);
            ButtonIcon.Clicked += async (sender, e) =>
            {
                if (LabelTitle.TranslationY == 0)
                    await Task.WhenAll(
                        LabelTitle.ScaleTo(0.8, 10, Easing.Linear),
                        LabelTitle.TranslateTo((LabelTitle.Width / 2) * (1 - LabelTitle.Scale) / 2, ((GridParen.Height / 2) - GridParen.Padding.Top) * -1, 10, Easing.Linear)
                        );
            };
            gridFrame.Children.Add(ButtonIcon, left: 1, right: 2, top: 0, bottom: 1);

            PancakeViewCollection = new PancakeView
            {
                HeightRequest = Height - 10,
                Border = new Border()
                {
                    Color = Color.Black,
                    Thickness = 1,
                    DrawingStyle = BorderDrawingStyle.Inside,
                    
                },
                CornerRadius = CornerRadius,
                WidthRequest = WidthRequest,
                Content = gridFrame,
                

            };

             GridParen.Children.Add(PancakeViewCollection);
            LabelTitle.GestureRecognizers.Add(tapLabelTitle);
            GridParen.Children.Add(LabelTitle);

            
            
            return GridParen;
        
        }
        protected override async void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            base.OnPropertyChanged(propertyName);
            if(propertyName == nameof(ItemDataTemplate))
            {
                Collection.ItemTemplate = ItemDataTemplate == null ? DefaulDataTemplate: ItemDataTemplate;
            }
            if (propertyName == nameof(ItemsSource))
            {
                if (LabelTitle.TranslationY == 0 && LabelTitle.Width > 1)
                    await Task.WhenAll(
                        LabelTitle.ScaleTo(0.8, 10, Easing.Linear),
                        LabelTitle.TranslateTo((LabelTitle.Width / 2) * (1 - LabelTitle.Scale) / 2, ((GridParen.Height / 2) - GridParen.Padding.Top) * -1, 10, Easing.Linear)
                        );
            }
            if (propertyName == nameof(SelectedItem) && SelectionChangedCommand != null)
            {
                if (SelectionChangedParameter == null)
                {
                    SelectionChangedCommand.CanExecute(SelectedItem);
                    SelectionChangedCommand.Execute(SelectedItem);
                }
                else
                {
                    SelectionChangedCommand.CanExecute(SelectionChangedParameter);
                    SelectionChangedCommand.Execute(SelectionChangedParameter);
                }

            }
            if(propertyName == nameof(TitleColor))
            {
                LabelTitle.TextColor = TitleColor;
                
                
            }
            if(propertyName == nameof(BackgroundColor))
            {
                LabelTitle.Background = BackgroundColor;
                PancakeViewCollection.BackgroundColor = BackgroundColor;
            }
        }
        private void OnColorIconChanged(BindableObject bindable, object oldvalue, object newvalue)
        {
            ButtonIcon.BackgroundColor = (Color)newvalue;
        }
        
        
        
        private void OnCornerRadiusChanged(CornerRadius newvalue)
        {
            PancakeViewCollection.CornerRadius = newvalue;
        }
        private void OnTitleChanged(string newvalue)
        {
            LabelTitle.Text = newvalue;
            LabelTitle.Background = BackgroundColor;
        }
        #endregion

    }

}
