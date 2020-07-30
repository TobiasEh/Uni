using Sopro.Interfaces;
using Sopro.Interfaces.ControllerSimulation;
using Sopro.Models.Infrastructure;
using Sopro.Models.Simulation;
using System;
using System.Collections.Generic;

namespace Sopro.ViewModels
{
    public class ScenarioExportImportViewModel
    {
        public string id { get; set; }
        public int duration { get; set; }
        public int bookingCountPerDay { get; set; }
        public List<Vehicle> vehicles { get; set; }
        public List<Rushhour> rushhours { get; set; }
        public DateTime start { get; set; }
        public ILocation location { get; set; }

        public ScenarioExportImportViewModel() { }

        public ScenarioExportImportViewModel(IScenario s)
        {
            id = s.id;
            duration = s.duration;
            bookingCountPerDay = s.bookingCountPerDay;
            vehicles = s.vehicles;
            rushhours = s.rushhours;
            start = s.start;
            location = s.location;
        }

        public IScenario generateScenario()
        {
            IScenario s = new Scenario();
            id = s.id;
            duration = s.duration;
            bookingCountPerDay = s.bookingCountPerDay;
            vehicles = s.vehicles;
            rushhours = s.rushhours;
            start = s.start;
            location = s.location;

            return s;
        }
    }

}
