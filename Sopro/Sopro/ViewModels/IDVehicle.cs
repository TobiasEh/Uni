using Sopro.Interfaces.ControllerSimulation;
using Sopro.Models.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sopro.ViewModels
{
    public class IDVehicle : IVehicle
    {
        public string model { get ; set ; }
        public int capacity { get; set; }
        public int socStart { get; set; }
        public int socEnd { get; set; }
        public List<PlugType> plugs { get; set; }
        public int id { get; set; }
    }
}
