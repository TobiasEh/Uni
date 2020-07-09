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
        [Range(1,int.MaxValue)]
        public int capacity { get; set; }
        public Plug plugs { get; set; }

        [Required]
        [Range(0, 100)]
        public int socStart { get; set; }
        [Required]
        [VehicleSocEndValidation]
        public int socEnd { get; set; }
    }
}
