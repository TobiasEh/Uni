using Sopro.Interfaces.ControllerSimulation;
using Sopro.Models.Infrastructure;
using Sopro.ValidationAttributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Sopro.ViewModels
{
    public class ScenarioVehicleViewModel : IVehicle
    {
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
        public List<PlugType> plugs { get; set; }
        public int count { get; set; }
    }
}
