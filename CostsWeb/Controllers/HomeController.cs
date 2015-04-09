using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CostsWeb.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            if (User != null && User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Dashboard");                
            }
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Учет расходов";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}