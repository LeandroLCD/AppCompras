using AppCompras.Views.Products;
using Foundation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.BehaviorsPack;

namespace AppCompras.Controls
{
    public class EntryView: Grid
    {
        #region Fields
        private Grid _gridParen;
        private StandardEntry _standardEntry;
        private Label _labelTitle;
        #endregion

        #region Constructor
        public EntryView()
        {
            Padding = new Thickness(0, 10, 0, 0);
            Children.Add(PlotConten());
        }
        #endregion

        #region Command
        public static readonly BindableProperty TextChangedCommandProperty = 
            BindableProperty.Create(nameof(TextChangedCommand), typeof(ICommand), typeof(StandardEntry));
        
        public ICommand TextChangedCommand
        {
            get => (ICommand)GetValue(TextChangedCommandProperty); 
            set => SetValue(TextChangedCommandProperty, value);
        }

        public static readonly BindableProperty TextChangedCommandParameterProperty = 
            BindableProperty.Create(nameof(TextChangedCommandParameter), typeof(object), typeof(StandardEntry));

        public ICommand TextChangedCommandParameter
        {
            get => (ICommand)GetValue(TextChangedCommandParameterProperty);
            set => SetValue(TextChangedCommandParameterProperty, value);
        }
        
        #endregion

        #region Property
        public static readonly BindableProperty CornerRadiusProperty =
        BindableProperty.Create(
            nameof(CornerRadius), 
            typeof(int), 
            typeof(EntryView), 
            0,
            BindingMode.TwoWay,
            null, 
            propertyChanged: (bindable, oldvalue, newvalue) =>
                        {
                            if (bindable != null)
                            {
                                ((EntryView)bindable).OnCornerRadiusChanged(newvalue);
                            }
                        });

        public int CornerRadius
        {
            get { return (int)GetValue(CornerRadiusProperty); }
            set { SetValue(CornerRadiusProperty, value); }
        }
        


        public static readonly BindableProperty BorderWidthProperty =
        BindableProperty.Create(nameof(BorderWidth), typeof(int), typeof(EntryView), 0,
                        BindingMode.TwoWay, null, propertyChanged: (bindable, oldvalue, newvalue) =>
                        {
                            if (bindable != null)
                            {
                                ((EntryView)bindable).OnBorderWidthChanged(newvalue);
                            }
                        });

        public int BorderWidth
        {
            get { return (int)GetValue(BorderWidthProperty); }
            set { SetValue(BorderWidthProperty, value); }
        }





        public static readonly BindableProperty TitleProperty =
        BindableProperty.Create(
            nameof(Title), 
            typeof(string), 
            typeof(EntryView), 
            "Title", 
            BindingMode.TwoWay, 
            null, 
            propertyChanged: (bindable, oldvalue, newvalue) =>
            {
                if (bindable != null)
                {
                    ((EntryView)bindable).OnTitleChanged(newvalue);
                }
            });

       

        public string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }

        public static readonly BindableProperty TitleColorProperty = 
            BindableProperty.Create("TitleColor", typeof(Color), typeof(EntryView), Color.Black, BindingMode.TwoWay, null, 
                propertyChanged: (bindable, oldvalue, newvalue) =>
                {
                    if (bindable != null)
                    {
                        ((EntryView)bindable).OnTitleColorChanged((Color)newvalue);
                    }
                });

        public Color TitleColor
        {
            get { return (Color)GetValue(TitleColorProperty); }
            set { SetValue(TitleColorProperty, value); }
        }


        public new static readonly BindableProperty BackgroundColorProperty = 
            BindableProperty.Create("BackgroundColor", typeof(Color), typeof(EntryView), Color.White, BindingMode.TwoWay, null, 
                propertyChanged: (bindable, oldvalue, newvalue) =>
                {
                    if (bindable != null)
                    {
                        ((EntryView)bindable).OnBackgroundColoChanged(newvalue);
                    }
                });

