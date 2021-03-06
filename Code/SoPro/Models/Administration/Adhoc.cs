﻿using Sopro.Interfaces;
using System;
using System.Collections.Generic;

namespace Sopro.Models.Administration
{
    public class Adhoc : Booking, ITrigger
    {
        public Adhoc() : base()
        {
            triggerBookingDistribution();
        }

        public Adhoc(bool active) : base()
        {

        }

        public bool triggerBookingDistribution()
        {
            location.distributor.strategy = new AdHocDistribution();
            return location.distributor.run(DateTime.Now, new List<Booking> { this });
        }
    }
}