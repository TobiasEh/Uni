using Sopro.Interfaces;
using System.Collections.Generic;
using Sopro.Models.Administration;

namespace Sopro.ViewModels
{
    public class BookingCreateViewModel
    {
        public List<ILocation> locations { get; set; }
        public Booking booking { get; set; }
        public bool ccs { get; set; }
        public bool type2 { get; set; }

        public BookingCreateViewModel(List<ILocation> _locations, Booking _booking, bool _ccs, bool _type2)
        {
            locations = _locations;
            booking = _booking;
            ccs = _ccs;
            type2 = _type2;
        }
    }
    
}
