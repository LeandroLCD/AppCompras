using Firebase.Database;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Xamarin.Forms;

namespace AppCompras.Models.Product
{
    public class CategoryProduct
    {
        
        public string Id { get; set; }

        public string Name { get; set; }

        [JsonIgnore]
        public string Base64Image { get; set; }

        [JsonIgnore]
        public ImageSource Image => ImageSource.FromStream(() =>
        {
            return new MemoryStream(Convert.FromBase64String(Base64Image));
        });


        public static CategoryProduct operator +(CategoryProduct category, FirebaseObject<CategoryProduct> firebase)
        {
            
            category = firebase.Object;
            category.Id = firebase.Key;
            return category;
        }

    }
}
