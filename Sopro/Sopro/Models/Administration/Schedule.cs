using Microsoft.AspNetCore.Server.IIS.Core;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sopro.Models.Administration
{
    public class Schedule
    {
        private List<Booking> bookings = null;

        public Schedule()
        {
            if (bookings == null)
                bookings = new List<Booking>();
        }
        /* Adds a new booking to the schedule.
         * Returns true if and only if the specific booking is succsessfully added to Bookinglist.
         * Thorws exception, when booking already exists in bookings.
         */
        public bool addBooking(Booking booking)
        {
            foreach (Booking item in bookings)
            {
                if (item.id == booking.id)
                    return false;
            };

            int checkCount = bookings.Count();
            bookings.Add(booking);

            if (checkCount == bookings.Count())
            {
                return false;
            }

            return true;
        }

        /* Adds a new booking to the schedule.
         * Returns true if and only if the specific booking is succsessfully removed from the booking list.
         * Thorws exception, when booking does not exists in bookings.
         */
        public bool removeBooking(Booking booking)
        {
            foreach (Booking item in bookings)
            {
                if (item.id != booking.id)
                    return false;
            };

            int checkCount = bookings.Count();
            bookings.Remove(booking);

            if (checkCount == bookings.Count())
            {
                return false;
            }
            return true;
        }
        /* Removes all booking items from booking, when their endTimes
         * are in the past.
         * Returns true if and only if all bookings with a lower endTime, 
         * than the given DateTime, are succsessfully removed form the bookinglist.
         */
        public bool clean(DateTime now)
        {
            foreach (Booking item in bookings)
            {
                if (item.endTime < now)
                    try
                    {
                        removeBooking(item);
                    }
                    catch (Exception)
                    {
                        return false;
                    }
            };
            return true;
        }
    }
}
