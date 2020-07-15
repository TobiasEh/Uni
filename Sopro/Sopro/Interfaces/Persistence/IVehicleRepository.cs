using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sopro.Interfaces.Persistence
{
    public interface IVehicleRepository
    {
        public List<IVehicle> import(string path);
        public void export(List<IVehicle> list, string path);
    }
}
