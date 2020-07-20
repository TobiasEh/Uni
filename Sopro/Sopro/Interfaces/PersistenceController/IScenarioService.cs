using Sopro.Interfaces.ControllerSimulation;
using System.Collections.Generic;

namespace Sopro.Interfaces.PersistenceController
{
    public interface IScenarioService
    {
        public List<IScenario> import(string path);
        public void export(List<IScenario> list, string path);
    }
}
