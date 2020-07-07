using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sopro.Models.Administration
{   //jede location hat ein distributer und einen schedule
    public class Distributor
    {
        public List<Booking> bookings;
        private Schedule schedule { get; }
        private IDistributionStrategy strategy;
        private BookingLocationFilter filter { get; }
        private int puffer = 15;

        public Distributor(IDistributionStrategy _strategy, List<Booking> _bookings, Schedule _schedule, int _puffer)
        {
            strategy = _strategy;
            bookings = _bookings;
            schedule = _schedule;
            puffer = _puffer;
        }

        public Distributor(IDistributionStrategy _strategy, List<Booking> _bookings, Schedule _schedule)
        {
            strategy = _strategy;
            bookings = _bookings;
            schedule = _schedule;
        }

        //TODO
        public bool run(DateTime now)
        {
            try
            {
                if (!strategy.distribute(bookings, schedule, puffer))
                    return false;
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }
    }
}
