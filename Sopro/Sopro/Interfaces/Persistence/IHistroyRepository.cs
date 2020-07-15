using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sopro.Interfaces.Persistence
{
    public interface IHistroyRepository
    {
        public List<History> import();
        public void export(List<History> list);
    }
}
