using Microsoft.CodeAnalysis;
using Sopro.Interfaces;
using Sopro.Interfaces.ControllerSimulation;
using Sopro.Models.Simulation;
using Sopro.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sopro.ViewModels
{
    public class ScenarioCreateViewModel
    {
        public Scenario scenario { get; set; }
        public List<ILocation> locations { get; set; }
        public List<Vehicle> vehicles { get; set; }
        public List<Rushhour> rushhours { get; set; }
        public ILocation location { get; set; }
        public int tickLength { get; set; }
        public List<int> vcount{get;set;}
        public ScenarioLocationViewModel sclocation { get; set; }
        public ScenarioCreateZoneViewModel sczone { get; set; }


        public ScenarioCreateViewModel(List<ILocation> _locations, List<Vehicle> _vehicles)
        {
            sclocation = new Sopro.ViewModels.ScenarioLocationViewModel();
            sclocation.location = new Sopro.Models.Infrastructure.Location();
            sclocation.location.name = "Neu";
            sclocation.location.emergency = 0;
            sclocation.location.zones = new List<Models.Infrastructure.Zone>();

            locations = _locations;
            vehicles = _vehicles;
            vcount = new List<int>(vehicles.Count);
            scenario = new Scenario();
            rushhours = new List<Rushhour>();
        }

        public ScenarioCreateViewModel() 
        {
            sclocation = new Sopro.ViewModels.ScenarioLocationViewModel();
            sclocation.location = new Sopro.Models.Infrastructure.Location();
            sclocation.location.name = "Neu";
            sclocation.location.emergency = 0;
            sclocation.location.zones = new List<Models.Infrastructure.Zone>();

            locations = new List<ILocation>();
            vehicles = new List<Vehicle>();
            rushhours = new List<Rushhour>();
        }
    }
}
