using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sopro.Interfaces;
using Sopro.Models.Administration;
using Sopro.Models.Infrastructure;

namespace Sopro.Models.Simulation
{
    public class Simulator : ITrigger
    {
        public static TimeSpan tickLength = new TimeSpan(24, 0, 0);
        public int tickCount { get; set; } = -1;
        public List<Booking> pendingBookings { get; set; }
        public ExecutedScenario exScenario { get; set; }

        public bool triggerBookingDistribution()
        {
            DateTime start = exScenario.start.Add(TimeSpan.FromTicks(tickCount * tickLength.Ticks));
            DateTime end = start.AddDays(30);
            List<Booking> toBeDistributed = new List<Booking>();

            List<int> indices = new List<int>();
            int count = pendingBookings.Count();

            //calculates indices for bookings in penigbookings list, which will be added to toBeDistributed
            for(int i = 1; i <= count; ++i)
            {
                int l = (int)(Math.Round(1 / Math.Sqrt(i), 4) * 1000) % count;
                if(!indices.Contains(l) && l % 2 == 1)
                {
                    indices.Add(l);
                }
            }

            foreach(Booking item in pendingBookings.ToList())
            {
                //bookings in range [start, start+next 30 Days] 
                if (item.startTime >= start && item.endTime <= end)
                {
                    //all bookings for the next day
                    if (item.startTime >= start && item.startTime <= start.AddDays(1))
                    {
                        toBeDistributed.Add(item);
                        pendingBookings.Remove(item);
                    }

                    //booking with an index that exists in list help
                    if (indices.Contains(pendingBookings.IndexOf(item)))
                    {
                        toBeDistributed.Add(item);
                        pendingBookings.Remove(item);
                    }
                }
            }
            Console.WriteLine("Pending:" + pendingBookings.Count.ToString());
            Console.WriteLine("To be distributed:" + toBeDistributed.Count.ToString());
            exScenario.location.distributor.strategy = new StandardDistribution();
            if (!exScenario.location.distributor.run(toBeDistributed))
                return false;

            return true;
        }

        /* Gerenates bookings, calls triggerBookingDistribution.
         * While bookings exists in pendingBookings list, the method will call triggerBookingDistrtibution and calculates workloads
         */
        public async Task<bool> run()
        {
            return await Task.Run(() =>
            {
                //List<Booking> tempBookings;
                //tempBookings = generator.generateBookings(exScenario);
                //tempBookings.Sort((x, y)=> (x.startTime.CompareTo(y.startTime)));
                exScenario.bookings = new List<Booking>();

                exScenario.bookings.AddRange(exScenario.generatedBookings);

                pendingBookings = exScenario.bookings.ToList();

                if (!triggerBookingDistribution())
                    return false;

                ++tickCount;

                while (tickCount <= exScenario.duration)
                {
                    
                    if (pendingBookings.Count() > 0)
                    {
                        if (!triggerBookingDistribution())
                            return false;
                       
                        double location = calculateLocationWorkload();
                        
                        List<double> station = calculateStationWorkload();
                        if (!exScenario.updateWorkload(location, station))
                            return false;
                    }
                    ++tickCount;
                }
                
                exScenario.fulfilledRequests = exScenario.bookings.Count(e => e.station != null);

                return true;
            });    
        }
        
        /* Calculates workload for whole loaction, per tick
         */
        private double calculateLocationWorkload()
        {
            DateTime time = exScenario.start.Add(TimeSpan.FromTicks(tickCount * tickLength.Ticks));
            DateTime end = time + tickLength;
            int numberOfPlugs = 0;

            foreach(Zone zone in exScenario.location.zones)
            {
                foreach (Station station in zone.stations)
                {
                    numberOfPlugs += station.maxParallelUseable;
                }
            }
            int count  = exScenario.bookings.Count( e => e.startTime >= time && e.startTime <= end && e.station != null);
            List<Booking> boo = exScenario.bookings.FindAll(e => e.startTime >= time && e.startTime <= end && e.station != null);
            boo.OrderBy(e => e.startTime);

            int usedTogether = 0;
            if(count > 2)
            {
                for (int i = 0; i < count; ++i)
                {
                    if(boo[i].endTime > boo[i + 1].startTime)
                    {
                        ++usedTogether;
                    }
                }
            }
            
            double workload = (double)((double)count * 100.0) / (double)numberOfPlugs;

            return workload;
        }

        /* Calculates workload for every Station in one tick
         */
        private List<double> calculateStationWorkload()
        {
            DateTime time = exScenario.start.Add(TimeSpan.FromTicks(tickCount * tickLength.Ticks));
            DateTime end = time + tickLength;
            List<double> workload = new List<double>();
            int k = 0;
            Station station;
            Console.WriteLine("Zones: " + exScenario.location.zones.Count.ToString());
            Console.WriteLine("Stations: " + exScenario.location.zones[0].stations.Count.ToString());
            for(int i = 0; i < exScenario.location.zones.Count(); ++i)
            {
                for(int j = 0; j < exScenario.location.zones[i].stations.Count(); ++j)
                {
                    station = exScenario.location.zones[i].stations[j];
                    int plugs = station.maxParallelUseable;
                    int usedPlugs = exScenario.bookings.Count(e => e.startTime >= time && e.startTime <= end && e.station == station);
                    workload.Add((double)((double)usedPlugs * 100.0) / (double)plugs);
                }
            }
            return workload;
        }
    }
}
