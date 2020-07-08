using sopro2020_abgabe.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sopro.ViewModels
{
    public class BookingCreateViewModel
    {
        private List<Location> locations { get; set; }
        private IBooking booking { get; set; }
    }
}
