using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace sopro_sose_2020.Models
{
    public class Booking
    {
        public double cur_charge { get; set; }
        public int needed_distance { get; set; }
        public DateTime startTime { get; set; }
        public DateTime endTime { get; set; }
    }
}
