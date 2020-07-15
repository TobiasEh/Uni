using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sopro.Models.Infrastructure;

namespace Sopro.Interfaces.Persistence
{
    public interface ILocationRepository
    {
        public List<ILocation> import();

        public void export(List<ILocation> list);
    }
}
