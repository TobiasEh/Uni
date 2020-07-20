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
        public IScenario scenario { get; set; }
        public List<ILocation> locations { get; set; }
        public List<IVehicle> vehicles { get; set; }
        public string idChosedLocation { get; set; }
        public List<DateTime> startRushours { get; set; } = new List<DateTime>() { new DateTime(0) };
        public List<DateTime> endRushours { get; set; } = new List<DateTime>() { new DateTime(0)};
        public List<int> bookingsRushours { get; set; } = new List<int>() { -1};

        public ScenarioCreateViewModel(IScenario _scenario, List<ILocation> _locations, List<IVehicle> _vehicles)
        {
            scenario = _scenario;
            locations = _locations;
            vehicles = _vehicles;
        }

        public ScenarioCreateViewModel() 
        {
            locations = new List<ILocation>();
            vehicles = new List<IVehicle>();
        }
    }
}
