using Sopro.Interfaces.ControllerSimulation;
using System.Collections.Generic;

namespace Sopro.Interfaces.Persistence
{
    public interface IScenarioRepository
    {
        public List<IScenario> import();

        public void export(List<IScenario> list, string path);
    }
}
