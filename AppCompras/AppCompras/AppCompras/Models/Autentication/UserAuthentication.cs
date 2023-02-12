namespace AppCompras.Models.Autentication
{
    public class UserAuthentication
    {
        public string email { get; set; }
        public string password { get; set; }
        public bool returnSecureToken => true;
    }
}
