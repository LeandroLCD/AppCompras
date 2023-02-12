using AppCompras.Controls;
using AppCompras.Services;
using AppCompras.Views;
using AppCompras.Views.Home;
using AppCompras.Views.Login;
using AppCompras.Views.Products;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AppCompras
{
    public partial class App : Application
    {
        #region Fields
        public static INavigation GlobalNavigation { get; set; }

        private NavigationPage _loginPage { get; set; }

        private Shell _homePage { get; set; }
        #endregion
        public App()
        {
            #region Services
            DependencyService.RegisterSingleton<IAuthentication>(new Authentication());
            DependencyService.RegisterSingleton<IApiFireBase>(new ApiFireBase());
            DependencyService.Register<IMessageService, MessageService>();
            
            #endregion

            InitializeComponent();

            if(AppSettings.Rememberme)
            {
                _homePage = new ShellPage();
                GlobalNavigation = _homePage.Navigation;
                // MainPage = _homePage; 
                MainPage = new AddProductPage();
            }
            else
            {
                _loginPage = new NavigationPage(new LoginPage());
                GlobalNavigation = _loginPage.Navigation;
                MainPage = _loginPage;
            }
            
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
