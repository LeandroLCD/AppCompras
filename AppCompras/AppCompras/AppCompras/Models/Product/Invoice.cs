using System;
using System.Collections.Generic;
using System.Text;

namespace AppCompras.Models.Product
{
    public class Invoice : Orders
    {
        public string Id { get; set; }

        public string ClientId { get; set; }

        public DateTime Date { get; set; }

        public List<Orders> Detalles { get; set; }

        public Delivery Delivery
        {
            get => default;
            set
            {
            }
        }

        public Invoice()
        {

        }
        public Invoice(List<Orders> prod, string clientId)
        {
            Detalles = prod;

            Date = DateTime.Now;

            ClientId = clientId;
        }

    }
}
