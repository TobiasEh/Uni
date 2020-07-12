using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sopro.Interfaces.Simulation
{
    public interface IRunnable
    {
        bool updateWorkload(double location, List<double> station);
    }
}
