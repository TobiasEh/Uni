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

        [BookingEndTimeValidation("start")]
        public DateTime end { get; set; }

        [Range(0,int.MaxValue)]
        public int bookings { get; set; }

        public IFunctionStrategy strategy { get; set; } = new NormalDistribution();

        public double spread { get; set; } =  1;

    }
}
