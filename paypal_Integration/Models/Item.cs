using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PayPalIntegration.Models
{
    public class Item
    {
        public Product pr { get; set; }
        public int Quantity { get; set; }

        public Item()
        {
            pr = new Product();
        }
    }
}