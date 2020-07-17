using Sopro.Interfaces.ControllerSimulation;
using Sopro.Interfaces.Persistence;
using System.Collections.Generic;


namespace Sopro.Interfaces.PersistenceController
{
    public interface IVehicleService : IVehicleRepository
    {
        public List<IVehicle> import(string path);
        public void export(List<IVehicle> list, string path);
    }
}
