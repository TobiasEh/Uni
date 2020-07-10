using Sopro.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sopro.Models.Simulation
{
    public class Rushhour
    {
        public DateTime start { get; set; }
        public DateTime end { get; set; }
        public int bookings { get; set; }
        public IFunctionStrategy strategy { get; set; }

        public List<DateTime> run()
        {

            return strategy.generateDateTimeValues(start,end,bookings);
        }
    }
}
