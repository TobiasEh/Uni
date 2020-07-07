using Sopro.ValidationAttributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using FoolProof.Core;
using System.ComponentModel.Design;
using Sopro.Models.Infrastructure;

namespace Sopro.Models.Administration
{
    public class Booking
    {
        [Required]
        public string id { get; set; }
        [Required]
        [BookingCapacityValidation]
        public int capacity { get; set; }
        [Required]
        [BookingPlugsValidation]
        public PlugType[] plugs { get; set; }
        [Required]
        [BookingSocStartValidation]
        public int socStart { get; set; }
        [Required]
        [BookingSocEndValidation]
        [GreaterThan("socStart")]
        public int socEnd { get; set; }
        [Required]
        public string user { get; set; }
        [Required]
        public DateTime startTime { get; set; }
        [Required]
        [GreaterThan("startTime")]
        public DateTime endTime { get; set; }
        [Required]
        public Station station { get; set; }
        [Required]
        public bool active
        {
            get { return active; }
            set { active = false; }
        }

    }
}
