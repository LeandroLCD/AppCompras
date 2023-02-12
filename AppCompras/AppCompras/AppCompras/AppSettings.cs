using AppCompras.Models;
using AppCompras.Models.Autentication;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Essentials;

namespace AppCompras
{
    public static class AppSettings
    {
        #region Fields
        private static ResponseAuthentication _AuthenticationUser;
        private static User value;
        private static bool _rememberme;
        #endregion

        #region Properties

        public static readonly string UrlFirebase = "https://appcompras-5fcd7-default-rtdb.firebaseio.com/";

        public static readonly string KeyAplication = "AIzaSyAYZ8w_lxy1DNsctHhxHNHiJjsl_XeYZFE";

        public static readonly string ApiUrlGoogleApis = "https://identitytoolkit.googleapis.com/v1/";

        public static bool Rememberme
        {
            get
            {
                if(Preferences.ContainsKey("Rememberme"))
                {
                    var p = Preferences.Get("Rememberme", false);
                    _rememberme = p;
                }
                return _rememberme;
            }
            set
            {
                Preferences.Set("Rememberme", value);
                _rememberme = value;
            }
        }
        public static ResponseAuthentication AuthenticationUser
        {
            get
            {

                if (Preferences.ContainsKey("UserAutentication"))
                {
                    var user = Preferences.Get("UserAutentication", string.Empty);
                    _AuthenticationUser = JsonConvert.DeserializeObject<ResponseAuthentication>(user);
                }

                return _AuthenticationUser;
            }
            set
            {
                _AuthenticationUser = value;
                Preferences.Set("UserAutentication", JsonConvert.SerializeObject(value));
            }
        }

        public static User DataUser
        {
            get
            {

                if (Preferences.ContainsKey("DataUser"))
                {
                    var user = Preferences.Get("DataUser", string.Empty);
                    value = JsonConvert.DeserializeObject<User>(user);
                }

                return value;
            }
            set
            {
                AppSettings.value = value;

                Preferences.Set("DataUser", JsonConvert.SerializeObject(value));
            }
        }

        #endregion

        #region Methods
        public static string ApiAuthentication(UriApi uriApi)
        {
            switch (uriApi)
            {
                case UriApi.Loging:
                    return ApiUrlGoogleApis + "accounts:signInWithPassword?key=" + KeyAplication;

                case UriApi.Sign:
                    return ApiUrlGoogleApis + "accounts:signUp?key=" + KeyAplication;
                case UriApi.Token:
                    return ApiUrlGoogleApis + "token?key=" + KeyAplication;
                default:
                    return String.Empty;

            }

        }
        #endregion
    }
}
