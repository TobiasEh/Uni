using Sopro.Interfaces.Persistence;
using Sopro.Models.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sopro.Interfaces.PersistenceController
{
    public interface ILocationService : ILocationRepository
    {
        public List<ILocation> import();

        public void export(List<ILocation> list);
    }
}
