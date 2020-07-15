using Sopro.Interfaces.PersistenceController;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sopro.Models.Infrastructure;

namespace Sopro.Persistence.PersLocation
{
    public class LocationService : LocationRepository, ILocationService
    {
        public List<Location> import()
        {
            return new List<Location>();
        }

        public void emport(List<Location> list)
        {

        }
    }
}
