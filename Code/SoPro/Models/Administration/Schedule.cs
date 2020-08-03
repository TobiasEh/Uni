using Sopro.Models.Communication;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Sopro.Models.Administration
{
    /// <summary>
    /// Klasse die die verteilten Buchugen verwaltet.
    /// </summary>
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

        /// <summary>
        /// Fügt eine neue Buchung der Buchungsliste hinzu.
        /// Benachrichtigt Benutzer, dass seine Buchung akzeptiert und erfolgreich verteilt wurde.
        /// </summary>
        /// <param name="booking">Buchung, die zur Buchungsliste hinzugefügt werden soll.</param>
        /// <returns>Gibt Wahrheitswert zurück, ob das Hinzufügen erfolgreich war.</returns>
        public bool addBooking(Booking booking)
        {
            if (bookings.Contains(booking))
                return false;

            int checkCount = bookings.Count();
            bookings.Add(booking);

            if (checkCount == bookings.Count())
                return false;

            Messenger.newMessage(booking, NotificationEvent.ACCEPTED, booking.user);

            return true;
        }

        /// <summary>
        /// Entfernt Buchung aus der Buchungsliste.
        /// </summary>
        /// <param name="booking">Buchung, die aus der Buchungsiste entfernt werden soll.</param>
        /// <returns>Gibt Wahrheitswert zurück, ob das Entfernen erfolgreich war.</returns>
        public bool removeBooking(Booking booking)
        {
            if (!bookings.Contains(booking))
                return false;

            int checkCount = bookings.Count();
            bookings.Remove(booking);

            if (checkCount-1 == bookings.Count())
                return true;                

            return false;
        }
        /// <summary>
        /// Entfernt alle Buchungen aus der Liste, wenn deren Endzeit in der Vergangenheit liegt.
        /// </summary>
        /// <param name="now">Zeit, die verglichen wird.</param>
        /// <returns>Gibt Wahrheitswert zurück, ob alle Buchungen erfolgreich entfernt wurden, deren Endzeit in der Vergangenheit liegt.</returns>
        public bool clean(DateTime now)
        {
            
            bool flag = false;
            var bookingC = new List<Booking>();
            foreach (Booking item in bookings)
            {
                if (item.endTime < now)
                    bookingC.Add(item);
            };

            if(bookingC.Count != 0) bookingC.ForEach(x => {
                if (bookings.Contains(x))
                    removeBooking(x);
                else
                    flag = true;
                });

            return bookingC.Count == 0 ? false : !flag;
        }

        /// <summary>
        /// Ändert den Status der Buchung, auf den entgegengesetzen Wahrheitswert.
        /// User wird Benachrichtigt, dass er Erfogreich ein bzw. aus gecheckt ist.
        /// </summary>
        /// <param name="booking">Buchung, deren Status geändert wird.</param>
        public void toggleCheck(Booking booking)
        {
            int index = bookings.IndexOf(booking);
            bookings[index].active = !bookings[index].active;


            if (bookings[index].active)
                Messenger.newMessage(booking, NotificationEvent.CHECKIN, booking.user);

            else
                Messenger.newMessage(booking, NotificationEvent.CHECKIN, booking.user);
        }
    }
}
