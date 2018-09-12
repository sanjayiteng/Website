using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PayPalIntegration.Controllers
{
    public class AccountController : Controller
    {
        // GET: Admin
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(string UserName,string Password)
        {

            if (UserName.Trim().ToLower() == "admin" && Password == "admin")
            {
                return RedirectToAction("Index", "Paypal");
            }
            else
            {
                return View();
            }
        }
    }
}