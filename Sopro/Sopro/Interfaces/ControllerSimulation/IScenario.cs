using Sopro.Models.Simulation;
using System;
using System.Collections.Generic;

namespace Sopro.Interfaces.ControllerSimulation
{
    public interface IScenario
    {

        public int duration { get; set; }
        public int bookingCountPerDay { get; set; }
        public List<Vehicle> vehicles { get; set; }
        public List<Rushhour> rushhours { get; set; }
        public DateTime start { get; set; }
        public ILocation location { get; set; }

        public bool addVehicle(Vehicle vehicle);
        public bool deleteVehicle(Vehicle vehicle);
        public bool addRushhour(Rushhour rushhour);
        public bool deleteRushhour(Rushhour rushhour);

    }
}
