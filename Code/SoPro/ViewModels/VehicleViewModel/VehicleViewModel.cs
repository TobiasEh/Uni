using Sopro.Interfaces.ControllerSimulation;
using Sopro.Models.Simulation;
using Sopro.ValidationAttributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sopro.ViewModels
{
    public class VehicleViewModel
    {
        public List<IVehicle> vehicles { get; set; }
        public Vehicle vehicle { get; set; }
        [AtleastOnePlug("CCS","TYPE2", ErrorMessage ="Mindestens 1 Plug!")]
        public bool CCS { get; set; }
        [AtleastOnePlug("CCS", "TYPE2", ErrorMessage = "Mindestens 1 Plug!")]
        public bool TYPE2 { get; set; }
    }
}
