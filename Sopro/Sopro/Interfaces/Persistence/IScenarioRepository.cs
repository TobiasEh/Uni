using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sopro.Interfaces.Persistence
{
    public interface IScenarioRepository
    {
        public List<IScenario> import();

        public void export(List<IScenario> list, string path);
    }
}
