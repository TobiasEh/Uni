using Sopro.Interfaces.ControllerSimulation;
using Sopro.Interfaces.Persistence;
using System.Collections.Generic;

namespace Sopro.Persistence.PersVehicle
{
    public class VehicleRepository : IVehicleRepository
    {
        public List<IVehicle> import(string path)
        {
            return new List<IVehicle>();
        }

        public void export(List<IVehicle> list, string path)
        {

        }
    }
}
