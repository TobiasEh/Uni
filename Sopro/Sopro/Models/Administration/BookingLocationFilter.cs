using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Sopro.Models.Infrastructure;

namespace Sopro.Models.Administration
{
    public class BookingLocationFilter
    {
        private Location location { get; }
        private int timespan = 30;

        public BookingLocationFilter(Location _location, int _timespan)
        {
            location = _location;
            if (!checkTimespan(_timespan))
                throw new Exception("Zeitraum darf nicht negativ sein");
            else
                timespan = _timespan;
        }
        public BookingLocationFilter(Location _location)
        {
            location = _location;
        }

        /* Check if timespan in non-negativ.
         */
        private bool checkTimespan(int timespan)
        {
            if (timespan >= 0)
                return true;
            else
                return false;
        }

        /* Filters the List of bookings on Location and a period of time  
         * between [date, date+timespan].
         * Throws exception, when date is in the past.
         * Throws exception, when List of bookings is empty.
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
