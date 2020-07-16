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
    public class AdminController : Controller
    {
        private IMemoryCache cache;
        List<IBooking> bookings;

        public AdminController(IMemoryCache _cache)
        {
            cache = _cache;
        }
        public IActionResult Index()
        {

            //Session for the role of the User
            var userID = HttpContext.Session.GetString("ID");
            if (userID.Equals(UserType.PLANER))
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
                return RedirectToAction("Dashboard", "Admin", new DashboardViewModel(scheduledBookings, unscheduledBookings));
            }
            else
            {
                return View("Index", "Booking");
            }
        }
    }
}
