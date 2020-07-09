using Sopro.ValidationAttributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Foolproof;
using System.ComponentModel.Design;
using Sopro.Models.Infrastructure;
using Foolproof;

namespace Sopro.Models.Administration
{
    public class Booking
    {
        [Required]
        [Range(1, Int32.MaxValue)]
        public int capacity { get; set; }
        [Required]
        [BookingPlugsValidation]
        public List<PlugType> plugs { get; set; }
        [Required]
        [Range(0, 100)]
        public int socStart { get; set; }
        [Required]
        [BookingSocEndValidation]
        public int socEnd { get; set; }
        [Required]
        public string user { get; set; }
        [Required]
        [BookingStartTimeValidation]
        public DateTime startTime { get; set; }
        [Required]
        [BookingEndTimeValidation]
        public DateTime endTime { get; set; }
        public Station station { get; set; }
        [Required]
        public bool active { get; set; }

        [Required]
        public Location location { get; set; }
        public UserType priority { get; set; }

    }
}
