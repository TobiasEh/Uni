using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Blatt3_Aufgabe4.Models;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Blatt3_Aufgabe4.Controllers
{
    public class BookingController : Controller
    {
        // GET: /<controller>/
        public IActionResult Index()
        {
            Booking[] bookings = new Booking[]{ new Booking
        {
            chargeStatus = 10,
            distance = 230,
            startTime = new DateTime(2020, 5, 12, 20, 22, 0),
            endTime = new DateTime(2020, 5, 12, 22, 0, 0),
        }, new Booking
        {
            chargeStatus = 50,
            distance = 40,
            startTime = new DateTime(2020, 5, 24, 08, 15, 33),
            endTime = new DateTime(2020, 5, 30, 10, 40, 12),
        } };
            return View(bookings);
        }
    }
}
