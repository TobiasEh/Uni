using Sopro.Interfaces.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sopro.Interfaces.PersistenceController
{
    public interface IVehicleService : IVehicleRepository
    {
        public List<Vehicle> import();
        public void emport(List<Vehicle> list);
    }
}
