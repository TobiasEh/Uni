using Microsoft.AspNetCore.DataProtection.KeyManagement.Internal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Sopro.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using sopro2020_abgabe.Interfaces.IBooking;

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
         * If the User has the role "Planer", then Admin.Dashboard view is returned.
         * Else the Booking.Index view.
         */
        public IActionResult Index()
        {
            var cacheKey = "bookings";
            cache.TryGetValue(cacheKey, out bookings);

            if (/*rolle Planer*/)
            {
                return View("Admin.Dashboard", new DashboardViewModel());
            }
            else /*nicht rolle Planer*/
            {
                return View("Index", bookings);
            }
        }

        /* Returns Booking.Create view.
         */
        public IActionResult Create()
        {
            return View("Booking.Create", new BookingCreateViewModel());           
        }

        /* Method to show all bookings in UI.
         * Checks if cache already exists.
         * When booking is valid it is added to 
         * the bookinglist and the cache.
         * Throws "exception" when booking is not valid.
         * Returns Booking.Index view, with bookinglist.
         */
        [HttpPost]
        public IActionResult Post(IBooking booking)
        {
            var cacheKey = "bookings";
            if (!cache.TryGetValue(cacheKey, out bookings))
            {
                bookings = new List<IBooking>();
            }
            if (!ModelState.IsValid)
            {
                //Ausnahme
            }
            bookings.Add(booking);
            cache.Set(cacheKey, bookings);
            return View("Index", bookings);
        }

        /* Method to edit already existing bookings.
         * Booking is removed from bookinglist and cache.
         * Returns Booking.Create view with already filled fields.
         */
        public IActionResult Edit(IBooking booking)
        {
            var cacheKey = "bookings";
            bookings.Remove(booking);
            cache.Set(cacheKey, booking);

            return View("Create", booking);
        }

        /* Method to delete existing booking.
         * Booking is removed from bookinglist and cache.
         * Returns Booking.Index view, without the given booking.
         */
        public IActionResult Delete(IBooking booking)
        {
            var cacheKey = "bookings";
            bookings.Remove(booking);
            cache.Set(cacheKey, bookings);
            return View("Index", bookings);
        }
        
        /* 
         */
        public IActionResult ToggleCheck()
        {

        }
    }
}
