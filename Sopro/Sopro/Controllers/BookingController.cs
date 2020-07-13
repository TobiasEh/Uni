using Microsoft.AspNetCore.DataProtection.KeyManagement.Internal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Sopro.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using System.Diagnostics.Eventing.Reader;
using Sopro.Interfaces;
using Microsoft.AspNetCore.Http;
using Sopro.Models.User;
using Sopro.Models.Administration;
using Sopro.Interfaces.AdministrationController;

namespace Sopro.Controllers
{
    public class BookingController : Controller
    {
        private IMemoryCache cache;
        List<IBooking> bookings;

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
            if (userID.Equals(UserType.PLANER))
            {
               
                

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
                return RedirectToAction("Dashboard", "Admin", new DashboardViewModel(scheduledBookings, unscheduledBookings));
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
        public IActionResult Create()
        {
            var cacheKey = CacheKeys.LOCATION;
            List<ILocation> locations = (List<ILocation>)cache.Get(cacheKey);
            return View("Create", new BookingCreateViewModel(locations,(IBooking) new Booking()));           
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
            var email = HttpContext.Session.GetString("email");
            IBooking booking = viewmodel.booking;
            if (booking.user == null)
            {
                booking.user = email;
            }
            var cacheKey = CacheKeys.BOOKING;
            if (!cache.TryGetValue(cacheKey, out bookings))
            {
                bookings = new List<IBooking>();
            }
            if (!ModelState.IsValid)
            {
                return RedirectToAction("Create", "Booking");
                //throw new Exception("Buchung ist nicht valide!");
            }
            bookings.Add(booking);
            cache.Set(cacheKey, bookings);
            return RedirectToAction("Index", "Booking");
        }

        /* Method to edit already existing bookings.
         * Booking is removed from bookinglist and cache.
         * Returns Booking.Create view with already filled fields.
         */
        public IActionResult Edit(IBooking booking)
        {
            var cacheKey = CacheKeys.BOOKING;
            bookings.Remove(booking);
            cache.Set(cacheKey, bookings);

            return View("Create", booking);
        }

        /* Method to delete existing booking.
         * Booking is removed from bookinglist and cache.
         * Returns Booking.Index view, without the given booking.
         */
        public IActionResult Delete(IBooking booking)
        {
            var cacheKey = CacheKeys.BOOKING;
        
            bookings.Remove(booking);
            cache.Set(cacheKey, bookings);
            return View("Index", bookings);
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
    }
}
