using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GalleryCafeWeb.Controllers
{
    public class OperationalStaffLoginController : Controller
    {
        // GET: OperationalStaffLogin
        public ActionResult OperationalStaffLogin()
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
                var getuname = entity.OSLogins.Where(a => a.OS_username == uname).SingleOrDefault();
                var getpsw = entity.OSLogins.Where(a => a.Password == password).SingleOrDefault();

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
                    Session["IsOStaff"] = true;
                    return RedirectToAction("OperationalStaffView", "OperationalStaffView");
                }
            }
        }

        public ActionResult Logout()
        {
            // Clear the session when logging out
            Session["IsOStaff"] = null;
            return RedirectToAction("index", "home");
        }
    }
}