        public new Color BackgroundColor
        {
            get { return (Color)GetValue(BackgroundColorProperty); }
            set { SetValue(BackgroundColorProperty, value); }
        }

        public static readonly BindableProperty IsReadOnlyProperty = 
            BindableProperty.Create("IsReadOnly", typeof(bool), typeof(EntryView), false, BindingMode.OneWay, null, 
                propertyChanged: (bindable, oldvalue, newvalue) =>
                {
                    if (bindable != null)
                    {
                        ((EntryView)bindable).OnIsReadOnlyChanged(newvalue);
                    }
                });

        public bool IsReadOnly
        {
            get => (bool)GetValue(IsReadOnlyProperty);
            set => SetValue(IsReadOnlyProperty, value);
        }

        public static readonly BindableProperty TextProperty = 
            BindableProperty.Create("Text", 
                typeof(string), 
                typeof(EntryView), 
                null, 
                BindingMode.TwoWay, 
                null, 
                propertyChanged: (bindable, oldValue, newValue) =>
                {
                    if (bindable != null)
                    {
                        ((EntryView)bindable).OnTextChanged((string)oldValue, (string)newValue);
                    }
                });

        public string Text
        {
            get => (string)GetValue(TextProperty);
            set => SetValue(TextProperty, value);
        }


        public static readonly BindableProperty VerticalTextAlignmentProperty =
            BindableProperty.Create(nameof(VerticalTextAlignment),
                typeof(TextAlignment),
                typeof(EntryView),
                TextAlignment.End,
                BindingMode.TwoWay,
                null,
                propertyChanged: (bindable, oldValue, newValue) =>
                {
                    if (bindable != null)
                    {
                        ((EntryView)bindable).OnVerticalTextAlignmentChanged((TextAlignment)oldValue, (TextAlignment)newValue);
                    }
                });


        public TextAlignment VerticalTextAlignment
        {
            get => (TextAlignment)GetValue(VerticalTextAlignmentProperty);
            set => SetValue(VerticalTextAlignmentProperty, value);
        }

        public static readonly BindableProperty HorizontalTextAlignmentProperty =
            BindableProperty.Create(nameof(HorizontalTextAlignment),
                typeof(TextAlignment),
                typeof(EntryView),
                TextAlignment.Center,
                BindingMode.TwoWay,
                null,
                propertyChanged: (bindable, oldValue, newValue) =>
                {
                    if (bindable != null)
                    {
                        ((EntryView)bindable).OnHorizontalTextAlignmentChanged((TextAlignment)oldValue, (TextAlignment)newValue);
                    }
                });


        public TextAlignment HorizontalTextAlignment
        {
            get => (TextAlignment)GetValue(HorizontalTextAlignmentProperty);
            set => SetValue(HorizontalTextAlignmentProperty, value);
        }

        
        public static readonly BindableProperty BorderColorProperty = 
            BindableProperty.Create("BorderColor", typeof(Color), typeof(EntryView), Color.Black, BindingMode.OneWayToSource, null, 
                propertyChanged: (bindable, oldvalue, newvalue) =>
                {
                    if (bindable != null)
                    {
                        ((EntryView)bindable).OnBorderColorChanged((Color)newvalue);
                    }
                });

        

        public Color BorderColor
        {
            get { return (Color)GetValue(BorderColorProperty); }
            set { SetValue(BorderColorProperty, value); }
        }

        public static readonly BindableProperty TextColorProperty = 
            BindableProperty.Create("TextColor", typeof(Color), typeof(EntryView), Color.Black, BindingMode.TwoWay, null, 
                propertyChanged: (bindable, oldvalue, newvalue) =>
                {
                    if (bindable != null)
                    {
                        ((EntryView)bindable).OnTextColorChanged((Color)newvalue);
                    }
                });


        public Color TextColor
        {
            get { return (Color)GetValue(TextColorProperty); }
            set { SetValue(TextColorProperty, value); }
        }

