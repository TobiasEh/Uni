using System;
using System.Collections.Generic;
using Sopro.Interfaces;

namespace Sopro.Models.Administration
{
    /// <summary>
    /// Klasse kommt beim filtern der Buchungen eines bestimmten Standortes zum Einsatz.
    /// Dabei werden Buchungen, eines Standorts, die weiter als timespan Tage in der Zukunft liegen herausgefiltert.
    /// </summary>
    public class BookingLocationFilter
    {
        public ILocation location { get; set; }
        public int timespan { get; set; } = 30;
        public DateTime startTime { get; set; }

        /// <summary>
        /// Konstruktor
        /// </summary>
        /// <param name="_location">Nach dem zu filternden Standort.</param>
        /// <param name="_timespan">Zeitspanne in der die Buchungen liegen düfen.</param>
        public BookingLocationFilter(ILocation _location, int _timespan)
        {
            location = _location;
            timespan = _timespan;
        }
        /// <summary>
        /// Konstruktor des Buchungsfilters.
        /// </summary>
        /// <param name="_location">Nach dem zu filternden Standort.</param>
        public BookingLocationFilter(ILocation _location)
        {
            location = _location;
        }
        /// <summary>
        /// Konstruktor
        /// </summary>
        /// <param name="start">Nach der zu filterned Startzeit.</param>
        public BookingLocationFilter(DateTime start)
        {
            startTime = start;
        }

        /// <summary>
        /// Filtert die Liste der übergebenen Buchungen nach Standort, Startzeit und Zeitspanne.
        /// </summary>
        /// <param name="bookings">Liste der Buchungen die gefiltert werden soll.</param>
        /// <param name="now">Abwelchen Zeitpunkt die Filterkriterien gelten.</param>
        /// <returns>
        /// List die nur noch die Buchungen enthält, die zum ausgewählten Standortgehören und im Intervall [jetzt, jetzt + timespan) liegen.
        /// </returns>
        public List<Booking> filter(DateTime now, List<Booking> bookings)
        {
            DateTime time = now.Add(new TimeSpan(timespan, 0, 0, 0));

            List<Booking> result = new List<Booking>();
            foreach (Booking item in bookings)
            {
                // Console.WriteLine("BookingLocationFilter.cs Condition 1: " + (item.location == location).ToString());
                // Console.WriteLine("BookingLocationFilter.cs Condition 2: " + (item.startTime < time).ToString());
                // Console.WriteLine("BookingLocationFilter.cs Condition 3: " + (item.startTime >= DateTime.Now).ToString());
                // Console.WriteLine(time.ToString());
                // Console.WriteLine(item.startTime.ToString());
                if ((item.location == location) && (item.startTime < time) && (item.startTime >= DateTime.Now))
                {
                    result.Add(item);
                }
            }
            return result;
        }
    }
}
