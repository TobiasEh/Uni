using Sopro.Interfaces;
using System.Collections.Generic;

namespace Sopro.Models.Administration
{
    public class AdHoc : Booking, ITrigger
    {
        public AdHoc() : base()
        {
            triggerBookingDistribution();
        }

        public AdHoc(bool active) : base()
        {

        }

        public bool triggerBookingDistribution()
        {
            this.location.distributor.strategy = new AdHocDistribution();
            return this.location.distributor.run(new List<Booking> { this });
        }
    }
}