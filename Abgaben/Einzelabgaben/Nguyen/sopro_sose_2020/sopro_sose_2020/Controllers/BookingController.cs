using System;
using System.Collections;
using System.Collections.Generic;
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
            
            return View();
        }

        [HttpPost]
        public IActionResult postBooking(Booking _booking) //modelbind post from html
        {
            var cacheKey = "bookingList";
            if (!_memoryCache.TryGetValue(cacheKey, out bookingList)) //read from cache to bookingList matching key
            {
                bookingList = new List<Booking>();
            }
            if (!dateTest(_booking.startTime,_booking.endTime))
            {
                return Content("Don't fool me!");
            }
            bookingList.Add(_booking); 
            _memoryCache.Set(cacheKey, bookingList); //overwrite old if necessary
            return View("Index", bookingList);
        }
        public bool dateTest(DateTime start, DateTime end)
        {
            if(start < DateTime.Now || start >= end)
            {
                return false;
            }
            return true;
        }
        public IActionResult Evaluation()
        {
            var cacheKey = "bookingList";
            if(!_memoryCache.TryGetValue(cacheKey, out bookingList))
            {
                return View();
            };
            List<ConnectorTypeEvaluationViewModel> EvaList = new List<ConnectorTypeEvaluationViewModel>() { };
            int i = 0;
            double ac = 0, bc = 0, cc = 0; // "a-Count" == ac [...]
           foreach(Booking b in bookingList)
            {
                if(b.connectorType ==  ConnectorType.type_a)
                {
                    ac++;

                }else if (b.connectorType ==  ConnectorType.type_b)
                {
                    bc++;
                }else if(b.connectorType == ConnectorType.type_c)
                {
                    cc++;
                }
                i++; 
            }


            EvaList.Add(new ConnectorTypeEvaluationViewModel()
            {
                connectorType = ConnectorType.type_a,
                percOfBookingsCT = Math.Round((ac / i) * 100, 2)

            });
            EvaList.Add(new ConnectorTypeEvaluationViewModel()
            {
                connectorType = ConnectorType.type_b,
                percOfBookingsCT = Math.Round((bc / i) * 100, 2)

            });
            EvaList.Add(new ConnectorTypeEvaluationViewModel()
            {
                connectorType = ConnectorType.type_c,
                percOfBookingsCT = Math.Round((cc / i) * 100, 2)

            });
            cacheKey = "eva";
            _memoryCache.Set(cacheKey, EvaList);
            return View(EvaList);
        }
    }
}