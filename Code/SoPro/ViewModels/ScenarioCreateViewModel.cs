using Sopro.Interfaces;
using Sopro.Interfaces.ControllerSimulation;
using Sopro.Models.Simulation;
using System.Collections.Generic;

namespace Sopro.ViewModels
{
    public class ScenarioCreateViewModel
    {

        public Scenario scenario { get; set; } = new Scenario();
        public List<ILocation> locations { get; set; } 
        public List<IVehicle> vehicles { get; set; } = new List<IVehicle>();
        public int countRushhours { get; set; } = 0;
        public List<int> countVehicles { get; set; } = new List<int>();
        public string idLocation { get; set; }
        public List<Rushhour> rushhours { get; } = new List<Rushhour>();
        public string id { get; set; }

        public void setVehicles(List<IVehicle> _vehicles)
        {
            vehicles = _vehicles;
            foreach(Vehicle v in vehicles)
            {
                countVehicles.Add(0);
            }
        }

        /*
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
        }*/
    }
}
