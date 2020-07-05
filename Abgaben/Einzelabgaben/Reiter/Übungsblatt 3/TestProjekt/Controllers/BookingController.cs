using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TestProjekt.Models;

namespace TestProjekt.Controllers
{
    public class BookingController : Controller
    {
        public IActionResult Index()
        {
            Booking booking1 = new Booking()
            {
                currentCharge = 15,
                requiredDistance = 100,
                start = new DateTime(2020, 5, 12, 10, 0, 0),
                end = new DateTime(2020, 5, 12, 13, 0, 0)
            };

            Booking booking2 = new Booking()
            {
                currentCharge = 20,
                requiredDistance = 60,
                start = new DateTime(2020, 5, 12, 12, 0, 0),
                end = new DateTime(2020, 5, 12, 17, 0, 0)
            };

            Booking booking3 = new Booking()
            {
                currentCharge = 65,
                requiredDistance = 96,
                start = new DateTime(2020, 5, 12, 11, 0, 0),
                end = new DateTime(2020, 5, 12, 20, 0, 0)
            };

            List<Booking> bookings = new List<Booking>()
            {
                booking1,
                booking2,
                booking3
            };

            return View(bookings);
        }
    }
}