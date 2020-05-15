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
        List<Booking> bookings = new List<Booking>();
        public IActionResult Index()
        {
            return View(bookings);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View(new Booking());
        }

        [HttpPost]
        public IActionResult Post(Booking booking)
        {
            bookings.Add(booking);
            return View("Index", bookings);
        }
    }
}
