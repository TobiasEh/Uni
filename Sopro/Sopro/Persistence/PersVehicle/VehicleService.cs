using Sopro.Interfaces.PersistenceController;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sopro.Persistence.PersVehicle
{
    public class VehicleService : VehicleRepository, IVehicleService
    {
        public List<Vehicle> import()
        {
            return new List<Vehicle>();
        }

        public void emport(List<Vehicle> list)
        {

        }

    }
}
