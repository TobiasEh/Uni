using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.ComponentModel;

namespace Website.Models
{
    public class Booking
    {
        [Required]
        [Range(0, 100)]
        public double currentCharge { get; set; }
        
        [Required]
        [Range(0, 1000)]
        public int requiredDistance { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        [DateValidater()]
        public DateTime start { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        [DateValidater("start")]
        public DateTime end { get; set; }

        public Plug plugType { get; set; }

    }
}
