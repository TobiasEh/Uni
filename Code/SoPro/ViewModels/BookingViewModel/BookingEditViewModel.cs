﻿using Sopro.Interfaces;
using System.Collections.Generic;
using Sopro.Models.Administration;
using Sopro.Interfaces.AdministrationController;

namespace Sopro.ViewModels
{
    public class BookingEditViewmodel
    {
        public List<ILocation> locations { get; set; } = new List<ILocation>();
        public IBooking booking { get; set; }
        public bool ccs { get; set; }
        public bool type2 { get; set; }
        public string locationId { get; set; }

        public BookingEditViewmodel(List<ILocation> _locations, IBooking _booking, bool _ccs, bool _type2)
        {
            locations = _locations;
            booking = _booking;
            ccs = _ccs;
            type2 = _type2;
        }

        public BookingEditViewmodel() { }
    }
    
}
