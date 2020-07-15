using Sopro.Interfaces.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sopro.Persistence.PersScenario
{
    public class ScenarioRepository : IScenarioRepository
    {
        public List<IScenario> import()
        {
            return new List<IScenario>();
        }
        public void export(List<IScenario> list, string path)
        {

        }
    }
}
