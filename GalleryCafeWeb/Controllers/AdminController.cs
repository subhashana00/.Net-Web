using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;
using GalleryCafeWeb.Models; // Import the model namespace

namespace GalleryCafeWeb.Controllers
{
    public class AdminController : Controller
    {
        private GalleryCafeODBEntities db = new GalleryCafeODBEntities(); // Entity Framework context

        // Existing Admin Dashboard View
        public ActionResult AdminView()
        {
            return View();
        }


    }
}
