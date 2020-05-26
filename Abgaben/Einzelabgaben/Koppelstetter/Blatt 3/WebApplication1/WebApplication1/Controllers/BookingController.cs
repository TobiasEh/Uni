using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class BookingController : Controller
    {
        public IActionResult Index()
        {
            Booking b1 = new Booking()
            {
                current_Charge = 50,
                required_Distance = 80,
                start_Time = new DateTime(2020, 5, 10, 10, 0, 0),
                end_Time = new DateTime(2020, 5, 10, 14, 0, 0)
            };

            Booking b2 = new Booking()
            {
                current_Charge = 50,
                required_Distance = 80,
                start_Time = new DateTime(2020, 5, 10, 10, 0, 0),
                end_Time = new DateTime(2020, 5, 10, 14, 0, 0)
            };

            List<Booking> bookinglist = new List<Booking>()
            { 
                b1,
                b2
            };

            return View(bookinglist);
        }
    }
}
