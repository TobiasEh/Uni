using Sopro.Interfaces.PersistenceController;
using Sopro.Persistence.PersLocation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sopro.Persistence.PersScenario
{
    public class ScenarioService : LocationRepository, ILocationService
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
