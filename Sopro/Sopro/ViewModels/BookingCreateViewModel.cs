using Sopro.Interfaces;
using System.Collections.Generic;
using Sopro.Models.Administration;

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
