using Sopro.Models.Administration;
using Sopro.Models.Infrastructure;
using System.Collections.Generic;

namespace Sopro.Models.History
{
    public interface IEvaluatable
    {
        public List<double> getLocationWorkload();
        public List<List<double>> getStationWorkload();
        public List<Booking> getBookings();
        public int getFulfilledRequests();
        public Location location { get; set; }
    }
}