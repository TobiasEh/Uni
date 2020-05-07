using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DocumentFormat.OpenXml.Drawing;
using Microsoft.AspNetCore.Mvc;
using sopro_sose_2020.Models;

namespace sopro_sose_2020.Controllers
{
    public class BookingController : Controller
    {
        public IActionResult Index()
        {
            Random rnd = new Random();
            DateTime startTime = new DateTime(2020, 05, 07, 22, 10, 00);
            DateTime rndStartTime = new DateTime();
            List<Models.Booking> bookingList = new List<Models.Booking>() ;
            for (int i = 0; i < 10; i++)
            {
                rndStartTime = startTime.AddHours(rnd.NextDouble()*240);
                bookingList.Add(
                    new Models.Booking() { cur_charge = Math.Round(rnd.NextDouble() * 100, 2), needed_distance = rnd.Next(100), startTime = rndStartTime, endTime = rndStartTime.AddHours(10) }
                    );
            };
            return View(bookingList);
        }
    }
}