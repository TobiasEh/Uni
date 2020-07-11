using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sopro.Interfaces.HistorySimulation
{
    public class IEvaluatable
    {
        List<double> getLocationWorkload();
        double[][] getStationWorkload();
        int getFulfilledRequests();
    }
}

