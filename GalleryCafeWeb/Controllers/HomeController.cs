using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GalleryCafeWeb.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "About page.";

            return View();
        }

        public ActionResult Menu()
        {
            ViewBag.Message = "Menu page.";

            return View();
        }

        public ActionResult Gallery()
        {
            ViewBag.Message = "Gallery page.";

            return View();
        }

        public ActionResult Reservation()
        {
            ViewBag.Message = "Reservation page.";

            return View();
        }



    }
}