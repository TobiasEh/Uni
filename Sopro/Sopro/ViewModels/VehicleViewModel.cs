using Sopro.Interfaces.ControllerSimulation;
using Sopro.Models.Infrastructure;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Sopro.ViewModels
{
    public class VehicleViewModel
    {
        public List<IDVehicle> vehicles { get; set; }
        public IDVehicle vehicle { get; set; }
        
    }
}
