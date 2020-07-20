﻿using Sopro.Models.Communication;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Sopro.Models.Administration
{
    public class Schedule
    {
        public NotificationManager notificationManager { get; set; }

        public List<Booking> bookings { get; set; }

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
            // notificationManager.notify(booking, NotificationEvent.ACCEPTED);

            return true;
        }

        /* Removes a booking from the schedule.
         * Returns true if and only if the specific booking is succsessfully removed from the booking list.
         * Thorws exception, when booking does not exists in bookings.
         */
        public bool removeBooking(Booking booking)
        {
            if (!bookings.Contains(booking))
                return false;

            int checkCount = bookings.Count();
            bookings.Remove(booking);

            if (checkCount-1 == bookings.Count())
            {
                return true;                
            }
            return false;
        }
        /* Removes all booking items from booking, when their endTimes
         * are in the past.
         * Returns true if and only if all bookings with a lower endTime, 
         * than the given DateTime, are succsessfully removed form the bookinglist.
         */
        public bool clean(DateTime now)
        {
            
            bool flag = false;
            var bookingC = new List<Booking>();
            foreach (Booking item in bookings)
            {
                if (item.endTime < now)
                {
                    bookingC.Add(item);
                }
            };
            if(bookingC.Count != 0) bookingC.ForEach(x => {
                if (bookings.Contains(x))
                {
                    removeBooking(x);
                }
                else
                {
                    flag = true;
                }
                });
            return bookingC.Count == 0 ? false : !flag;
        }

        /* Sets active attribute of booking to the opposite boolean and notifys user.
         */
        public void toggleCheck(Booking booking)
        {
            int index = bookings.IndexOf(booking);
            bookings[index].active = !bookings[index].active;

            /*
            if (bookings[index].active)
                notificationManager.notify(booking, NotificationEvent.CHECKIN);

            else
                notificationManager.notify(booking, NotificationEvent.CHECKOUT);
            */
        }
    }
}
