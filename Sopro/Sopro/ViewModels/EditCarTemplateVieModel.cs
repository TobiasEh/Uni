using Sopro.Interfaces.ControllerSimulation;
using Sopro.Models.Infrastructure;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Sopro.ViewModels
{
    public class EditCarTemplateVieModel : IVehicle
    {
        public string model { get; set; }
        public int capacity { get; set; }
        public int socStart { get; set; }
        public int socEnd { get; set; }
        public List<PlugType> plugs { get; set; }
        public int index { get; set; }
    }
}
