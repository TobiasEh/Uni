using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using DocumentFormat.OpenXml.Drawing;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using sopro_sose_2020.Models;
using sopro_sose_2020.ViewModel.Booking;

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

        [HttpGet]
        public IActionResult create()
        {
            
            return View(new Booking());
        }

        [HttpPost]
        public IActionResult postBooking(Booking _booking) //modelbind post from html
        {
            if (ModelState.IsValid)
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
            return Content("Error - incorrect input");
        }
        public IActionResult Evaluation()
        {
            var cacheKey = "bookingList";
            if(!_memoryCache.TryGetValue(cacheKey, out bookingList))
            {
                return View();
            };
            var controller = new EvaluationController();
            return View(controller.Evaluation(bookingList));
        }
        
        
    }

}