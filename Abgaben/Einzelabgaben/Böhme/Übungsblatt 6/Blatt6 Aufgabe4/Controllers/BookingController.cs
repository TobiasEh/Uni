using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Blatt3_Aufgabe4.Models;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.AspNetCore.DataProtection.KeyManagement.Internal;
using System.Collections;
using Blatt3_Aufgabe4.ViewModel;

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
        public IActionResult Auswertung()
        {
            string cacheKey = "bookings";
            if (_memorycache.TryGetValue(cacheKey, out bookings))
            {
                List<ConnectorTypeEvaluationViewModel> evData = new List<ConnectorTypeEvaluationViewModel>();

                int[] plugsIn = new int[6];
                foreach (Booking e in bookings)
                {
                    plugsIn[(int)(e.connectorType)]++;
                }

                int totalCon = plugsIn.Sum();

                foreach (ConnectorType t in Enum.GetValues(typeof(ConnectorType)))
                {
                    evData.Add(new ConnectorTypeEvaluationViewModel()
                    {
                        connectorType = t,
                        percentage = Math.Round((double)plugsIn[(int)t] / (double)totalCon * 100, 2)
                    });
                }
                cacheKey = "evaluation";
               _memorycache.Set(cacheKey, evData);
                return View(evData);
            } else
            {
                return View();
            }
           
        }
        
    }
}
