using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sopro.Interfaces;
using Sopro.Models.Administration;
using Sopro.Interfaces;
using System.Reflection.Emit;
using Sopro.Models.Infrastructure;

namespace Sopro.Models.Simulation
{
    public class Simulation : ITrigger
    {
        private TimeSpan tickLength { get; set; } = new TimeSpan(1, 0, 0, 0, 0);
        private int tickCount { get; set; } = -1;
        private List<Booking> pendingBookings { get; set; }
        private IGenerator generator;
        private ExecutedScenario exScenario;
        private Distributor distributor;

        public bool triggerBookingDistribution()
        {
            DateTime start = tickCount * tickLength + exScenario.start;
            DateTime end = start.AddDays(30);
            List<Booking> toBeDistributed = new List<Booking>();
            //TODO
            foreach(Booking item in pendingBookings)
            {
                //if (startTime the next day) || 
                if((item.startTime >= start && item.startTime <= start.AddDays(1)))
                {
                    toBeDistributed.Add(item);
                    pendingBookings.Remove(item);
                }
            }

            if (!exScenario.location.distributor.run(toBeDistributed))
                return false;

            return true;
        }

        public bool run()
        {
            List<Booking> tempBookings;
            tempBookings = generator.generateBookings(exScenario);
            tempBookings.Sort((x, y)=> (x.startTime.CompareTo(y.startTime)));

            pendingBookings = tempBookings;
            exScenario.bookings = tempBookings;
            int count = tempBookings.Count();

            if (!triggerBookingDistribution())
                return false;

            ++tickCount;
            while(tickCount <= exScenario.duration)
            {
                if(pendingBookings.Count() != 0)
                {
                    if (!triggerBookingDistribution())
                        return false;
                    double location = calculateLocationWorkload();
                    List<double> station = calculateStationWorkload();
                    if (!exScenario.updateWorkload(location, station))
                        return false;
                    ++tickCount;
                }
            }

            exScenario.fulfilledRequests = exScenario.getFulfilledRequests() + exScenario.bookings.Count(e => e.station != null);
            
            return true;
        }
        
        private double calculateLocationWorkload()
        {
            DateTime time = tickCount * tickLength + exScenario.start;
            DateTime end = time + tickLength;
            int numberOfStations = 0;

            foreach(Zone zone in exScenario.location.zones)
            {
                foreach (Station station in zone.stations)
                {
                    numberOfStations += station.maxParallelUseable;
                }
            }
            int count  = exScenario.bookings.Count( e => e.startTime >= time && e.startTime <= end);
            double workload = count * 100 / numberOfStations;

            return workload;
        }

        private List<double> calculateStationWorkload()
        {
            DateTime time = tickCount * tickLength + exScenario.start;
            DateTime end = time + tickLength;
            List<double> workload = new List<double>();
            int k = 0;

            for(int i = 0; i <= exScenario.location.zones.Count(); ++i)
            {
                for(int j = 0; j <= exScenario.location.zone[i].Count(); ++j)
                {
                    Station station = exScenario.location.zone[i].station[j];
                    int plugs = station.maxParallelUseable;
                    int usedPlugs = exScenario.bookings.Count(e => e.startTime >= time && e.startTime <= end && e.station == station);
                    workload[k++] = usedPlugs * 100 / plugs;
                }
            }
            return workload;
        }
    }
}
