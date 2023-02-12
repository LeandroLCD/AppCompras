using AppCompras.Views.Home;
using AppCompras.Views.Login;
using Plugin.SharedTransitions;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AppCompras.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ShellPage : SharedTransitionShell
    {
        public ShellPage()
        {
            InitializeComponent();

            RegisterRoutes();
        }

        private void RegisterRoutes()
        {
            Routing.RegisterRoute(nameof(HomePage), typeof(HomePage));
            Routing.RegisterRoute(nameof(LoginPage), typeof(LoginPage));
            Routing.RegisterRoute(nameof(AddBaskedPage), typeof(AddBaskedPage));

        }
    }
}