using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity.Infrastructure; 
using System.Web.Mvc;
using System.Data.Entity; // Add this namespace for EntityState
using GalleryCafeWeb.Models;



namespace GalleryCafeWeb.Controllers
{
    public class AdminViewController : Controller
    {
        private GalleryCafeODBEntities db = new GalleryCafeODBEntities();

        // GET: AdminView
        public ActionResult AdminView()
        {
            return View();
        }

        // GET: ManageUsers (List of Customers)
        public ActionResult ManageUsers()
        {
            var customers = db.Customers.ToList();
            return View(customers);
        }

        // GET: Edit Customer (Load the customer to edit)
        public ActionResult EditCustomer(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }

            Customer customer = db.Customers.Find(id);
            if (customer == null)
            {
                return HttpNotFound();
            }

            return View(customer);
        }

        // POST: Edit Customer
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditCustomer(Customer customer)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // Attach customer to the context
                    db.Entry(customer).State = EntityState.Modified;

                    // Ensure concurrency handling
                    db.SaveChanges();
                    return RedirectToAction("ManageUsers");
                }
                catch (DbUpdateConcurrencyException)
                {
                    var entry = db.Entry(customer);
                    if (entry == null)
                    {
                        ModelState.AddModelError(string.Empty, "The record you attempted to edit was deleted by another user.");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "The record you attempted to edit was modified by another user after you got the original value. The edit operation was canceled, and the current values in the database have been displayed. If you still want to edit this record, click Save again.");
                        // Optionally, reload the data from the database
                        customer.RowVersion = (byte[])entry.OriginalValues["RowVersion"];
                    }
                }
            }
            return View(customer);
        }

        // GET: Delete Customer
        public ActionResult DeleteCustomer(int id)
        {
            var customer = db.Customers.Find(id);
            if (customer == null)
            {
                return HttpNotFound();
            }
            return View(customer);
        }

        // POST: Delete Customer
        [HttpPost, ActionName("DeleteCustomer")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var customer = db.Customers.Find(id);
            if (customer != null)
            {
                db.Customers.Remove(customer);
                db.SaveChanges();
                TempData["SuccessMessage"] = "Customer deleted successfully.";
            }
            else
            {
                ModelState.AddModelError("", "Error deleting customer: Customer not found.");
            }
            return RedirectToAction("ManageUsers");
        }

        // GET: Create Customer
        public ActionResult CreateCustomer()
        {
            return View();
        }

        // POST: Create Customer
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateCustomer(Customer customer)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    db.Customers.Add(customer);
                    db.SaveChanges();
                    TempData["SuccessMessage"] = "Customer created successfully.";
                    return RedirectToAction("ManageUsers");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Error creating customer: " + ex.Message);
                }
            }

            return View(customer);
        }
    }
}