        new public static readonly BindableProperty HeightRequestProperty = 
            BindableProperty.Create("HeightRequest", typeof(double), typeof(EntryView), double.Parse("60"), BindingMode.TwoWay, null, 
                propertyChanged: (bindable, oldvalue, newvalue) =>
                {
                    if (bindable != null)
                    {
                        ((EntryView)bindable).OnHeightRequestChanged((double)newvalue);
                    }
                });

        

        new public double HeightRequest
        {
            get => (double)GetValue(HeightRequestProperty);
            set => SetValue(HeightRequestProperty, value);
        }
        public static readonly BindableProperty FontSizeProperty = 
            BindableProperty.Create("FontSize", typeof(double), typeof(EntryView), double.Parse("15"), BindingMode.TwoWay, null, 
                propertyChanged: (bindable, oldvalue, newvalue) =>
                {
                    if (bindable != null)
                    {
                        ((EntryView)bindable).OnFontSizePropertyChanged((double)newvalue);
                    }
                });

        

        public double FontSize
        {
            get => (double)GetValue(FontSizeProperty);
            set => SetValue(FontSizeProperty, value);
        }

        public static readonly BindableProperty KeyboardProperty =
        BindableProperty.Create(nameof(Keyboard), typeof(Keyboard), typeof(EntryView), Keyboard.Default,
                        BindingMode.TwoWay, null, propertyChanged: (bindable, oldvalue, newvalue) =>
                        {
                            if (bindable != null)
                            {
                                ((EntryView)bindable).OnKeyboardChanged((Keyboard)newvalue);
                            }
                        });

        
        public Keyboard Keyboard
        {
            get { return (Keyboard)GetValue(KeyboardProperty); }
            set { SetValue(KeyboardProperty, value); }
        }

        public static readonly BindableProperty IsClearTextFocusProperty = 
            BindableProperty.Create("IsClearTextFocus", typeof(bool), typeof(EntryView), false, BindingMode.OneWay, null);

       

        public bool IsClearTextFocus
        {
            get => (bool)GetValue(IsClearTextFocusProperty);
            set => SetValue(IsClearTextFocusProperty, value);
        }

        #endregion

        #region Methods

        private Grid PlotConten()
        {
            var tapLabel = new TapGestureRecognizer();

            
            _gridParen = new Grid()
            {
                VerticalOptions = LayoutOptions.FillAndExpand,
                HorizontalOptions= LayoutOptions.FillAndExpand,
            };
            _gridParen.SetBinding(Grid.BackgroundColorProperty, nameof(EntryView.BackgroundColor), BindingMode.OneWay);
            _standardEntry = new StandardEntry()
            {
                Padding = new Thickness(15, 5, 5, 15),
                BorderColor = BorderColor,
                BorderThickness = BorderWidth,
                TextColor= TextColor,
                VerticalTextAlignment = VerticalTextAlignment,
                HorizontalTextAlignment= HorizontalTextAlignment,
                FontSize = FontSize,
                CornerRadius = CornerRadius,
                HeightRequest= HeightRequest -10,
                VerticalOptions = LayoutOptions.End,
                HorizontalOptions= LayoutOptions.FillAndExpand,
                BackgroundColor = BackgroundColor
            };
            _standardEntry.Focused += async (sender, e) => {
                if(_standardEntry.IsFocused)
                {
                    AnimateLabelTitle();
                    _standardEntry.CursorPosition = Text == null ? 0 : Text.Length;
                }
                if (!_standardEntry.IsFocused && _standardEntry.Text.Length == 0)
                {
                    await Task.WhenAll(
                   _labelTitle.ScaleTo(0, 10, Easing.Linear),
                   _labelTitle.TranslateTo(0, 0, 10, Easing.Linear)
                   );
                }
                else if(_standardEntry.IsFocused && IsClearTextFocus)
                {
                    _standardEntry.Text = string.Empty;
                }
            };
            _standardEntry.SetBinding(StandardEntry.BackgroundColorProperty, nameof(EntryView.BackgroundColor), BindingMode.OneWay);
            _labelTitle = new Label()
            {
                HorizontalOptions= LayoutOptions.StartAndExpand,
                VerticalOptions= LayoutOptions.Center,
                VerticalTextAlignment = TextAlignment.Start,
                FontSize = FontSize,
                Background = BackgroundColor,
                Margin = new Thickness(10,0)
            };
            tapLabel.Tapped +=  (sender, e) =>
            {
                AnimateLabelTitle();
                _standardEntry.CursorPosition = Text == null ? 0: Text.Length;
                _standardEntry.Focus();
            };


            _standardEntry.BindingContext = this;
            _standardEntry.SetBinding(StandardEntry.TextProperty, nameof(EntryView.Text), BindingMode.TwoWay);
            
            _labelTitle.GestureRecognizers.Add(tapLabel);
            _gridParen.Children.Add(_standardEntry);
            _gridParen.Children.Add(_labelTitle);


            return _gridParen;
        }

