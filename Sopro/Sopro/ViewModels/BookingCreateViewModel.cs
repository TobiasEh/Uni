using Sopro.Models;
using Sopro.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sopro.Models.Administration;
using Sopro.Models.Infrastructure;
using Sopro.Interfaces.AdministrationController;

namespace Sopro.ViewModels
{
    public class BookingCreateViewModel
    {
        public List<ILocation> locations { get; set; }
        public IBooking booking { get; set; }

        public BookingCreateViewModel(List<ILocation> _locations, IBooking _booking)
        {
            locations = _locations;

            booking = _booking;
        }

        public BookingCreateViewModel()
        {
            locations = new List<ILocation>();

            booking = (IBooking)new Booking();
        }
    }
    
}
