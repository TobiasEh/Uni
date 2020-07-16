using System.Collections.Generic;


namespace Sopro.Interfaces.Persistence
{
    public interface ILocationRepository
    {
        public List<ILocation> import();

        public void export(List<ILocation> list);
    }
}
