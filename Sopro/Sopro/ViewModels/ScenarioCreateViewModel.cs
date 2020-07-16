using Sopro.Interfaces;
using Sopro.Interfaces.ControllerSimulation;
using Sopro.Models.Simulation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sopro.ViewModels
{
    public class ScenarioCreateViewModel
    {
        private IScenario scenario;
        private List<ILocation> locations;
        private List<IVehicle> vehicles;

        public ScenarioCreateViewModel(IScenario _scenario, List<ILocation> _locations, List<IVehicle> _vehicles)
        {
            scenario = _scenario;
            locations = _locations;
            vehicles = _vehicles;
        }
    }
}
