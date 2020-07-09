using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.DataProtection.KeyManagement.Internal;
using Microsoft.Extensions.Caching.Memory;
using SoPro.Interfaces;

namespace Sopro.Models.Administration
{   //jede location hat ein distributer und einen schedule
    public class Distributor
    {
        IMemoryCache cache;
        private Schedule schedule { get; set; }
        private IDistributionStrategy strategy { get; set; }
        private BookingLocationFilter filter { get; set; }
        private int puffer {get; set; } = 15;
        private NotificationManager notificationManager;
        private ILocation location;
        private int timespan { get; set; }

        public Distributor(IDistributionStrategy _strategy, Schedule _schedule, int _puffer, ILocation _location, int _timespan)
        {
            strategy = _strategy;
            timespan = _timespan;
            schedule = _schedule;
            puffer = _puffer;
            location = _location;
            filter = new BookingLocationFilter(location, timespan);
        }

        public Distributor(IDistributionStrategy _strategy, Schedule _schedule, ILocation _location, int _timespan)
        {
            strategy = _strategy;
            schedule = _schedule;
            location = _location;
            timespan = _timespan;
            filter = new BookingLocationFilter(location, timespan);
        }

        public Distributor(IDistributionStrategy _strategy, Schedule _schedule, ILocation _location, int _puffer)
        {
            strategy = _strategy;
            schedule = _schedule;
            location = _location;
            puffer = _puffer;
            filter = new BookingLocationFilter(location);
        }

        //TODO
        public bool run(DateTime now)
        {
            List<Booking> bookings;
            bookings = cache.Get<List<Booking>>(CacheKey.BOOKING);

            bookings = filter.filter(bookings, now);
            if (bookings == null || bookings.Count() == 0)
                return false;
            if (!strategy.distribute(bookings, schedule, puffer))
                return false;
            return true;
        }
    }
}
