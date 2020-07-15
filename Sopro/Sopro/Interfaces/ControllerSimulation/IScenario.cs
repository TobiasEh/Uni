using Sopro.Models.Simulation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sopro.Interfaces.ControllerSimulation
{
    public interface IScenario
    {
        public bool addVehicle(Vehicle vehicle);
        public bool deleteVehicle(Vehicle vehicle);
        public bool addRushhour(Rushhour rushhour);
        public bool deleteRushhour(Rushhour rushhour);
        ILocation location { get; set; }
        public List<Vehicle> vehicles { get; set; }

    }
}