        protected override void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            base.OnPropertyChanged(propertyName);
        }
        private void OnTitleColorChanged(Color newvalue)
        {
            _labelTitle.TextColor = newvalue;
        }

        private void OnCornerRadiusChanged(object newvalue)
        {
            _standardEntry.CornerRadius = (int)newvalue;
        }

        private void OnBorderWidthChanged(object newvalue)
        {
            _standardEntry.BorderThickness = (int)newvalue;
        }
        private void OnTitleChanged(object newvalue)
        {
            _labelTitle.Text = (string)newvalue;
        }

        private void OnBackgroundColoChanged(object newvalue)
        {
            _gridParen.BackgroundColor = (Color)newvalue;
            _labelTitle.Background= (Color)newvalue;
            _standardEntry.BackgroundColor = (Color)newvalue;
        }
        private void OnIsReadOnlyChanged(object newValue)
        {
            _standardEntry.IsReadOnly = (bool)newValue;
        }
        private async void AnimateLabelTitle()
        {
            if (_labelTitle.TranslationY == 0)
                await Task.WhenAll(
                    _labelTitle.ScaleTo(0.8, 10, Easing.Linear),
                    _labelTitle.TranslateTo((_labelTitle.Width / 2) * (1 - _labelTitle.Scale) / 2, ((HeightRequest - Padding.Top) / 2) * -1, 10, Easing.Linear)
                    );
            
        }
        public event EventHandler<TextChangedEventArgs> TextChanged;
        private void OnTextChanged(string oldValue, string newValue)
        {
            TextChanged?.Invoke(this, new TextChangedEventArgs(oldValue, newValue));
            

            //Execute command
            if (TextChangedCommand != null)
            {
                if (TextChangedCommand.CanExecute(TextChangedCommand))
                {
                    TextChangedCommand.Execute(TextChangedCommand);
                }
            }
            if(Text != null)
            {
                _standardEntry.CursorPosition = Text.Length;
                AnimateLabelTitle();

            }
            
        }

        private void OnBorderColorChanged(Color newvalue)
        {
            _standardEntry.BorderColor = newvalue;
        }


        private void OnTextColorChanged(Color newvalue)
        {
            _standardEntry.TextColor = newvalue;
        }
        private void OnHorizontalTextAlignmentChanged(TextAlignment oldValue, TextAlignment newValue)
        {
            _standardEntry.HorizontalTextAlignment = newValue;
        }
        private void OnVerticalTextAlignmentChanged(TextAlignment oldValue, TextAlignment newValue)
        {
            _standardEntry.VerticalTextAlignment = newValue;
        }
        private void OnHeightRequestChanged(double newvalue)
        {
        }

        private void OnFontSizePropertyChanged(double newvalue)
        {
            _standardEntry.FontSize = newvalue;
            _labelTitle.FontSize = newvalue;
        }

        private void OnKeyboardChanged(Keyboard newvalue)
        {
            _standardEntry.Keyboard = newvalue;
        }
       
        #endregion
    }
}
