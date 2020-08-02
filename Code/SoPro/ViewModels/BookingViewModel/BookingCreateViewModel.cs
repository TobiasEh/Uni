using Sopro.Interfaces;
using System.Collections.Generic;
using Sopro.Models.Administration;
using Sopro.ValidationAttributes;
using System.ComponentModel.DataAnnotations;

namespace Sopro.ViewModels
{
    public class BookingCreateViewModel
    {
        public List<ILocation> locations { get; set; } = new List<ILocation>();
        public Booking booking { get; set; }
        [AtleastOnePlug("ccs", "type2", ErrorMessage = "Es muss mindestens ein Plug gewählt sein.")]
        public bool ccs { get; set; } = false;
        [AtleastOnePlug("ccs", "type2", ErrorMessage = "Es muss mindestens ein Plug gewählt sein.")]
        public bool type2 { get; set; } = false;
        [Required(ErrorMessage = "Der Standort muss gesetzt sein.")]
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
