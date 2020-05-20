using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Aufgabe_4.Models
{
    public class Booking
    {
        [Range(0, 100)]
        [Required]
        public double currentCharge { get; set; }

        [Range(0, 1000)]
        [Required]
        public int requiredDistance { get; set; }

        [DataType(DataType.DateTime)]
        [Required]
        public DateTime start { get; set; }

        [DataType(DataType.DateTime)]
        [Required]
        public DateTime end { get; set; }
    }
}
