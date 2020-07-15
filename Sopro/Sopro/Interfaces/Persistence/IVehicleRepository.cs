using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sopro.Interfaces.Persistence
{
    public interface IVehicleRepository
    {
        public List<Vehicle> import();
        public void emport(List<Vehicle> list);
    }
}
