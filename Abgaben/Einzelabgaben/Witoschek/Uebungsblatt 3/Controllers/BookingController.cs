using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blatt03.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Blatt03.Controllers
{
    public class BookingController : Controller
    {
        // GET: /<controller>/
        public IActionResult Index()
        {
            Booking b1 = new Booking()
            {
                currentCharge = 20,
                requiredDistance = 50,
                start = new DateTime(2020, 5, 8, 13, 0, 0),
                end = new DateTime(2020, 5, 8, 18, 0, 0)
            };

            Booking b2 = new Booking()
            {
                currentCharge = 20,
                requiredDistance = 50,
                start = new DateTime(2020, 5, 10, 12, 0, 0),
                end = new DateTime(2020, 5, 8, 18, 0, 0)
            };

            Booking b3 = new Booking()
            {
                currentCharge = 20,
                requiredDistance = 50,
                start = new DateTime(2020, 5, 14, 19, 0, 0),
                end = new DateTime(2020, 5, 8, 18, 0, 0)
            };

            List<Booking> bookings = new List<Booking>()
            {
                b1,
                b2,
                b3
            };

            return View(bookings);
        }
    }
}
