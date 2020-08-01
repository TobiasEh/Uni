using Sopro.Interfaces;
using System.Collections.Generic;
using Sopro.Models.Administration;
using System.ComponentModel.DataAnnotations;

namespace Sopro.ViewModels
{
    public class BookingCreateViewModel
    {
       
        public List<ILocation> locations { get; set; } = new List<ILocation>();
        public Booking booking { get; set; }
        public bool ccs { get; set; }
        public bool type2 { get; set; }
        [Required(ErrorMessage = "Location muss angegeben werden")]
        public string locationId { get; set; }

        public BookingCreateViewModel(List<ILocation> _locations, Booking _booking, bool _ccs, bool _type2)
        {
            locations = _locations;
            booking = _booking;
            ccs = _ccs;
            type2 = _type2;
        }

        public BookingCreateViewModel() { }
    }
    
}
