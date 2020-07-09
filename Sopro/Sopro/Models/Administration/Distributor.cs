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
        private Schedule schedule { get; set; }
        private IDistributionStrategy strategy { get; set; }
        private BookingLocationFilter filter { get; set; }
        private int puffer {get; set; } = 15;
        private int timespan { get; set; }
        private NotificationManager notificationManager;
        private ILocation location;
        IMemoryCache cache;

        public Distributor(Schedule _schedule, ILocation _location)
        {
            schedule = _schedule;
            location = _location;
            filter = new BookingLocationFilter(location, timespan);
        }

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
