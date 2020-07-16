using Sopro.Interfaces.ControllerSimulation;
using Sopro.Interfaces.Persistence;
using System.Collections.Generic;

namespace Sopro.Interfaces.PersistenceController
{
    public interface IScenarioService : IScenarioRepository
    {
        public List<IScenario> import(string path);

        public void export(List<IScenario> list, string path);
    }
}
