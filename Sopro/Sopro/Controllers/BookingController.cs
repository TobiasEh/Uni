using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Sopro.ViewModels;
using System;
using System.Collections.Generic;
using Sopro.Interfaces;
using Microsoft.AspNetCore.Http;
using Sopro.Models.User;
using Sopro.Models.Administration;
using Sopro.Interfaces.AdministrationController;
using System.IO;
using Sopro.Models.Infrastructure;
using Sopro.Interfaces.PersistenceController;
using Sopro.Persistence.PersBooking;

namespace Sopro.Controllers
{
    public class BookingController : Controller
    {
        private IMemoryCache cache;
        List<IBooking> bookings;
        private IBookingService service = new BookingService();

        public BookingController(IMemoryCache _cache)
        {
            cache = _cache;
        }

        /* Method to show Index or Dashobard.
         * If the User has the role "Planer", then Admin.Dashboard view is returned,
         * with scheduled and unscheduled bookings.
         * Else the Booking.Index view with all bookings is returned.
         */
        public IActionResult Index()
        {
            //Session for the role of the User
            var userID = this.HttpContext.Session.GetString("role");
            if (userID == null)
            {
                return RedirectToAction("Index", "Home");
            }
            List<Booking> unscheduledBookings = new List<Booking>();
            List<Booking> scheduledBookings = new List<Booking>();
            var cacheKey = CacheKeys.BOOKING;
            if (!cache.TryGetValue(cacheKey, out bookings))
            {
                bookings = new List<IBooking>();
            }
            if (userID.Equals(UserType.PLANER.ToString()))
            {
                return RedirectToAction("Index", "Admin");
            }
            else if (userID.Equals(UserType.ASSISTANCE.ToString()))
                {
                foreach (IBooking item in bookings)
                    if (item.priority == UserType.VIP|| item.priority == UserType.GUEST)
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
                var email = this.HttpContext.Session.GetString("email");
                foreach (IBooking item in bookings) 
                    if (item.user == email)
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
        }

        /* Returns Booking.Create view.
         */
        public IActionResult Create(Booking booking)
        {
            var cacheKey = CacheKeys.LOCATION;
            List<ILocation> locations = (List<ILocation>)cache.Get(cacheKey);
            if (locations == null)
            {
                locations = new List<ILocation>();
            }
            if(booking.startTime==new DateTime() && booking.endTime==new DateTime())
            {
                DateTime now = DateTime.Now;
                now = new DateTime(now.Year, now.Month, now.Day, now.Hour, now.Minute,0);
                booking.startTime = now;
                booking.endTime = now;
            }
            return View("Create", new BookingCreateViewModel(locations, booking, booking.plugs.Contains(PlugType.CCS), booking.plugs.Contains(PlugType.TYPE2)));           
        }

        /* Method to show all bookings in UI.
         * Checks if cache already exists.
         * When booking is valid it is added to 
         * the bookinglist and the cache.
         * Throws "exception" when booking is not valid.
         * Returns Booking.Index view, with bookinglist.
         */
        [HttpPost]
        public IActionResult Post(BookingCreateViewModel viewmodel)
        {
            IBooking booking = viewmodel.booking;
            List < PlugType > plugs = new List<PlugType>();

            List<ILocation> locations = (List<ILocation>)cache.Get(CacheKeys.LOCATION);
            if (locations == null)
            {
                locations = new List<ILocation>();
            }
            if(booking.priority == UserType.EMPLOYEE)
            {
                booking.user = this.HttpContext.Session.GetString("email");
            }
            foreach (ILocation l in locations) 
            {
                if(l.id.Equals(viewmodel.locationId))
                {
                    booking.location = l;
                }
            }
            if (viewmodel.ccs)
            {
                plugs.Add(PlugType.CCS);
            } 
            if (viewmodel.type2)
            {
                plugs.Add(PlugType.TYPE2);
            }
            booking.plugs = plugs;
            var cacheKey = CacheKeys.BOOKING;
            if (!cache.TryGetValue(cacheKey, out bookings))
            {
                bookings = new List<IBooking>();
            }
            if (!TryValidateModel(booking, nameof(booking)))
            {
                return RedirectToAction("Create", "Booking", booking);
                //throw new Exception("Buchung ist nicht valide!");
            }
            DateTime test = DateTime.Now.Date;
            if(booking.startTime.Date == DateTime.Now.Date)
            {
                Adhoc adhoc = new Adhoc(false) { 
                    capacity = booking.capacity, 
                    plugs = booking.plugs, socStart = 
                    booking.socStart, 
                    socEnd = booking.socEnd, 
                    user = booking.user, 
                    startTime = booking.startTime, 
                    endTime = booking.endTime,
                    active = booking.active,
                    location = booking.location,
                    priority = booking.priority
                };
                bookings.Add(adhoc);
                cache.Set(cacheKey, bookings);
                adhoc.triggerBookingDistribution();
                return RedirectToAction("Index", "Booking");
            }
            bookings.Add(booking);
            cache.Set(cacheKey, bookings);
            return RedirectToAction("Index", "Booking");
        }

        /* Method to edit already existing bookings.
         * Booking is removed from bookinglist and cache.
         * Returns Booking.Create view with already filled fields.
         */
        public IActionResult Edit(string bookingID)
        {
            var cacheKey = CacheKeys.BOOKING;
            if (!cache.TryGetValue(cacheKey, out bookings))
            {
                bookings = new List<IBooking>();
            }
            Booking booking = null;
            foreach (Booking b in bookings)
            {
                if (b.id.Equals(bookingID))
                {
                    booking = b;
                    break;
                }
            }
            
            bookings.Remove(booking);
            cache.Set(cacheKey, bookings);
            List<ILocation> locations;
            if (!cache.TryGetValue(CacheKeys.LOCATION, out locations))
            {
                locations =new List<ILocation>();
            }
            
            
            BookingCreateViewModel viewmodel = new BookingCreateViewModel(locations, booking, false, false);
            if (booking.plugs.Contains(PlugType.CCS))
            {
                viewmodel.ccs = true;
            }


            if (booking.plugs.Contains(PlugType.TYPE2))
            {
                viewmodel.type2 = true;
            }
            return View("Create", viewmodel);
        }

        /* Method to delete existing booking.
         * Booking is removed from bookinglist and cache.
         * Returns Booking.Index view, without the given booking.
         */
        public IActionResult Delete(string bookingID)
        {
            var cacheKey = CacheKeys.BOOKING;
            if (!cache.TryGetValue(cacheKey, out bookings))
            {
                bookings = new List<IBooking>();
            }
            Booking booking = null;
            foreach (Booking b in bookings)
            {
                if (b.id.Equals(bookingID))
                {
                    booking = b;
                    break;
                }
            }

            bookings.Remove(booking);
            cache.Set(cacheKey, bookings);

            return RedirectToAction("index");
        }

        /* Method takes care about Check-In/-Out.
         * Therefore it changes the attribute active of given booking to the opposite boolean.
         * Returns Booking.Index view, with bookinglist.
         */
        public IActionResult ToggleCheck(IBooking booking)
        {
            var cacheKey = CacheKeys.BOOKING;

            int index = bookings.IndexOf(booking);
            booking.location.schedule.toggleCheck((Booking)booking);
            bookings[index].active = !bookings[index].active;

            cache.Set(cacheKey, bookings);
            return View("Index", bookings);

        }
        /*
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Import([FromForm]FileViewModel model)
        {
            IFormFile file = model.importedFile;
            List<Booking> importedBookings = service.Import(file);

            if (!cache.TryGetValue(CacheKeys.BOOKING, out bookings))
            {
                bookings = new List<IBooking>();
            }

            List<ILocation> locations;
            if (!cache.TryGetValue(CacheKeys.LOCATION, out locations))
            {
                locations = new List<ILocation>();
            }

            foreach (IBooking boo in importedBookings)
            {
                if (!bookings.Contains(boo))
                {
                    bookings.Add(boo);

                    if (!locations.Contains(boo.location))
                    {
                        locations.Add(boo.location);
                    }

                }
            }

            cache.Set(CacheKeys.LOCATION, locations);
            cache.Set(CacheKeys.BOOKING, bookings);
            return View("Index", bookings);
        }

        public IActionResult Export([FromForm]FileViewModel model)
        {
            cache.TryGetValue(CacheKeys.BOOKING, out bookings);
            IFormFile file = model.exportedFile;
            string path = Path.GetFullPath(file.Name);
            service.export(bookings);

            return View("Index", bookings);
        }
        */
    }

}
