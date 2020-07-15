using Sopro.Interfaces.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sopro.Persistence.PersScenario
{
    public class ScenarioRepository : IScenarioRepository
    {
        public List<Scenario> import()
        {
            return new List<Scenario>();
        }
        public void emport(List<Scenario> list)
        {

        }
    }
}
