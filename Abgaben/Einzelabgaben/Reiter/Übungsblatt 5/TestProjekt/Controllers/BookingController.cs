using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using TestProjekt.Models;
using TestProjekt.ViewModel;

namespace TestProjekt.Controllers
{
    public class BookingController : Controller
    {
        private readonly IMemoryCache _memoryCache;
        List<Booking> bookings = new List<Booking>();

        public BookingController(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        public IActionResult Index()
        {
            var cacheKey = "bookings";
            _memoryCache.TryGetValue(cacheKey, out bookings);

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
            var cacheKey = "bookings";

            if (!_memoryCache.TryGetValue(cacheKey, out bookings))
            {
                bookings = new List<Booking>();
            }

            if (ModelState.IsValid)
            {
                bookings.Add(booking);
                _memoryCache.Set(cacheKey, bookings);
            } 
            return View("Index", bookings);
        }

        public IActionResult Evaluation()
        {
            string cacheKey = "bookings";
            if (_memoryCache.TryGetValue(cacheKey, out bookings))
            {
                List<ConnectorTypeEvaluationViewModel> evaluation = new List<ConnectorTypeEvaluationViewModel>();

                int[] plugs = new int[6];
                foreach (Booking e in bookings)
                {
                    plugs[(int)(e.connectorType)]++;
                }

                int totalCon = plugs.Sum();

                foreach (ConnectorType ct in Enum.GetValues(typeof(ConnectorType)))
                {
                    evaluation.Add(new ConnectorTypeEvaluationViewModel()
                        {
                            connectorType = ct,
                            percBooking = plugs[(int)ct] / totalCon * 100
                        });
                }

                return View(evaluation);
            }
            else
            {
                return View();
            }

        }
    }
}