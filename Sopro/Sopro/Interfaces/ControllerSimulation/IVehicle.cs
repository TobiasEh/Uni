using Sopro.Models.Infrastructure;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Sopro.Interfaces.ControllerSimulation
{
    public interface IVehicle
    {
        public string id { get; set; }
        public string model { get; set; }
        public int capacity { get; set; }
        public int socStart { get; set; }
        public int socEnd { get; set; }
        public List<PlugType> plugs { get; set; }
    }
}
