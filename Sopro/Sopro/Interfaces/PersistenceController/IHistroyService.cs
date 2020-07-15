using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sopro.Interfaces.Persistence;

namespace Sopro.Interfaces.PersistenceController
{
    public interface IHistroyService : IHistroyRepository
    {
        public List<History> import();

        public void export(List<History> list);
    }
}
