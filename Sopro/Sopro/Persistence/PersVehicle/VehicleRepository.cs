using Sopro.Interfaces.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
