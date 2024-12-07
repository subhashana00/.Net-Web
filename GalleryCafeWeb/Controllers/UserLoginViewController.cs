using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GalleryCafeWeb.Models;


namespace GalleryCafeWeb.Controllers
{
    public class UserLoginViewController : Controller
    {
        private GalleryCafeODBEntities db = new GalleryCafeODBEntities();

        // GET: UserLoginView
        public ActionResult UserLoginPage()
        {
            return View();
        }
        [HttpPost]
        public ActionResult ReserveTable(Reservation reservation)
        {
            // Get the logged-in user's CustomerID
            int customerId = (int)Session["CustomerID"]; // Ensure that CustomerID is set in session after login

            if (ModelState.IsValid)
            {
                reservation.CustomerID = customerId; // Set CustomerID
                db.Reservations.Add(reservation); // Add the reservation
                db.SaveChanges(); // Save changes to the database

                TempData["SuccessMessage"] = "Reservation successfully made!";
                return RedirectToAction("UserLoginPage"); // Redirect back to the user page
            }

            TempData["ErrorMessage"] = "Failed to make the reservation. Please check your input.";
            return RedirectToAction("UserLoginPage");
        }
    }


}
