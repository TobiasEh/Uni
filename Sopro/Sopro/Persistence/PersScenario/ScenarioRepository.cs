using Sopro.Interfaces.ControllerSimulation;
using Sopro.Interfaces.Persistence;
using System.Collections.Generic;


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
