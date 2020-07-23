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
using Sopro.Interfaces;

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
            // Session for the role of the User.
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
            if(file == null)
            {
                return RedirectToAction("Index");
            }
            List< BookingExportImportViewModel > importedBookings = bookingService.Import(file);

            if (!cache.TryGetValue(CacheKeys.BOOKING, out bookings))
            {
                bookings = new List<IBooking>();
            }

            List<ILocation> locations;
            if (!cache.TryGetValue(CacheKeys.LOCATION, out locations))
            {
                locations = new List<ILocation>();
            }

            foreach (BookingExportImportViewModel b in importedBookings)
            {
                IBooking boo = b.generateBooking();
                bool uniqeId = true;
                foreach(IBooking ib in bookings)
                {
                    if (ib.id.Equals(b.id))
                    {
                        uniqeId = false;
                    }
                }
                if (uniqeId)
                {
                    if (TryValidateModel(boo)) { 
                        bookings.Add(boo);

                        bool notIncluded = true;
                        foreach (ILocation loc in locations)
                        {
                            if (loc.id.Equals(boo.location.id))
                            {
                                notIncluded = false;
                                break;
                            }
                        }
                        if (notIncluded)
                        {
                            locations.Add(boo.location);
                        }
                    }
                }
            }

            cache.Set(CacheKeys.LOCATION, locations);
            cache.Set(CacheKeys.BOOKING, bookings);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Export([FromForm] FileViewModel model)
        {
            if (!cache.TryGetValue(CacheKeys.BOOKING, out bookings))
            {
                bookings = new List<IBooking>();
            }

            List<BookingExportImportViewModel> bookinglist = new List<BookingExportImportViewModel>();
            foreach(IBooking b in bookings)
            {
                bookinglist.Add(new BookingExportImportViewModel(b));
            }

            return bookingService.export(bookinglist);
        }
    }

}
