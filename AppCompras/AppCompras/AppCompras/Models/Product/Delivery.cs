using System;
using System.Collections.Generic;
using System.Text;

namespace AppCompras.Models.Product
{
    public class Delivery
    {
        public Delivery(Invoice invoice)
        {
            Invoice = invoice;
        }

        public Invoice Invoice { get; set; }

        public int Id { get; set; }

        public double Price { get; set; }

        public double PriceNeto { get; set; }

        public double PriceTotal { get; set; }

        public List<Tax> Taxs { get; set; }
    
    }
}