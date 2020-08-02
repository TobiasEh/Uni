using Sopro.Interfaces;
using Sopro.Interfaces.ControllerSimulation;
using Sopro.Models.Infrastructure;
using Sopro.Models.Simulation;
using Sopro.ViewModels.ExportImportViewModel;
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
        public List<RushhourExportImportViewModel> rushhours { get; set; } = new List<RushhourExportImportViewModel>();
        public DateTime start { get; set; }
        public LocationExportImportViewModel location { get; set; }

        public ScenarioExportImportViewModel() { }

        public ScenarioExportImportViewModel(IScenario s)
        {
            id = s.id;
            duration = s.duration;
            bookingCountPerDay = s.bookingCountPerDay;
            vehicles = s.vehicles;
            foreach(Rushhour r in s.rushhours)
            {
                rushhours.Add(new RushhourExportImportViewModel(r));
            }
            start = s.start;
            location = new LocationExportImportViewModel(s.location);
            
        }

        public IScenario generateScenario()
        {
            IScenario s = new Scenario();
            s.id = id;
            s.duration = duration;
            s.bookingCountPerDay = bookingCountPerDay;
            s.vehicles = vehicles;
            s.rushhours = new List<Rushhour>();
            foreach(RushhourExportImportViewModel r in rushhours)
            {
                s.rushhours.Add(r.generateRushhour());
            }
            s.start = start;
            s.location = location.generateLocation();

            return s;
        }
    }

}
