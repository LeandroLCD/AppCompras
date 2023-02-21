using Firebase.Database;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
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

       

        public double Conten { get; set; }

        public List<CategoryProduct> Categories { get; set; }
        public string UdM { get; set; }

        public float Offer { get; set; }

        public bool IsOffer { get; set; }

        public double GrossPrice { get; set; } 

        public double Discounts => IsOffer? Math.Round(GrossPrice * (Offer /100),2) : 0;

        public double NetPrice => GrossPrice - Discounts;

        public double TaxTotal => Taxs != null && Taxs.Count > 0 ? CalculateTax(NetPrice) : 0;

        public double TotalPrice => NetPrice + TaxTotal;

        public List<Tax> Taxs { get; set; }

        [JsonIgnore]
        public List<ImageX> GetImageSource => GetImages(Base64Image);

        public static Products operator +(Products products, FirebaseObject<Products> firebase)
        {

            products = firebase.Object;
            products.Id = firebase.Key;
            products.Base64Image = products.Base64Image ?? new List<string>();
            return products;
        }

        private List<ImageX> GetImages(List<string> imagesBase64)
        {
            var list = new List<ImageX>();
            if(imagesBase64 != null) { 
            foreach (var image in imagesBase64)
            {
                if (!string.IsNullOrEmpty(image))
                    list.Add(new ImageX()
                    { 
                        Id = imagesBase64.FindIndex(p => p.Equals(image)),
                        Source= ImageSource.FromStream(() => new MemoryStream(Convert.FromBase64String(image)))
                    });
            }
            }
            return list;
        }
        private double CalculateTax(double netPrice)
        {
            double tax = 0;
            foreach (var item in Taxs)
            {
                tax += netPrice * (1 + item.Value);
            }

            return tax;
        }
    }
}
