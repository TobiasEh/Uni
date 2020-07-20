using Sopro.Interfaces;
using Sopro.ValidationAttributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Sopro.Models.Simulation
{
    public class Rushhour
    {
        public DateTime start { get; set; }
        [BookingEndTimeValidation]
        public DateTime end { get; set; }
        [Range(0,int.MaxValue)]
        public int bookings { get; set; }
        public IFunctionStrategy strategy { get; set; } = new NormalDistribution();

        public List<DateTime> run()
        {
            return strategy.generateDateTimeValues(start,end,bookings);
        }
    }
}
