using AppCompras.Models;
using AppCompras.Models.Autentication;
using AppCompras.Models.Product;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AppCompras.Services
{
    public interface IApiFireBase
    {
        Task<response> RegisterUser(User user, ResponseAuthentication oResponse);
        Task<User> GetUser();
        Task<bool> CheckConnection();
        #region Categories
        Task<response> InsertCategory(CategoryProduct category);

        Task<response> UpdateCategory(CategoryProduct category);

        Task<response> DeleteCategory(CategoryProduct category);

        Task<response> GetCategories();
        #endregion

        #region Products
        Task<response> GetProducts();

        Task<response> UpdateProduct(Products model);

        Task<response> DeleteProduct(Products model);

        Task<response> InsertProduct(Products model);
        #endregion
    }
}
