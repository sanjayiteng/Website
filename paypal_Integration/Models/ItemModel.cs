using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PayPalIntegration.Models
{
    public class ItemModel
    {
        private string _sku;
        private string _name;
        private string _description;
        private string _quantity;
        private string _price;
        private string _currency;
        private string _tax;
        private string _url;
        private string _category;
        private string _totalPrice;


        public string sku
        {
            get { return _sku; }
            set { _sku = value; }
        }

        public string name
        {
            get { return _name; }
            set { _name = value; }
        }
        public string description {
            get { return _description; }
            set { _description = value; }
        }
        public string quantity {
            get { return _quantity; }
            set { _quantity = value; }
        }
        public string price {
            get { return _price; }
            set { _price = value; }
        }
        public string currency {
            get { return _currency; }
            set { _currency = value; }
        }
        public string tax {
            get { return _tax; }
            set { _tax = value; }
        }
        public string url {
            get { return _url; }
            set { _url = value; }
        }
        public string category {
            get { return _category; }
            set { _category = value; }
        }
        public string totalPrice {
            get { return _totalPrice;}
            set { _totalPrice = value; }
        }
        //public Measurement weight{
        //    get; set;
        //}
        //public Measurement length{
        //    get; set;
        //}
        //public Measurement height
        //{
        //    get; set;
        //}
        //public Measurement width
        //{
        //    get; set;
        //}
    }
}