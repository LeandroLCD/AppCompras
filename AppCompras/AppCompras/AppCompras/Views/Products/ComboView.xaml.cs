using AppCompras.Views.Products;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
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
	public partial class ComboView : ContentView, INotifyPropertyChanged
	{

        #region Property
        public static readonly BindableProperty labelTitleProperty =
        BindableProperty.Create(
                    "labelTitle", typeof(string), typeof(ComboView), null, BindingMode.TwoWay);



        public string labelTitle
        {
            get { return (string)GetValue(labelTitleProperty); }
            set { SetValue(labelTitleProperty, value); }
        }

        public static readonly BindableProperty DisplayItemProperty =
        BindableProperty.Create(
                    "DisplayItem", typeof(string), typeof(ComboView), null, BindingMode.TwoWay);



        public string DisplayItem
        {
            get { return (string)GetValue(DisplayItemProperty); }
            set { SetValue(DisplayItemProperty, value); }
        }

        public static readonly BindableProperty ItemsSourceProperty = BindableProperty.Create(
                   nameof(ItemsSource),
                   typeof(IEnumerable),
                   typeof(ComboView),
                   null,
                   BindingMode.OneWay);

        

        public IEnumerable ItemsSource
        {
            get { return (IEnumerable)GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }


        public static readonly BindableProperty SelectItemCommandProperty =
        BindableProperty.Create(
                    "SelectItemCommand", typeof(Command<string>), typeof(ComboView), null, BindingMode.TwoWay);




        public Command<string> SelectItemCommand
        {
            get { return (Command<string>)GetValue(SelectItemCommandProperty); }
            set { SetValue(SelectItemCommandProperty, value); }
        }

        #endregion
        public ComboView ()
		{
            
            InitializeComponent ();
            BindingContext = this;
        }

        private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            entry.Focus();

        }
    }
}