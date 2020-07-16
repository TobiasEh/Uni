using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Sopro.Interfaces;

namespace Sopro.Models.Administration
{
    public class BookingLocationFilter
    {
        private ILocation location { get; set; }
        private int timespan { get; set; } = 30;

        public BookingLocationFilter(ILocation _location, int _timespan)
        {
            location = _location;
            timespan = _timespan;
        }
        public BookingLocationFilter(ILocation _location)
        {
            location = _location;
        }

        /* Filters the List of bookings on Location and 
         * if the startime is in range [now, now + timespan).
         */
        public List<Booking> filter(List<Booking> bookings)
        {
            DateTime time = DateTime.Now.AddDays(timespan);
            List<Booking> result = new List<Booking>();
            foreach (Booking item in bookings)
            {
                if (item.location == location && item.startTime < time && item.startTime >= DateTime.Now)
                {
                    result.Add(item);
                }
            }
            if (result.Count() == 0 || result == null)
                return result;
            else
            {
                return result;
            }
        }
    }
}
