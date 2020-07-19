using Org.BouncyCastle.Asn1.Mozilla;
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
        public List<Booking> bookings { get; set; }
        public readonly List<Booking> generatedBookings;

        public ExecutedScenario(Scenario scenario)
        {
            bookings = new List<Booking>();
            stationWorkload = new List<List<double>>();
            locationWorkload = new List<double>();

            if (scenario != null)
            {
                duration = scenario.duration;
                bookingCountPerDay = scenario.bookingCountPerDay;
                vehicles = scenario.vehicles;
                rushhours = scenario.rushhours;
                start = scenario.start;
                location = scenario.location;
            }
            generatedBookings = Generator.generateBookings(this);
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

        /* Updates workloads of location and station.
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
            return bookings;
        }

        public void clear()
        {
            locationWorkload = new List<double>();
            stationWorkload = new List<List<double>>();
            fulfilledRequests = 0;
            bookings = new List<Booking>();
            bookings.AddRange(generatedBookings);
        }
    }
}
