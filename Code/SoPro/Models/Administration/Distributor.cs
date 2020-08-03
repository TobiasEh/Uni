using System.Collections.Generic;
using System.Linq;
using Sopro.Interfaces;
using System;

namespace Sopro.Models.Administration
{
    /// <summary>
    /// Klasse die zum Verteilen der Buchungen in die Schedule zuständig ist.
    /// </summary>
    public class Distributor
    {
        private Schedule schedule { get; set; }
        public IDistributionStrategy strategy { get; set; }
        private BookingLocationFilter filter { get; set; }
        private int buffer {get; set; } = 15;
        private NotificationManager notificationManager;
        private ILocation location;

        public Distributor()
        {
            strategy = new StandardDistribution();
        }

        /// <summary>
        /// Konstruktor der Klasse.
        /// </summary>
        /// <param name="_schedule">Planer Objekt in dem die Verteilten Buchungen gespeichert werden.</param>
        /// <param name="_location">Standort auf dem die Buchugen zugeteilt sind.</param>
        public Distributor(Schedule _schedule, ILocation _location)
        {
            schedule = _schedule;
            location = _location;
            filter = new BookingLocationFilter(location);
            notificationManager = new NotificationManager();
        }

        /// <summary>
        /// Methode ruft den passenden Algorithmus auf um die Verteilung zu starten.
        /// Der User wird per E-Mail darüber benachrichtigt.
        /// </summary>
        /// <param name="bookings">Liste der Buchungen die verteilt werden soll.</param>
        /// <returns></returns>
        public bool run(DateTime now, List<Booking> bookings)
        {
            Console.WriteLine("[Distributor.cs], Zeilen 46, 47, 55");
            Console.WriteLine("Buchungen vor Filter:\t" + bookings.Count.ToString());
            bookings = filter.filter(now, bookings);
            if ((bookings == null) || (bookings.Count() == 0))
            {
                if (bookings == null)
                    return false;
            }
            
            Console.WriteLine("Buchungen nach Filter:\t" + bookings.Count.ToString());
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
    }
}
