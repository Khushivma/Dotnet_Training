﻿using System.Linq;
using System.Web.Mvc;
using Assessment001.Models;

namespace Assessment001.Controllers
{
    public class CodeController : Controller
    {
        private NorthwindEntities db = new NorthwindEntities(); // Adjust the context name as per your project

        // Action method to return all customers residing in Germany
        public ActionResult GermanCustomers()
        {
            var germanCustomers = db.Customers.Where(c => c.Country == "Germany").ToList();
            return View(germanCustomers);
        }

        // Action method to return customer details with orderId == 10248
        public ActionResult CustomerDetailsWithOrderId()
        {
            var customerWithOrder10248 = db.Customers.FirstOrDefault(c => c.Orders.Any(o => o.OrderID == 10248));
            return View(customerWithOrder10248);
        }
    }
}
