using Sopro.Interfaces;
using Sopro.ValidationAttributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Sopro.Models.Simulation
{
    /// <summary>
    /// Klasse welche die Struktur einer Stoßzeit beschreibt.
    /// </summary>
    public class Rushhour
    {
        public DateTime start { get; set; }
        [BookingEndTimeValidation]
        public DateTime end { get; set; }
        [Range(0,int.MaxValue)]
        public int bookings { get; set; }
        public IFunctionStrategy strategy { get; set; } = new NormalDistribution();

        /// <summary>
        /// Generiert die Verteilung der Buchungen in der Stoßzeit.
        /// </summary>
        /// <returns>Die Liste an DateTimes für die Buchungen in der Stoßzeit.</returns>
        public List<DateTime> run()
        {
            return strategy.generateDateTimeValues(start,end,bookings);
        }
    }
}
