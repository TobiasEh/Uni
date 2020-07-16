using System.Collections.Generic;


namespace Sopro.Interfaces.HistorySimulation
{
    public interface IEvaluatable
    {
        List<double> getLocationWorkload();
        List<List<double>> getStationWorkload();
        int getFulfilledRequests();
    }
}

