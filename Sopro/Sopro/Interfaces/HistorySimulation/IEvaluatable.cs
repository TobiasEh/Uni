using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sopro.Interfaces.HistorySimulation
{
    public interface IEvaluatable
    {
        List<double> getLocationWorkload();
        List<List<double>> getStationWorkload();
        int getFulfilledRequests();
    }
}

