﻿using Microsoft.AspNetCore.Server.IIS.Core;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sopro.Models.Administration
{
    public class Schedule
    {
        private NotificationManager notificationManager { get; set; }

        private List<Booking> bookings { get; set; } = null;

        public Schedule()
        {
            notificationManager = new NotificationManager();
            if (bookings == null)
                bookings = new List<Booking>();
        }
        /* Adds a new booking to the schedule.
         * Returns true if and only if the specific booking is succsessfully added to Bookinglist.
         * Thorws exception, when booking already exists in bookings.
         */
        public bool addBooking(Booking booking)
        {
            if (bookings.Contains(booking))
                return false;

            int checkCount = bookings.Count();
            bookings.Add(booking);

            if (checkCount == bookings.Count())
            {
                return false;
            }
            notificationManager.notify(booking, NotificationEvent.ACCEPTED);

            return true;
        }

        /* Removes a new booking to the schedule.
         * Returns true if and only if the specific booking is succsessfully removed from the booking list.
         * Thorws exception, when booking does not exists in bookings.
         */
        public bool removeBooking(Booking booking)
        {
            if (!bookings.Contains(booking))
                return false;

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
                {
                    if (!removeBooking(item))
                        return false;
                }
            };
            return true;
        }
    }
}
