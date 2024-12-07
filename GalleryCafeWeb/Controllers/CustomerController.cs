using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Security.Cryptography;
using System.Text;
using GalleryCafeWeb.Models;


namespace GalleryCafeWeb.Controllers
{
    public class CustomerController : Controller
    {
        private GalleryCafeODBEntities db = new GalleryCafeODBEntities();

        // GET: Customer/SignUp
        public ActionResult SignUp()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SignUp(Customer customer)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // Directly saving the password entered by the user (no hashing)
                    db.Customers.Add(customer);
                    db.SaveChanges();  // Save the customer data to the database

                    // Store the success message in TempData
                    TempData["SuccessMessage"] = "SignUp successfully!";

                    return RedirectToAction("Login");
                }
                catch (System.Data.Entity.Validation.DbEntityValidationException dbEx)
                {
                    // Capture and display validation errors
                    foreach (var validationErrors in dbEx.EntityValidationErrors)
                    {
                        foreach (var validationError in validationErrors.ValidationErrors)
                        {
                            ModelState.AddModelError("", $"Property: {validationError.PropertyName} Error: {validationError.ErrorMessage}");
                        }
                    }
                }
                catch (Exception ex)
                {
                    // Capture other errors (like database constraint violations)
                    ModelState.AddModelError("", ex.Message);
                }
            }

            return View(customer);
        }

        // GET: Customer/Login
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(string usernameOrEmail, string password)
        {
            if (ModelState.IsValid)
            {
                // Find the customer by username or email
                var customer = db.Customers.FirstOrDefault(c =>
                    c.CusUsername == usernameOrEmail ||
                    c.CusEmail == usernameOrEmail);

                if (customer != null && customer.CusPasswordHash == password) // Direct comparison without hashing
                {
                    // Optionally, you can save customer info in session or cookie
                    Session["CustomerID"] = customer.CustomerID; // Store in session
                    Session["CustomerUsername"] = customer.CusUsername;

                    // Show success alert
                    TempData["LoginMessage"] = $"Hi, {customer.CusUsername}! You're logged in.";

                    // Redirect to UserLoginPage view upon successful login
                    return RedirectToAction("UserLoginPage", "UserLoginView"); // Change 'YourControllerName' to the actual controller name
                }
                else
                {
                    ModelState.AddModelError("", "Invalid username/email or password.");
                }
            }

            return View();
        }

        public ActionResult AccountDetails()
        {
            if (Session["CustomerID"] == null)
            {
                return RedirectToAction("Login", "Customer"); // Redirect to login if not authenticated
            }

            int customerId = (int)Session["CustomerID"];
            var customer = db.Customers.Find(customerId);

            if (customer == null)
            {
                return HttpNotFound();
            }

            return View(customer);
        }



        public ActionResult SignOut()
        {
            // Clear session variables
            Session.Clear();
            Session.Abandon();

            // Redirect to the previous view or home page
            return RedirectToAction("Index", "Home");
        }
    }
}