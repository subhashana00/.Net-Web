using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using GalleryCafeWeb.Models;
using Newtonsoft.Json;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Data.Entity.Infrastructure; // Added this line for DbUpdateException

public class OrderController : Controller
{
    private GalleryCafeODBEntities db = new GalleryCafeODBEntities();

    [HttpPost]
    public ActionResult PurchaseOrder(string orders)
    {
        if (Session["CustomerID"] == null)
        {
            return Json(new { success = false, message = "Customer not logged in." });
        }

        try
        {
            // Deserialize the orders
            var orderItems = JsonConvert.DeserializeObject<List<OrderItem>>(orders);
            decimal totalAmount = 0;

            // Create a new Order instance
            var order = new Order
            {
                CustomerID = (int)Session["CustomerID"],
                OrderDate = DateTime.Now,
                TotalAmount = 0, // Will calculate later
                Status = "Pending"
            };

            // Add the order to the database first to get the OrderID
            db.Orders.Add(order);
            db.SaveChanges(); // This saves the order and generates an OrderID

            // Now add each order detail
            foreach (var item in orderItems)
            {
                var orderDetail = new OrderDetail
                {
                    OrderID = order.OrderID, // Set the OrderID from the saved order
                    MenuItem = item.MenuItem,
                    Quantity = item.Quantity,
                    Price = item.Price
                };

                db.OrderDetails.Add(orderDetail);
                totalAmount += item.Price * item.Quantity; // Accumulate total amount
            }

            // Now save the order details to the database
            db.SaveChanges(); // This saves all order details

            // Update the total amount for the order
            order.TotalAmount = totalAmount;
            db.Entry(order).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges(); // Save the updated total amount

            return Json(new { success = true, message = "Order placed successfully!", orderId = order.OrderID });
        }
        catch (DbUpdateException dbEx)
        {
            return Json(new { success = false, message = "An error occurred while processing your order: " + dbEx.InnerException?.Message });
        }
        catch (JsonException jsonEx)
        {
            return Json(new { success = false, message = "An error occurred while processing your order: " + jsonEx.Message });
        }
        catch (Exception ex)
        {
            return Json(new { success = false, message = "An error occurred while processing your order: " + ex.Message });
        }
    }

    public ActionResult GeneratePDF(int orderId)
    {
        var order = db.Orders.Include("OrderDetails").FirstOrDefault(o => o.OrderID == orderId);
        if (order == null)
        {
            return HttpNotFound("Order not found.");
        }

        // Create the PDF document
        using (MemoryStream workStream = new MemoryStream())
        {
            var document = new Document(PageSize.A4, 25, 25, 30, 30);
            PdfWriter.GetInstance(document, workStream).CloseStream = false;
            document.Open();

            // Add content to PDF
            document.Add(new Paragraph("Order Confirmation"));
            document.Add(new Paragraph("Order ID: " + order.OrderID));
            document.Add(new Paragraph("Order Date: " + order.OrderDate));
            document.Add(new Paragraph("Total Amount: $" + order.TotalAmount));

            // Add order details
            document.Add(new Paragraph("\nOrder Details:"));
            foreach (var detail in order.OrderDetails)
            {
                document.Add(new Paragraph($"{detail.MenuItem} - Quantity: {detail.Quantity}, Price: ${detail.Price}"));
            }

            document.Close();

            // Return the PDF file as a downloadable response
            byte[] byteInfo = workStream.ToArray();
            return File(byteInfo, "application/pdf", "OrderConfirmation.pdf");
        }
    }
    public ActionResult ViewOrders()
    {
        // Fetch all orders and their details
        var orders = db.Orders.Include("OrderDetails").ToList();
        return View(orders);
    }
    public ActionResult EditOrder(int id)
    {
        var order = db.Orders.Include("OrderDetails").FirstOrDefault(o => o.OrderID == id);
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

    [HttpPost]
    public ActionResult DeleteOrder(int id)
    {
        var order = db.Orders.Include("OrderDetails").FirstOrDefault(o => o.OrderID == id);
        if (order == null)
        {
            return HttpNotFound();
        }

        // Delete associated order details
        db.OrderDetails.RemoveRange(order.OrderDetails);

        // Delete the order itself
        db.Orders.Remove(order);
        db.SaveChanges();

        return RedirectToAction("ViewOrders");
    }
}

// OrderItem class definition
public class OrderItem
{
    public string MenuItem { get; set; }
    public int Quantity { get; set; }
    public decimal Price { get; set; }
}