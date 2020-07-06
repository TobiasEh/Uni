using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace sopro2020_abgabe.Controllers
{
    public class BookingController : Controller
    {
        public IActionResult CreateBooking()
        {
            return View();
        }
    }
}