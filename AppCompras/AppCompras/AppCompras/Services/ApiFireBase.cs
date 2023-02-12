using AppCompras.Models;
using AppCompras.Models.Autentication;
using AppCompras.Models.Product;
using Firebase.Database;
using Firebase.Database.Query;
using Newtonsoft.Json;
using Plugin.Connectivity;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace AppCompras.Services
{
    public class ApiFireBase : IApiFireBase
    {
        private IAuthentication _authentication;
        private static FirebaseClient _ClientFireBase { get; set; }

        public ApiFireBase()
        {
            _authentication = DependencyService.Get<IAuthentication>();
            _ClientFireBase = new FirebaseClient(AppSettings.UrlFirebase,
                  new FirebaseOptions
                  {
                      AuthTokenAsyncFactory = () => Token()
                  });
        
        }
        private Task<string> Token()
        {
            return Task.FromResult(AppSettings.AuthenticationUser.IdToken);
        }
        public async Task<bool> CheckConnection()
        {
            if (!CrossConnectivity.Current.IsConnected)
            {
                return false;
            }

            bool isReachable = await CrossConnectivity.Current.IsReachable(AppSettings.UrlFirebase, 5000);
            if (isReachable)
            {
                return false;
            }
            var token = await _authentication.TokenRefresh();
            return true;
        }

        #region User
        public async Task<response> RegisterUser(User user, ResponseAuthentication oResponse)
        {
            try
            {
                HttpClient client = new HttpClient();
                string body = JsonConvert.SerializeObject(user);
                StringContent content = new StringContent(body, Encoding.UTF8, "application/json");
                string ApiUrl = string.Concat(AppSettings.UrlFirebase, $"Users/{oResponse.LocalId}.json?auth={oResponse.IdToken}");

                HttpResponseMessage response = await client.PutAsync(ApiUrl, content);
                if (response.StatusCode.Equals(HttpStatusCode.OK))
                {
                    string jsonstring = await response.Content.ReadAsStringAsync();

                    return new response
                    {
                        Success = true,
                        Object = user,
                        Message = "Usuario registrado exitosamente.",
                        Status = response.StatusCode.GetHashCode()
                    };
                }
                else
                {
                    string jsonstring = await response.Content.ReadAsStringAsync();
                    return new response
                    {
                        Success = false,
                        Object = jsonstring,
                        Message = "Usuario registrado exitosamente.",
                        Status = response.StatusCode.GetHashCode()
                    };
                }
            }
            catch (Exception ex)
            {
                return new response
                {
                    Success = true,
                    Object = ex,
                    Status = ex.GetHashCode(),
                    Message = $"Se propujo una excepción. \n Detalles: {ex.Message}"

                };
            }
        }

        
        public async Task<User> GetUser()
        {
            User user = null;
            try
            {
                
                HttpClient client = new HttpClient();
                string apiUri = string.Concat(AppSettings.UrlFirebase, $"Users/{AppSettings.AuthenticationUser.LocalId}.json?auth={AppSettings.AuthenticationUser.IdToken}");

                HttpResponseMessage response = await client.GetAsync(apiUri);
                if (response.StatusCode.Equals(HttpStatusCode.OK))
                {
                    string jsonstring = await response.Content.ReadAsStringAsync();

                    if (jsonstring != "null")
                    {
                        user = JsonConvert.DeserializeObject<User>(jsonstring);
                        user.LocalId = AppSettings.AuthenticationUser.LocalId;
                    }
                    return user;
                }
                else
                {
                    return user;
                }
            }
            catch (Exception)
            {
                return user;
            }
        }

       

        #endregion

        #region Categories
        public async Task<response> InsertCategory(CategoryProduct category)
        {
            try
            {

            
            var result = await _ClientFireBase.Child(nameof(CategoryProduct)).PostAsync(category);
            category += result;
            if(!string.IsNullOrEmpty(result.Key))
            {
                return new response
                {
                    Status = 200,
                    Success = true,
                    Object = category,
                    Message = "Se creo correctamente la categoría."
                };
            }
            else
            {
                return new response
                {
                    Status = 400,
                    Success = true,
                    Object = result,
                    Message = "Se produjo un error al intentar crear la categoría."
                };
            }
            }
            catch (Exception ex)
            {
                return new response
                {
                    Status = ex.GetHashCode(),
                    Success = false,
                    Object = ex,
                    Message = $"Se produjo una excención no controlada. \nDetalles{ex.Message}"
                };
            }
        }

        public Task<response> UpdateCategory(CategoryProduct category)
        {
            throw new NotImplementedException();
        }

        public async Task<response> DeleteCategory(CategoryProduct category)
        {
            try
            {
                await _ClientFireBase
                .Child(nameof(CategoryProduct))
                .Child(category.Id)
                .DeleteAsync();
                
                return new response
                {
                    Status = 200,
                    Success = true,
                    Object = category,
                    Message = "La categoria fue eliminada."
                };
            
            
            }
            catch (Exception ex)
            {
                return new response
                {
                    Status = ex.GetHashCode(),
                    Success = false,
                    Object = category,
                    Message = $"Se produjo una excepción no controlada. \nDetalles: {ex.Message}"
                };
            }
        }

        public async Task<response> GetCategories()
        {
            List<CategoryProduct> categories = new List<CategoryProduct>();
            CategoryProduct category = new CategoryProduct();

            try
            {
                if (await CheckConnection())
                {
                    var result = await _ClientFireBase.Child(nameof(CategoryProduct)).OnceAsync<CategoryProduct>();

                    foreach (var item in result)
                    {
                        category += item;
                        categories.Add(category);
                    }


                    if (categories.Count > 0)
                    {
                        return new response
                        {
                            Status = 200,
                            Success = true,
                            Object = categories,
                            Message = "Datos Obtenidos"
                        };

                    }
                    else
                    {
                        return new response
                        {
                            Status = 400,
                            Success = false,
                            Object = result,
                            Message = "No se logro optener los datos."
                        };
                    }
                }

                else
                {
                    return new response
                    {
                        Status = 401,
                        Success = false,
                        Object = false,
                        Message = "El dispositivo no cuenta con conexión a internet."
                    };
                }

            }
            catch (Exception ex)
            {
                return new response
                {
                    Status = ex.GetHashCode(),
                    Success = false,
                    Object = ex,
                    Message = $"Se produjo una excención no controlada. \nDetalles{ex.Message}"
                };
            }


        }

        #endregion

        #region Products
        public async Task<response> GetProducts()
        {
            List<Products> products = new List<Products>();
            Products product = new Products();

            try
            {
                if (await CheckConnection())
                {
                    var result = await _ClientFireBase.Child(nameof(Products)).OnceAsync<Products>();

                    foreach (var item in result)
                    {
                        product += item;
                        products.Add(product);
                    }


                    if (products.Count > 0)
                    {
                        return new response
                        {
                            Status = 200,
                            Success = true,
                            Object = products,
                            Message = "Datos Obtenidos"
                        };

                    }
                    else
                    {
                        return new response
                        {
                            Status = 400,
                            Success = false,
                            Object = result,
                            Message = "No se logro optener los datos."
                        };
                    }
                }

                else
                {
                    return new response
                    {
                        Status = 401,
                        Success = false,
                        Object = false,
                        Message = "El dispositivo no cuenta con conexión a internet."
                    };
                }

            }
            catch (Exception ex)
            {
                return new response
                {
                    Status = ex.GetHashCode(),
                    Success = false,
                    Object = ex,
                    Message = $"Se produjo una excención no controlada. \nDetalles{ex.Message}"
                };
            }
        }

        public async Task<response> UpdateProduct(Products model)
        {
            try
            {


                await _ClientFireBase
                    .Child(nameof(Products))
                    .Child(model.Id)
                    .PutAsync<Products>(model);

                return new response
                {
                    Status = 200,
                    Success = true,
                    Object = model,
                    Message = "Se modifico el producto."
                };
            }
            catch (Exception ex)
            {
                return new response
                {
                    Status = ex.GetHashCode(),
                    Success = false,
                    Object = ex,
                    Message = $"Se produjo una excención no controlada. \nDetalles{ex.Message}"
                };
            }
        }

        public async Task<response> DeleteProduct(Products model)
        {
            try
            {
                await _ClientFireBase
                .Child(nameof(Products))
                .Child(model.Id)
                .DeleteAsync();

                return new response
                {
                    Status = 200,
                    Success = true,
                    Object = model,
                    Message = "El producto fue eliminado."
                };


            }
            catch (Exception ex)
            {
                return new response
                {
                    Status = ex.GetHashCode(),
                    Success = false,
                    Object = model,
                    Message = $"Se produjo una excepción no controlada. \nDetalles: {ex.Message}"
                };
            }
        }

        public async Task<response> InsertProduct(Products model)
        {
            try
            {


                var result = await _ClientFireBase.Child(nameof(Products)).PostAsync(model);
                model += result;
                if (!string.IsNullOrEmpty(result.Key))
                {
                    return new response
                    {
                        Status = 200,
                        Success = true,
                        Object = model,
                        Message = "Se creo correctamente el producto."
                    };
                }
                else
                {
                    return new response
                    {
                        Status = 400,
                        Success = true,
                        Object = result,
                        Message = "Se produjo un error al intentar crear el producto."
                    };
                }
            }
            catch (Exception ex)
            {
                return new response
                {
                    Status = ex.GetHashCode(),
                    Success = false,
                    Object = ex,
                    Message = $"Se produjo una excención no controlada. \nDetalles{ex.Message}"
                };
            }
        }
        #endregion

    }

    
    
}
