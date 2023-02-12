using AppCompras.Models.Autentication;
using AppCompras.Models.Error;
using AppCompras.Services;
using Xamarin.Forms;

namespace AppCompras.ViewModels.Login
{
    public class SignPageViewModel : LoginPageViewModel
    {
        #region Fields
        private string _validateName;
        private string _validatePhone;
        private IAuthentication _authenticationService;
        private IMessageService _messajeService;
        private IApiFireBase _firebaseAPI;

        #endregion

        #region ctor
        public SignPageViewModel()
        {


            #region Service
            _authenticationService = DependencyService.Get<IAuthentication>();
            _messajeService = DependencyService.Get<IMessageService>();
            _firebaseAPI = DependencyService.Get<IApiFireBase>();
            #endregion

            InicializeProperties();
        }
        #endregion

        #region Properties

        public string ValidateName
        {
            get => _validateName;
            set => SetProperty(ref _validateName, value);
        }
        public string ValidatePhone
        {
            get => _validatePhone;
            set => SetProperty(ref _validatePhone, value);
        }


        #endregion

        #region Commands
        public Command SignInCommand { get; set; }

        public Command BackToCommand { get; set; }
        #endregion

        #region Methods
        private void InicializeProperties()
        {
            IsVisiblePassword = true;
            SignInCommand = new Command(SignIn);

            BackToCommand = new Command(async () =>
            {
                await App.GlobalNavigation.PopToRootAsync();
            });
        }

        private async void SignIn(object obj)
        {
            #region Validation
            if (!await _firebaseAPI.CheckConnection())
            {
                _messajeService.Toast("El dispositivo no cuenta con conexión a internet.");
                return;
            }

            if (string.IsNullOrEmpty(User.Name))
            {
                ValidateName = $"El nombre es requerido.";
                return;
            }
            else if (!User.IsValidPhone)
            {
                ValidateName = string.Empty;
                ValidatePhone = "El teléfono es requerido, en formato +56934071234.";
                return;
            }
            else if (!User.IsValidEmail)
            {
                ValidateName = ValidatePhone = string.Empty;
                ValidateEmail = $"Los Email no coinciden o tiene formato email.";
                return;

            }
            else if (!User.IsValidDomain)
            {
                var email = User.Email.Split('@');
                ValidateEmail = $"El dominio {email[1]}, no existe o no es alcanzable.";
                return;
            }
            else if (!User.IsValidPasswor)
            {
                ValidateName = ValidatePhone = ValidateEmail = string.Empty;
                ValidatePassword = $"Las contraseñas no coinciden.";
                return;
            }
            else
            {
                ValidateName = ValidatePhone = ValidateEmail = ValidatePassword = string.Empty;
            }
            #endregion

            #region Register
            //Register user Authentication
            var resp = await _authenticationService.CreateUser(User);
            if (resp.Success)
            {
                var auth = (ResponseAuthentication)resp.Object;
                var register = await _firebaseAPI.RegisterUser(User, auth);

                if (register.Success)
                {
                    _messajeService.Toast("Se registro correctamente su cuenta por favor inicie sesión.");
                    BackToCommand.Execute(null);
                }
                else
                {
                    await _messajeService.Show($"{register.Message}. Código: {register.Status} \nDetalles:{register.Object}");
                }
            }
            else
            {
                var error = (RootError)resp.Object;
                if (error != null && error.error.message.Contains("EMAIL_EXISTS"))
                {
                    await _messajeService.Show($"El Email ingresado ya se encuentra registrado, inicie sesión con su correo y contraseña.");
                    BackToCommand.Execute(null);
                    return;
                }
                await _messajeService.Show($"{resp.Message} Código:{resp.Status} \nDetalles: {error.error.message}");
            }


            #endregion


        }

        #endregion
    }
}
