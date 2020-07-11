using Sopro.Models.Simulation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sopro.Interfaces.ControllerSimulation
{
    public interface IScenario
    {
        bool addVehicle(Vehicle vehicle);
        bool deleteVehicle(Vehicle vehicle);
        bool addRushhour(Rushhour rushhour);
        bool deleteRushhour(Rushhour rushhour);
    }
}
