using Sopro.Interfaces.HistorySimulation;
using Sopro.Interfaces.Simulation;
using Sopro.Models.Administration;
using Sopro.Models.Infrastructure;
using System.Collections.Generic;
using System.Linq;

namespace Sopro.Models.Simulation
{
    public class ExecutedScenario : Scenario, IRunnable, IEvaluatable
    {
        private List<double> locationWorkload { get; set; }
        private List<List<double>> stationWorkload { get; set; }
        public int fulfilledRequests { private get; set; } = 0;
        public List<Booking> bookings { get; set; } = new List<Booking>();

        public ExecutedScenario()
        {

        }

        public List<double> getLocationWorkload()
        {
            return locationWorkload;
        }

        public List<List<double>> getStationWorkload()
        {
            return stationWorkload;
        }

        public int getFulfilledRequests()
        {
            return fulfilledRequests;
        }

        /* Upadtes workloads of location and station.
         */
        public bool updateWorkload(double location, List<double> station)
        {
            int count = locationWorkload.Count();
            locationWorkload.Add(location);
            if (count == locationWorkload.Count())
                return false;

            count = stationWorkload.Count();
            stationWorkload.Add(station);
            if (count == stationWorkload.Count())
                return false;
            return true;
        }

        public List<Booking> getBookings()
        {
            throw new System.NotImplementedException();
        }
    }
}
