using AppCompras.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AppCompras.Views.Products
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EntryView : ContentView
    {
        #region Properties
        public static readonly BindableProperty labelTitleProperty =
        BindableProperty.Create(
                    "labelTitle", typeof(string), typeof(EntryView), null,BindingMode.TwoWay);

        

        public string labelTitle
        {
            get { return (string)GetValue(labelTitleProperty); }
            set { SetValue(labelTitleProperty, value); }
        }

        public static readonly BindableProperty CommandChangedProperty =
        BindableProperty.Create(
                    "CommandChanged", typeof(Command<string>), typeof(EntryView), null, BindingMode.TwoWay);




        public Command<string> CommandChanged
        {
            get { return (Command<string>)GetValue(CommandChangedProperty); }
            set { SetValue(CommandChangedProperty, value); }
        }

        public static readonly BindableProperty CommandChangedParameterProperty =
        BindableProperty.Create(
                    "CommandChangedParameter", typeof(string), typeof(EntryView), null, BindingMode.TwoWay);




        public string CommandChangedParameter
        {
            get { return (string)GetValue(CommandChangedParameterProperty); }
            set { SetValue(CommandChangedParameterProperty, value); }
        }

        public static readonly BindableProperty entryTextProperty =
        BindableProperty.Create(
                    "entryText", typeof(string), typeof(EntryView), null, BindingMode.TwoWay);




        public string entryText
        {
            get { return (string)GetValue(entryTextProperty); }
            set { SetValue(entryTextProperty, value); }
        }

        #endregion
        public EntryView()
        {
            BindingContext= this;
            InitializeComponent();
            
        }

        private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            entry.Focus();
        }
    }
}