using AppCompras.Font;
using AppCompras.Models.Autentication;
using AppCompras.Services;
using AppCompras.Views.Home;
using AppCompras.Views.Login;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace AppCompras.ViewModels.Login
{
    public class LoginPageViewModel : BaseViewModel
    {
        #region Fields
        private bool _isVisiblePassword;
        private string _iconPassword;
        private User _user;
        private bool _isVisibleModal;
        private IAuthentication _authenticationService;
        private IMessageService _messajeService;
        private IApiFireBase _firebaseAPI;
        private string _validateEmail;
        private string _validatePassword;
        private bool _rememberme;

        #endregion

        #region Ctor
        public LoginPageViewModel()
        {
            #region Service
            _authenticationService = DependencyService.Get<IAuthentication>();
            _messajeService = DependencyService.Get<IMessageService>();
            _firebaseAPI = DependencyService.Get<IApiFireBase>();
            #endregion
            IsVisiblePassword = true;
            InicializeProperties();
        }
        #endregion

        #region Properties
        public User User
        {
            get
            {
                if (_user == null)
                    _user = new User();
                return _user;
            }
            set => SetProperty(ref _user, value);
        }
        public bool IsVisiblePassword
        {
            get => _isVisiblePassword;
            set => SetProperty(ref _isVisiblePassword, value);
        }
        public bool IsVisibleModal
        {
            get => _isVisibleModal;
            set => SetProperty(ref _isVisibleModal, value);
        }
        public string IconPassword
        {
            get => _iconPassword;
            set => SetProperty(ref _iconPassword, value);
        }
        public string ValidateEmail
        {
            get => _validateEmail;
            set => SetProperty(ref _validateEmail, value);
        }
        public string ValidatePassword
        {
            get => _validatePassword;
            set => SetProperty(ref _validatePassword, value);
        }
        #endregion

        #region Command
        public Command IsPasswordCommand { get; set; }

        public Command ToGoSignInCommand { get; set; }

        public Command IsRecoveryCommand { get; set; }

        public Command LoginCommand { get; set; }

        public Command<bool> RemembermeCommand { get; set; }

        #endregion

        #region Methods
        private void InicializeProperties()
        {
            IconPassword = IconFont.Eye;

            IsPasswordCommand = new Command(() =>
            {
                IsVisiblePassword = !IsVisiblePassword;
                if (IsVisiblePassword)
                {
                    IconPassword = IconFont.Eye;
                }
                else
                {
                    IconPassword = IconFont.EyeOff;
                }
            });

            IsRecoveryCommand = new Command(() =>
            {
                IsVisibleModal = !IsVisibleModal;

            });

            ToGoSignInCommand = new Command(async () =>
            {
                await App.GlobalNavigation.PushAsync(new SignInPage());
            });

            LoginCommand = new Command(LoginAppAsync);

            RemembermeCommand = new Command<bool>((obj) =>
            {
                _rememberme = obj;
            });
        }

        private async void LoginAppAsync(object obj)
        {
            #region Validations
            if (!await _firebaseAPI.CheckConnection())
            {
                _messajeService.Toast("El dispositivo no cuenta con conexión a internet.");
                return;
            }
            else if (!User.IsEmail(User.Email))
            {
                ValidateEmail = $"El Email es requerido y debe tiener formato email.";
                return;

            }
            else if (string.IsNullOrEmpty(User.Password))
            {
                ValidateEmail = string.Empty;
                ValidatePassword = $"La contraseña es requerida.";
                return;
            }
            else
            {
                ValidateEmail = ValidatePassword = string.Empty;
            }

            #endregion

            #region Login
            var resp = await _authenticationService.Login(new UserAuthentication
            {
                email = User.Email,
                password = User.Password
            });
            if (resp.Success)
            {
                AppSettings.AuthenticationUser = (ResponseAuthentication)resp.Object;

                AppSettings.DataUser = await _firebaseAPI.GetUser();
                AppSettings.Rememberme = _rememberme;
                _messajeService.Toast($"Bienvenido {AppSettings.DataUser.Name}.");
                var home = new NavigationPage(new HomePage());
                App.GlobalNavigation = home.Navigation;
                Application.Current.MainPage = home;
            }
            else
            {
                await _messajeService.Show($"{resp.Message}. Código:{resp.Status}");
            }
            #endregion
        }
        #endregion
    }
}
