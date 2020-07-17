using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Sopro.ViewModels;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Sopro.Models.User;
using Sopro.Models.Administration;
using Sopro.Interfaces.AdministrationController;
using System.IO;
using Sopro.Interfaces.PersistenceController;
using Sopro.Persistence.PersBooking;

namespace Sopro.Controllers
{
    public class AdminController : Controller
    {
        private IMemoryCache cache;
        List<IBooking> bookings;
        private IBookingService bookingService = new BookingService();

        public AdminController(IMemoryCache _cache)
        {
            cache = _cache;
        }
        public IActionResult Index()
        {
            //Session for the role of the User
            var userID = HttpContext.Session.GetString("role");
            if (userID == null)
            {
                return RedirectToAction("Index", "Home");
            }
            var cacheKey = CacheKeys.BOOKING;
            if (!cache.TryGetValue(cacheKey, out bookings))
            {
                bookings = new List<IBooking>();
            }
            if (userID.Equals(UserType.PLANER.ToString()))
            {
                List<Booking> unscheduledBookings = new List<Booking>();
                List<Booking> scheduledBookings = new List<Booking>();
                foreach (IBooking item in bookings)
                {
                    if (item.station == null)
                    {
                        unscheduledBookings.Add((Booking)item);
                    }
                    else if (item.station != null)
                    {
                        scheduledBookings.Add((Booking)item);
                    }
                }
                return View(new DashboardViewModel(scheduledBookings, unscheduledBookings));
            }
            else
            {
                return RedirectToAction("Index", "Booking");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Import([FromForm] FileViewModel model)
        {
            IFormFile file = model.importedFile;
            string cacheKey = CacheKeys.BOOKING;

            if (!cache.TryGetValue(cacheKey, out bookings))
            {
                bookings = new List<IBooking>();
            }

            string path = Path.GetFullPath(file.Name);

            List<IBooking> importedBookings = bookingService.import(path);

            foreach (IBooking item in importedBookings)
            {
                bookings.Add(item);
            }

            cache.Set(cacheKey, bookings);

            return View("Index", bookings);
        }

        [HttpGet]
        public IActionResult Export([FromForm] FileViewModel model)
        {
            IFormFile file = model.exportedFile;
            cache.TryGetValue(CacheKeys.BOOKING, out bookings);

            string path = Path.GetFullPath(file.Name); ;

            bookingService.export(bookings, path);

            return View("Index", bookings);
        }
    }

}
