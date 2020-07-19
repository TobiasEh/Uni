using Microsoft.CodeAnalysis;
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
        public Scenario scenario { get; set; }
        public List<ILocation> locations { get; set; }
        public List<Vehicle> vehicles { get; set; }
        public List<Rushhour> rushhours { get; set; }
        public int tickLength { get; set; }

        public ScenarioCreateViewModel(List<ILocation> _locations, List<Vehicle> _vehicles)
        {
            locations = _locations;
            vehicles = _vehicles;
            scenario = new Scenario();
            rushhours = new List<Rushhour>();
        }

        public ScenarioCreateViewModel() 
        {
            locations = new List<ILocation>();
            vehicles = new List<Vehicle>();
            rushhours = new List<Rushhour>();
        }
    }
}
