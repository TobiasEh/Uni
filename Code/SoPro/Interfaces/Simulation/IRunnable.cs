using System.Collections.Generic;

namespace Sopro.Interfaces.Simulation
{
    public interface IRunnable
    {
        bool updateWorkload(double location, List<double> station);
    }
}
