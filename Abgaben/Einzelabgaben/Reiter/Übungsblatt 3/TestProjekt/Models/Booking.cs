using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestProjekt.Models
{
    public class Booking
    {
        public double currentCharge { get; set; }
        public int requiredDistance { get; set; }
        public DateTime start { get; set; }
        public DateTime end { get; set; }
    }
}
