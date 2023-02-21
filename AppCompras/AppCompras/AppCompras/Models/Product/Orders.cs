using System;
using System.Collections.Generic;
using System.Text;

namespace AppCompras.Models.Product
{
    public class Orders
    {
        public string ProductId { get; set; }

        public double Price { get; set; }

        public double Quality { get; set; }

        public double SubTotal {get; set; }

        public double Total { get; set; }

        public Products Products
        {
            get => default;
            set
            {
            }
        }

        public Orders()
        {

        }
        public Orders(Products products, double quality)
        {
            ProductId = products.Id;
            Price = products.TotalPrice;
            Quality= quality;
            SubTotal = Price * Quality;
            Total = Price * Quality;

        }
    }
}
