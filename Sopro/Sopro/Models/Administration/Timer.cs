using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sopro.Interfaces;

namespace Sopro.Models.Administration
{
    //TODO
    public class Timer : ITrigger
    {
        private DateTime distributionTime;
        private List<Distributor> distributor;
        public Timer(DateTime distributionTime)
        {
            this.distributionTime = distributionTime;
        }

        public bool triggerBookingDistribution(DateTime now)
        {
            if (distributor.run(now))
                return true;
            return false;
        }
    }
}
