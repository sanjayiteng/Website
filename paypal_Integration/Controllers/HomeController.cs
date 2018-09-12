using PayPalIntegration.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PayPalIntegration.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ItemModel item = new ItemModel();
            item.name = "Window Licence";
            item.price = "50";
            item.tax = "2";
            return View(item);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}