using Sopro.Interfaces.Persistence;
using System.Collections.Generic;

namespace Sopro.Interfaces.PersistenceController
{
    public interface ILocationService : ILocationRepository
    {
        public List<ILocation> import();

        public void export(List<ILocation> list);
    }
}
