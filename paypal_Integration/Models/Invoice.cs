using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PayPalIntegration.Models
{
    public class Invoice
    {

        public DateTime CreationDate { get; set; }

        public string CustomerName { get; set; }

        public string CustomerAddress { get; set; }
        public string Name { get; set; }
    }
}