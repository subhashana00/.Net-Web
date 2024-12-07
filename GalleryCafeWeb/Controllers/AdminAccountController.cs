using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace GalleryCafeWeb.Controllers
{
    public class AdminAccountController : Controller
    {
        public ActionResult AdminLogin()
        {
            return View();
        }

        [HttpPost]
        public ActionResult check()
        {
            var uname = Request["uname"];
            var password = Request["psw"];
            int unamecounter = 0;
            int pswcounter = 0;

            using (Models.GalleryCafeODBEntities entity = new Models.GalleryCafeODBEntities())
            {
                var getuname = entity.AdminLogins.Where(a => a.Admin_Username == uname).SingleOrDefault();
                var getpsw = entity.AdminLogins.Where(a => a.Password == password).SingleOrDefault();

                if (getuname == null)
                {
                    unamecounter = 0;
                }
                else
                {
                    unamecounter = 1;
                }

                if (getpsw == null)
                {
                    pswcounter = 0;
                }
                else
                {
                    pswcounter = 1;
                }

                if (unamecounter == 0 || pswcounter == 0)
                {
                    return RedirectToAction("index", "home");
                }
                else
                {
                    // Set session for admin user
                    Session["IsAdmin"] = true;
                    return RedirectToAction("adminview", "adminview");
                }
            }
        }

        public ActionResult Logout()
        {
            // Clear the session when logging out
            Session["IsAdmin"] = null;
            return RedirectToAction("index", "home");
        }
    }
}



     