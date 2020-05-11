using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Aufgabe_4.Models;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Aufgabe_4.Controllers
{
    public class BookingController : Controller
    {
        // GET: /<controller>/
        public IActionResult Index()
        {
            Booking b1 = new Booking()
            {
                currentCharge = 10,
                requiredDistance = 30,
                start = new DateTime(2020, 5, 7, 12, 0, 0),
                end = new DateTime(2020, 5, 7, 17, 0, 0)
            };

            Booking b2 = new Booking()
            {
                currentCharge = 10,
                requiredDistance = 30,
                start = new DateTime(2020, 5, 10, 12, 30, 0),
                end = new DateTime(2020, 5, 10, 18, 0, 0)
            };

            Booking b3 = new Booking()
            {
                currentCharge = 10,
                requiredDistance = 30,
                start = new DateTime(2020, 5, 7, 22, 0, 0),
                end = new DateTime(2020, 5, 8, 8, 15, 0)
            };
            Booking b4 = new Booking()
            {
                currentCharge = 10,
                requiredDistance = 30,
                start = new DateTime(2020, 5, 8, 22, 0, 0),
                end = new DateTime(2020, 5, 9, 8, 15, 0)
            };

            List<Booking> bookingList = new List<Booking>()
            {
                b1,
                b2,
                b3,
                b4
            };
            return View(bookingList);
        }
    }
}
