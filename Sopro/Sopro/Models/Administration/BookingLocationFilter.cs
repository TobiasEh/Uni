using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using SoPro.Interfaces;

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

        /* Filters the List of bookings on Location and a period of time  
         * between [date, date+timespan].
         * Throws exception, when after filtering no booking is found.
         */
        public List<Booking> filter(List<Booking> bookings, DateTime date)
        {
                DateTime end = date.AddDays(timespan);
                foreach (Booking item in bookings)
                {
                    if (item.location != location && item.startTime >= date && item.startTime <= end)
                    {
                        bookings.Remove(item);
                    }
                }
                if (bookings.Count() == 0)
                {
                    throw new Exception("Es wurde keine Buchung gefunden die in " + location + ", zwischen " + date + "und " + end + "liegt");
                }
                else
                {
                    return bookings;
                }
        }
    }
}
