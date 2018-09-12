using PayPalIntegration.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PayPalIntegration.Controllers
{
    public class ProductController : Controller
    {
        // GET: Product
        public ActionResult Index()
        {

            List<Product> lstProject = new List<Product>();
            lstProject.Add(new Product() { Price = 1000, ProductCategory = 1, ProductId = 1, ProductName = "IPhone", Title = "IPhone" });
            lstProject.Add(new Product() { Price = 2000, ProductCategory = 1, ProductId = 2, ProductName = "Samsung", Title = "Samsung" });
            lstProject.Add(new Product() { Price = 3000, ProductCategory = 1, ProductId = 3, ProductName = "Nokia", Title = "Nokia" });
            lstProject.Add(new Product() { Price = 4000, ProductCategory = 1, ProductId = 4, ProductName = "Motorola", Title = "Motorola" }); 
                                                                           
            lstProject.Add(new Product() { Price = 5000, ProductCategory = 1, ProductId = 5, ProductName = "Lenovo", Title = "Lenovo" });
            Session["Project"]= lstProject;
            return View(lstProject);
        }
    }
}