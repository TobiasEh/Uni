using Sopro.Models;
using SoPro.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sopro.ViewModels
{
    public class BookingCreateViewModel
    {
        private List<ILocation> locations { get; set; }
        private Booking booking { get; set; }

        public BookingCreateViewModel(List<Location> _locations, Booking _booking)
        {
            locations = _locations;
            booking = _booking;
        }
    }
    
}
