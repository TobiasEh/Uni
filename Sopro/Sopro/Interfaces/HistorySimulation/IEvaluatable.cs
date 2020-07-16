using Sopro.Models.Administration;
using Sopro.Models.Infrastructure;
using System.Collections.Generic;

namespace Sopro.Interfaces.HistorySimulation
{
    public interface IEvaluatable
    {
        public List<double> getLocationWorkload();
        public List<List<double>> getStationWorkload();
        public List<Booking> getBookings();
        public int getFulfilledRequests();
        public ILocation location { get; set; }
    }
}

