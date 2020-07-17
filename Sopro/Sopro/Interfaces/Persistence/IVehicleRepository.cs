using Sopro.Interfaces.ControllerSimulation;
using System.Collections.Generic;

namespace Sopro.Interfaces.Persistence
{
    public interface IVehicleRepository
    {
        public List<IVehicle> import(string path);
        public void export(List<IVehicle> list, string path);
    }
}
