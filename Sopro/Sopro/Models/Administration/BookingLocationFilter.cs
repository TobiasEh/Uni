using System;
using System.Collections.Generic;
using Sopro.Interfaces;

namespace Sopro.Models.Administration
{
    public class BookingLocationFilter
    {
        public ILocation location { get; set; }
        public int timespan { get; set; } = 30;
        public DateTime startTime { get; set; }

        public BookingLocationFilter(ILocation _location, int _timespan)
        {
            location = _location;
            timespan = _timespan;
        }
        public BookingLocationFilter(ILocation _location)
        {
            location = _location;
        }

        public BookingLocationFilter(DateTime start)
        {
            startTime = start;
        }

        /* Filters the List of bookings on Location and 
         * if the startime is in range [now, now + timespan).
         */
        public List<Booking> filter(List<Booking> bookings)
        {
            DateTime time = DateTime.Now.Add(new TimeSpan(timespan, 0, 0, 0));

            List<Booking> result = new List<Booking>();
            foreach (Booking item in bookings)
            {
                // Console.WriteLine("BookingLocationFilter.cs Condition 1: " + (item.location == location).ToString());
                // Console.WriteLine("BookingLocationFilter.cs Condition 2: " + (item.startTime < time).ToString());
                // Console.WriteLine("BookingLocationFilter.cs Condition 3: " + (item.startTime >= DateTime.Now).ToString());
                // Console.WriteLine(time.ToString());
                // Console.WriteLine(item.startTime.ToString());
                if (item.location == location && item.startTime < time && item.startTime >= DateTime.Now)
                {
                    result.Add(item);
                }
            }
            return result;
        }
    }
}
