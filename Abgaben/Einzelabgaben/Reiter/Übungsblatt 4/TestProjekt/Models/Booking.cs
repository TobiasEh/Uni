using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TestProjekt.Models
{
    public class Booking
    {
        [Range(0, 100)]
        [Required]
        public double currentCharge { get; set; }

        [Range(1, 1000)]
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
