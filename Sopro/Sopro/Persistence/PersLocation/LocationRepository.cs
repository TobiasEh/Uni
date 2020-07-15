using Sopro.Interfaces.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sopro.Models.Infrastructure;
using Sopro.Interfaces;

namespace Sopro.Persistence.PersLocation
{
    public class LocationRepository : ILocationRepository
    {
        public List<ILocation> import()
        {
            return new List<ILocation>();
        }

        public void export(List<ILocation> list)
        {

        }
    }
}
