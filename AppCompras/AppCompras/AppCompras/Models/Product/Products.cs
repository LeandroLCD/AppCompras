using Firebase.Database;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Xamarin.Forms;

namespace AppCompras.Models.Product
{
    public class Products
    {
        [JsonIgnore]
        public string Id { get; set; }

        public string Name { get; set; } //1

        public string Sku { get; set; }

        public int BarCode { get; set; }

        public string Description { get; set; }//2

        public List<string> Base64Image { get; set; } //0

        public double Price { get; set; } //3

        public double Conten { get; set; }

        public List<CategoryProduct> Categories { get; set; }
        public string UdM { get; set; }

        public double Offer { get; set; }

        public bool IsOffer { get; set; }

        public double PriceBruto => Price;

        public List<Tax> Taxs { get; set; }

        [JsonIgnore]
        public List<ImageX> GetImageSource => GetImages(Base64Image);

        public static Products operator +(Products products, FirebaseObject<Products> firebase)
        {

            products = firebase.Object;
            products.Id = firebase.Key;
            return products;
        }

        private List<ImageX> GetImages(List<string> imagesBase64)
        {
            var list = new List<ImageX>();

            foreach (var image in imagesBase64)
            {
                if (!string.IsNullOrEmpty(image))
                    list.Add(new ImageX()
                    { 
                        Id = imagesBase64.FindIndex(p => p.Equals(image)),
                        Source= ImageSource.FromStream(() => new MemoryStream(Convert.FromBase64String(image)))
                    });
            }
            return list;
        }
    }
}
