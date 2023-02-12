using AppCompras.Models;
using AppCompras.Models.Autentication;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using AppCompras.Models.Error;
using AppCompras.Views.Login;

namespace AppCompras.Services
{
    public class Authentication : IAuthentication
    {
        private IApiFireBase _ApiFirebase;

        public Authentication()
        {
            _ApiFirebase = DependencyService.Get<IApiFireBase>();
        }

        public async Task<response> Login(UserAuthentication user)
        {

            try
            {

                HttpClient client = new HttpClient();
                string body = JsonConvert.SerializeObject(user);
                StringContent content = new StringContent(body, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PostAsync(AppSettings.ApiAuthentication(UriApi.Loging), content);
                if (response.StatusCode.Equals(HttpStatusCode.OK))
                {
                    string jsonResult = await response.Content.ReadAsStringAsync();
                    ResponseAuthentication oResponse = JsonConvert.DeserializeObject<ResponseAuthentication>(jsonResult);
                    oResponse.DateRegister = DateTime.Now;
                    oResponse.DateToken = DateTime.Now;
                    AppSettings.AuthenticationUser = oResponse;


                    return new response { Success = true, Message = "Login Exitoso", Object = oResponse, Status = 200 };
                }
                else
                {
                    string jsonResult = await response.Content.ReadAsStringAsync();
                    var oResponse = JsonConvert.DeserializeObject<RootError>(jsonResult);
                    string Message = string.Empty;
                    if (oResponse.error.message.Contains("INVALID_PASSWORD"))
                    {
                        Message = "Correo o Contraseña Inalida.";
                    }
                    else if (oResponse.error.message.Contains("EMAIL_NOT_FOUND"))
                    {
                        Message = "El correo es invalido.";
                    }
                    else
                    {
                        Message = oResponse.error.message;
                    }
                    return new response { Success = false, Message = Message, Object = oResponse, Status = 400 };
                }
            }
            catch (Exception ex)
            {
                return new response { Success = false, Message = ex.Message, Status = 401 };

            }

        }

        public async Task<response> PasswordReset(PasswordReset passwordReset)
        {

            try
            {

                HttpClient client = new HttpClient();
                string body = JsonConvert.SerializeObject(passwordReset);
                StringContent content = new StringContent(body, Encoding.UTF8, "application/json");
                string apiUri = string.Concat(AppSettings.ApiUrlGoogleApis, $"accounts:sendOobCode?key={AppSettings.KeyAplication}");

                HttpResponseMessage response = await client.PostAsync(apiUri, content);
                if (response.StatusCode.Equals(HttpStatusCode.OK))
                {
                    string jsonResult = await response.Content.ReadAsStringAsync();
                    object oResponse = JsonConvert.DeserializeObject<object>(jsonResult);


                    return new response { Success = true, Message = "Se ha enviado un correo para restablecer su contraseña, revise su bandeja o spam.", Object = oResponse, Status = 200 };
                }
                else
                {
                    string jsonResult = await response.Content.ReadAsStringAsync();
                    var oResponse = JsonConvert.DeserializeObject<RootError>(jsonResult);
                    string Message = string.Empty;
                    if (oResponse.error.message.Contains("EMAIL_NOT_FOUND"))
                    {
                        Message = "No existe registro de usuario correspondiente a este correo. Es posible que el usuario haya sido eliminado o este mal escrito el correo.";
                    }
                    else
                    {
                        Message = oResponse.error.message;
                    }
                    return new response { Success = false, Message = Message, Object = oResponse, Status = 400 };
                }
            }
            catch (Exception ex)
            {
                return new response { Success = false, Message = ex.Message, Status = ex.GetHashCode() };

            }

        }

        public void Logout()
        {

            AppSettings.DataUser = null;
            AppSettings.AuthenticationUser = null;
            AppSettings.Rememberme= false;
            var _loginPage = new NavigationPage(new LoginPage());
            App.GlobalNavigation = _loginPage.Navigation;
            App.Current.MainPage = _loginPage;
        }
        
        public async Task<bool> TokenRefresh()
        {
            ResponseAuthentication user = AppSettings.AuthenticationUser;
            try
            {
                if (DateTime.Now >= user.DateToken.AddSeconds(user.ExpiresIn))
                {
                    TokenRefres token = new TokenRefres { RefreshToken = user.RefreshToken };

                    HttpClient client = new HttpClient();

                    string body = JsonConvert.SerializeObject(token);
                    StringContent content = new StringContent(body, Encoding.UTF8, "application/json");
                    HttpResponseMessage response = await client.PostAsync(AppSettings.ApiAuthentication(UriApi.Token), content);
                    user.DateToken = DateTime.Now;
                    if (response.StatusCode.Equals(HttpStatusCode.OK))
                    {
                        string jsonResult = await response.Content.ReadAsStringAsync();
                        ResponseTokenRefesh oResponse = JsonConvert.DeserializeObject<ResponseTokenRefesh>(jsonResult);

                        user.IdToken = oResponse.IdToken;
                        AppSettings.AuthenticationUser = user;



                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {

                    return true;
                }
            }
            catch (Exception ex)
            {
                string t = ex.Message;
                return false;
            }






        }

        public async Task<response> CreateUser(User user)
        {
            try
            {
                UserAuthentication oUser = new UserAuthentication()
                {
                    email = user.Email,
                    password = user.Password
                };

                HttpClient client = new HttpClient();
                string body = JsonConvert.SerializeObject(oUser);
                StringContent content = new StringContent(body, Encoding.UTF8, "application/json");
                string ApiUrl = AppSettings.ApiAuthentication(UriApi.Sign);
                HttpResponseMessage response = await client.PostAsync(ApiUrl, content);
                if (response.StatusCode.Equals(HttpStatusCode.OK))
                {
                    string jsonResult = await response.Content.ReadAsStringAsync();
                    ResponseAuthentication oResponse = JsonConvert.DeserializeObject<ResponseAuthentication>(jsonResult);
                    
                    return new response
                    {
                        Success = true,
                        Object = oResponse,
                        Status = response.StatusCode.GetHashCode(),
                        Message = $"Se registro correctamente"
                    };
                }
                else
                {
                    string jsonResult = await response.Content.ReadAsStringAsync();
                    RootError error  = JsonConvert.DeserializeObject<RootError>(jsonResult);
                    return new response
                    {
                        Success = false,
                        Object = error,
                        Status = response.StatusCode.GetHashCode(),
                        Message = $"Se produjo un error al registrar al usuario."
                    };
                }
            }
            catch (Exception ex)
            {
                return new response
                {
                    Success = false,
                    Object = ex,
                    Status = ex.GetHashCode(),
                    Message = $"Se produjo una excepción al registrar al usuario. \n Detalles:{ex.Message}"
                };
            }

        }

    }
}
