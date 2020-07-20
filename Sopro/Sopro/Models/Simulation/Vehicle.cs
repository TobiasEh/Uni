using Sopro.ValidationAttributes;
using System;
using System.ComponentModel.DataAnnotations;
using Sopro.Models.Infrastructure;
using Sopro.Interfaces.ControllerSimulation;
using System.Collections.Generic;

namespace Sopro.Models.Simulation
{
    public class Vehicle : IVehicle
    {
        public string id { get; set; }
        [Required]
        public string model { get; set; }
        [Required]
        [Range(1, int.MaxValue)]
        public int capacity { get; set; }

        [Required]
        [Range(0, 100)]
        public int socStart { get; set; }
        [Required]
        [VehicleSocEndValidation]
        public int socEnd { get; set; }
        [Required]
        [EnumLength(1, typeof(PlugType))]
        public List<PlugType> plugs { get; set; } = new List<PlugType>();

        public Vehicle()
        {
            id = Guid.NewGuid().ToString();
        }
    }
}
