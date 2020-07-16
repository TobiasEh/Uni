using Sopro.Interfaces.Persistence;
using System.Collections.Generic;
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
