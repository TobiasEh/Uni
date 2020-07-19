using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Caching.Memory;
using Sopro.Interfaces;
using Sopro.Controllers;
using System;

namespace Sopro.Models.Administration
{
    public class Distributor
    {
        private Schedule schedule { get; set; }
        public IDistributionStrategy strategy { get; set; }
        private BookingLocationFilter filter { get; set; }
        private int buffer {get; set; } = 15;
        private NotificationManager notificationManager;
        private ILocation location;
        private IMemoryCache cache;


        public Distributor()
        {
            strategy = new StandardDistribution();
        }

            public Distributor(Schedule _schedule, ILocation _location)
        {
            schedule = _schedule;
            location = _location;
            filter = new BookingLocationFilter(location);
            notificationManager = new NotificationManager();
        }

        /* Method calls other method to distribute booking.
         * If a booking is not distributed, the user will be notified.
         */
        public bool run(IMemoryCache cache)
        {
            List<Booking> bookings;
            if(cache.TryGetValue(CacheKeys.BOOKING, out bookings))
            {
                Console.WriteLine("Before filter: " + bookings.Count().ToString());
                bookings = filter.filter(bookings);
                Console.WriteLine("After filter: " + bookings.Count().ToString());
                if (bookings == null || bookings.Count() == 0)
                    return false;
                if (!strategy.distribute(bookings, schedule, buffer))
                    return false;
                /*
                foreach (Booking item in bookings)
                {
                    if (item.station == null)
                        notificationManager.notify(item, NotificationEvent.DECLINED);
                }
                */
                return true;
            }
            return false;
        }

        public bool run(List<Booking> toBeDistributed)
        {
            return false;
        }
    }
}
