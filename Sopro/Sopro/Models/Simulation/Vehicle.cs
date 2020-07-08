using Foolproof;
using Sopro.ValidationAttributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Sopro.Models.Infrastructure;

namespace Sopro.Models.Simulation
{
    public class Vehicle
    {
        [Required]
        public string model { get; set; }
        [Required]
        [BookingCapacityValidation]
        public int capacity { get; set; }
        [Required]
        [BookingPlugsValidation]
        public List<PlugType> plugs { get; set; }

        //public PlugType[] plugs { get; set; }
        [Required]
        [BookingSocStartValidation]
        public int socStart { get; set; }
        [Required]
        [BookingSocEndValidation]
        [GreaterThan("socStart")]
        public int socEnd { get; set; }
    }
}
