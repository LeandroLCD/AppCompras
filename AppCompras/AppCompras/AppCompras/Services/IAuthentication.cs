using AppCompras.Models;
using AppCompras.Models.Autentication;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AppCompras.Services
{
    public interface IAuthentication
    {
        Task<response> Login(UserAuthentication user);

        Task<response> PasswordReset(PasswordReset passwordReset);

        void Logout();

        Task<response> CreateUser(User user);

        Task<bool> TokenRefresh();


    }
}
