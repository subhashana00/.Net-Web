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
    public class OperationalStaffController : Controller
    {
        private GalleryCafeODBEntities db = new GalleryCafeODBEntities(); // Entity Framework context

        // GET: OperationalStaff
        public ActionResult OperationalStaffView()
        {
            return View();
        }

        // GET: ManageReservations
        public ActionResult ManageReservations()
        {
            var reservations = db.Reservations.ToList(); // Fetch all reservations from the database
            return View(reservations); // Pass the list of reservations to the view
        }

        // GET: EditReservation
        [HttpGet]
        public ActionResult EditReservation(int id)
        {
            var reservation = db.Reservations.Find(id); // Find reservation by ID
            if (reservation == null)
            {
                return HttpNotFound(); // Return 404 if not found
            }
            return View(reservation);
        }

        // POST: EditReservation
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditReservation(Reservation model)
        {
            if (ModelState.IsValid)
            {
                var existingReservation = db.Reservations.Find(model.ReservationID);
                if (existingReservation != null)
                {
                    existingReservation.ResName = model.ResName;
                    existingReservation.ResEmail = model.ResEmail;
                    existingReservation.ResPhoneNumber = model.ResPhoneNumber;
                    existingReservation.ResDate = model.ResDate;
                    existingReservation.ResTime = model.ResTime;
                    existingReservation.ResMessage = model.ResMessage;

                    db.Entry(existingReservation).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();

                    return RedirectToAction("ManageReservations");
                }
                else
                {
                    ModelState.AddModelError("", "Reservation not found.");
                }
            }
            return View(model);
        }

        // Delete Reservation
        public ActionResult DeleteReservation(int id)
        {
            var reservation = db.Reservations.Find(id); // Find reservation by ID
            db.Reservations.Remove(reservation); // Delete reservation
            db.SaveChanges();
            return RedirectToAction("ManageReservations");
        }

        // Generate PDF for Reservation
        public ActionResult DownloadReservationPDF(int id)
        {
            var reservation = db.Reservations.Find(id);
            if (reservation == null)
            {
                return HttpNotFound();
            }

            MemoryStream workStream = new MemoryStream();
            Document doc = new Document();
            PdfWriter.GetInstance(doc, workStream).CloseStream = false;
            doc.Open();

            // Add content to PDF
            doc.Add(new Paragraph("Reservation Details"));
            doc.Add(new Paragraph("Name: " + reservation.ResName));
            doc.Add(new Paragraph("Email: " + reservation.ResEmail));
            doc.Add(new Paragraph("Phone: " + reservation.ResPhoneNumber));
            doc.Add(new Paragraph("Date: " + reservation.ResDate));
            doc.Add(new Paragraph("Time: " + reservation.ResTime));
            doc.Add(new Paragraph("Message: " + reservation.ResMessage));

            doc.Close();

            byte[] byteArray = workStream.ToArray();
            workStream.Write(byteArray, 0, byteArray.Length);
            workStream.Position = 0;

            return File(workStream, "application/pdf", "ReservationDetails.pdf");
        }

        // Action to display the order list
        public ActionResult ViewOrders()
        {
            // Fetch all orders from the database
            var orders = db.Orders.Include("Customer").Include("OrderDetails").ToList();
            return View(orders);
        }

        // Action to Edit an Order
        public ActionResult EditOrder(int id)
        {
            var order = db.Orders.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }

        [HttpPost]
        public ActionResult EditOrder(Order order)
        {
            if (ModelState.IsValid)
            {
                db.Entry(order).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("ViewOrders");
            }
            return View(order);
        }

        // Action to Delete an Order
        public ActionResult DeleteOrder(int id)
        {
            var order = db.Orders.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }

        [HttpPost, ActionName("DeleteOrder")]
        public ActionResult DeleteConfirmed(int id)
        {
            var order = db.Orders.Find(id);
            db.Orders.Remove(order);
            db.SaveChanges();
            return RedirectToAction("ViewOrders");
        }
    }
}
