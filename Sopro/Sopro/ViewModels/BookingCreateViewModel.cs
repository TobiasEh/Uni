using Sopro.Models;
using Sopro.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sopro.Models.Administration;
using Sopro.Models.Infrastructure;

namespace Sopro.ViewModels
{
    public class BookingCreateViewModel
    {
        private List<ILocation> locations { get; set; }
        private Booking booking { get; set; }

        public BookingCreateViewModel(List<ILocation> _locations, Booking _booking)
        {
            locations = _locations;
            booking = _booking;
        }
    }
    
}
