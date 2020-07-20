using Sopro.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
            return this.location.distributor.run(new List<Booking> { this });
        }
    }
}
