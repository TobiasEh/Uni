using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sopro.Models.Administration
{   //jede location hat ein distributer und einen schedule
    public class Distributor
    {
        public List<Booking> bookings;
        private Schedule schedule { get; set; }
        private IDistributionStrategy strategy { get; set; }
        private BookingLocationFilter filter { get; set; }
        private int puffer {get; set; } = 15;
        private NotificationManager notificationManager;

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
