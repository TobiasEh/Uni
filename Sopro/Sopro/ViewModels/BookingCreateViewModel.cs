using sopro2020_abgabe.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using sopro2020_abgabe.Interfaces;

namespace Sopro.ViewModels
{
    public class BookingCreateViewModel
    {
        private List<Location> locations { get; set; }
        private Booking booking { get; set; }

        public BookingCreateViewModel(List<Location> _locations, Booking _booking)
        {
            locations = _locations;
            booking = _booking;
        }
    }
    
}
