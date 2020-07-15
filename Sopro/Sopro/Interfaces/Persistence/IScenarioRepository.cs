using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sopro.Interfaces.Persistence
{
    public interface IScenarioRepository
    {
        public List<Scenario> import();

        public void emport(List<Scenario> list);
    }
}
