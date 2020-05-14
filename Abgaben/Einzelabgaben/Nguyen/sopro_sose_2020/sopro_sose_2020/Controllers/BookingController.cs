using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DocumentFormat.OpenXml.Drawing;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using sopro_sose_2020.Models;

namespace sopro_sose_2020.Controllers
{
    public class BookingController : Controller
    {
        List<Models.Booking> bookingList;
        private readonly IMemoryCache _memoryCache;

        public BookingController(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }
        public IActionResult Index()
        {
            var cacheKey = "bookingList";
            _memoryCache.TryGetValue(cacheKey, out bookingList); //read from cache , if null see Index.cshtml
            
           
            return View(bookingList);
        }

        public IActionResult create()
        {
            
            return View();
        }
        public void get()
        {
            // warum zum fick soll ich ne extra get funktion machen bzw wofür lol
        }
        public IActionResult postBooking(Booking _booking) //modelbind post from html
        {
            var cacheKey = "bookingList";
            if (!_memoryCache.TryGetValue(cacheKey, out bookingList)) //read from cache to bookingList matching key
            {
                bookingList = new List<Booking>();
            }
            bookingList.Add(_booking); 
            _memoryCache.Set(cacheKey, bookingList); //overwrite old if necessary
            return View("Index", bookingList);
        }
    }
}