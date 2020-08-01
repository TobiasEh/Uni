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
using Sopro.Models.Infrastructure;

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

        public IActionResult Edit(string id)
        {
            // Die Buchungen werden aus dem Cache geladen.
            var cacheKey = CacheKeys.BOOKING;
            if (!cache.TryGetValue(cacheKey, out bookings))
            {
                bookings = new List<IBooking>();
            }

            // Herausfiltern der Buchung, welche die übergebene Id hat.
            Booking booking = bookings.Find(x => x.id == id) as Booking;

            // Die Liste der Standorte wird aus dem Cache geladen.
            List<ILocation> locations;
            if (!cache.TryGetValue(CacheKeys.LOCATION, out locations))
            {
                locations = new List<ILocation>();
            }

            // Anlegen des ViewModels für die Seite.
            BookingCreateViewModel viewmodel = new BookingCreateViewModel(locations, booking, false, false);
            if (booking.plugs.Contains(PlugType.CCS))
            {
                viewmodel.ccs = true;
            }
            if (booking.plugs.Contains(PlugType.TYPE2))
            {
                viewmodel.type2 = true;
            }

            return View(viewmodel);
        }
        [HttpPost]
        public IActionResult Edit(string id, BookingCreateViewModel viewmodel)
        {
            if (!ModelState.IsValid)
            {
                return View(id, viewmodel);
            }
            IBooking booking = viewmodel.booking;
            booking.id = id;

            // Die Liste der Standorte wird aus dem Cache geladen.
            List<ILocation> locations = (List<ILocation>)cache.Get(CacheKeys.LOCATION);
            if (locations == null)
            {
                locations = new List<ILocation>();
            }
            // Die Buchungen werden aus dem Cache geladen.
            var cacheKey = CacheKeys.BOOKING;
            if (!cache.TryGetValue(cacheKey, out bookings))
            {
                bookings = new List<IBooking>();
            }
            booking.user = bookings[bookings.FindIndex(x => x.id == viewmodel.booking.id)].user;
            booking.priority = bookings[bookings.FindIndex(x => x.id == viewmodel.booking.id)].priority;
            // Filtern des richtigen Standorts aus der Liste der Standorte.
            booking.location = locations.Find(x => x.id == viewmodel.locationId);

            // Befüllen der Liste an Stecker, welche zum Auto des Benutzers passen.
            List<PlugType> plugs = new List<PlugType>();
            if (viewmodel.ccs)
            {
                plugs.Add(PlugType.CCS);
            }
            if (viewmodel.type2)
            {
                plugs.Add(PlugType.TYPE2);
            }
            booking.plugs = plugs;

            // Hinzufügen der Buchung zum Cache.
            bookings[bookings.FindIndex(x => x.id == viewmodel.booking.id)] = booking;
            cache.Set(cacheKey, bookings);

            return RedirectToAction("Index");
        }

        public IActionResult Delete(string id)
        {
            if (!cache.TryGetValue(CacheKeys.BOOKING, out bookings))
            {
                bookings = new List<IBooking>();
            }
            bookings.RemoveAll(x => x.id == id);

            cache.Set(CacheKeys.BOOKING, bookings);
            return RedirectToAction("Index");
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
