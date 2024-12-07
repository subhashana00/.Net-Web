using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using iTextSharp.text;
using iTextSharp.text.pdf;
using GalleryCafeWeb.Models;
using System.IO;

namespace GalleryCafeWeb.Controllers
{
    public class ReservationController : Controller
    {
        private GalleryCafeODBEntities db = new GalleryCafeODBEntities();

        // GET: Reservation
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(Reservation reservation)
        {
            if (ModelState.IsValid)
            {
                int customerId = (int)Session["CustomerID"]; // Assuming CustomerID is stored in session
                reservation.CustomerID = customerId; // Set CustomerID
                db.Reservations.Add(reservation);
                db.SaveChanges();

                TempData["SuccessMessage"] = "Reservation successfully made!";
                return RedirectToAction("Details", new { id = reservation.ReservationID }); // Redirect to Details
            }

            TempData["ErrorMessage"] = "Failed to make the reservation. Please check your input.";
            return View(reservation);
        }

        // New action method for viewing reservation details
        public ActionResult Details(int id)
        {
            var reservation = db.Reservations.Find(id);
            if (reservation == null)
            {
                return HttpNotFound();
            }
            return View(reservation);
        }

        public ActionResult DownloadPDF(int id)
        {
            var reservation = db.Reservations.Find(id);
            if (reservation == null)
            {
                return HttpNotFound();
            }

            using (var memoryStream = new MemoryStream())
            {
                Document pdfDoc = new Document(PageSize.A4);
                PdfWriter.GetInstance(pdfDoc, memoryStream);
                pdfDoc.Open();
                pdfDoc.Add(new Paragraph("Reservation Details"));
                pdfDoc.Add(new Paragraph($"Full Name: {reservation.ResName}"));
                pdfDoc.Add(new Paragraph($"Email: {reservation.ResEmail}"));
                pdfDoc.Add(new Paragraph($"Phone Number: {reservation.ResPhoneNumber}"));

                // Directly use the string values for ResDate and ResTime
                string reservationDate = string.IsNullOrEmpty(reservation.ResDate) ? "N/A" : reservation.ResDate;
                string reservationTime = string.IsNullOrEmpty(reservation.ResTime) ? "N/A" : reservation.ResTime;

                pdfDoc.Add(new Paragraph($"Reservation Date: {reservationDate}"));
                pdfDoc.Add(new Paragraph($"Reservation Time: {reservationTime}"));
                pdfDoc.Add(new Paragraph($"Table Type: {reservation.ResMessage}"));
                pdfDoc.Close();

                return File(memoryStream.ToArray(), "application/pdf", "ReservationDetails.pdf");
            }
        }
    }
}