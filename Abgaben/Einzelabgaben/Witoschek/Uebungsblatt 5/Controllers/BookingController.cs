using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blatt03.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections;
using Microsoft.Extensions.Caching.Memory;
using Blatt03.ViewModel;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Blatt03.Controllers
{
    public class BookingController : Controller
    {
        private readonly IMemoryCache _memoryCache;
        List<Booking> bookings;

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

            bookings.Add(booking);
            _memoryCache.Set(cacheKey, bookings);
            return View("Index", bookings);
        }

        public IActionResult Evaluation()
        {
            var cacheKey = "bookings";
            if (!_memoryCache.TryGetValue(cacheKey, out bookings))
            {
                return View();
            }

            List<ConnectorTypeEvaluationViewModel> evaluationData = new List<ConnectorTypeEvaluationViewModel>() { };
            
            int[] plugAppearances = new int[Enum.GetNames(typeof(ConnectorType)).Length];
            foreach (Booking b in bookings)
            {
                plugAppearances[(int)(b.connectorType)]++;
            }
                
            int totalConnectors = plugAppearances.Sum();

            foreach (ConnectorType c in Enum.GetValues(typeof(ConnectorType)))
            {
                evaluationData.Add(new ConnectorTypeEvaluationViewModel()
                {
                    connectorType = c,
                    percAppearsInBookings = Math.Round((double)plugAppearances[(int)c] / (double)totalConnectors * 100, 2)
                });
            }

            return View(evaluationData);
        }
    }
}
