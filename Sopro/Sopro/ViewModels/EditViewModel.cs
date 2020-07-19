using Sopro.Interfaces.ControllerSimulation;
using Sopro.Models.Simulation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sopro.ViewModels
{
    public class EditViewModel
    {
        public Vehicle vehicle { get; set; }
        public bool CCS { get; set; }
        public bool TYPE2 { get; set; }

    }
}
