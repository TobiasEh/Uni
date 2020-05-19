using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Blatt3_Aufgabe4.Models;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.AspNetCore.DataProtection.KeyManagement.Internal;
using System.Collections;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Blatt3_Aufgabe4.Controllers
{
    public class BookingController : Controller
    {
        private readonly IMemoryCache _memorycache;
        List<Booking> bookings = new List<Booking>();

        public BookingController(IMemoryCache memoryCache)
        {
            _memorycache = memoryCache;
        }
        
        public IActionResult Index()
        {
            string cacheKey = "bookings";
            _memorycache.TryGetValue(cacheKey, out bookings);
            
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
            string cacheKey = "bookings";
            if (!_memorycache.TryGetValue(cacheKey, out bookings))
            {
                bookings = new List<Booking>();
            }

            bookings.Add(booking);
            _memorycache.Set(cacheKey, bookings);
            return View("Index", bookings);
        }
    }
}
