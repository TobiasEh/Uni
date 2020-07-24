using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sopro.Interfaces;
using Sopro.Models.Administration;
using Sopro.Models.Infrastructure;

namespace Sopro.Models.Simulation
{
    /// <summary>
    /// Klasse ist für die Simulation zuständig.
    /// </summary>
    public class Simulator : ITrigger
    {
        public static TimeSpan tickLength = new TimeSpan(24, 0, 0);
        public int tickCount { get; set; } = -1;
        public List<Booking> pendingBookings { get; set; }
        public ExecutedScenario exScenario { get; set; }

        /// <summary>
        /// Startet die Verteilung der Buchungen, indem die entsprechende Methode aufgerufen wird.
        /// </summary>
        /// <returns>Wahrheitswert ob das starten der Verteilung erfolgreich war.</returns>
        public bool triggerBookingDistribution()
        {
            DateTime start = exScenario.start.Add(TimeSpan.FromTicks(tickCount * tickLength.Ticks));
            DateTime end = start.AddDays(30);
            var toBeDistributed = new List<Booking>();

            var indices = new List<int>();
            int count = pendingBookings.Count();

            // Berechnet Indizes für die Buchungen in pendingBooking, welche der toBeDistributed Liste hinzugefügt werden sollen
            for(int i = 1; i <= count; ++i)
            {
                int l = (int)(Math.Round(1 / Math.Sqrt(i), 4) * 1000) % count;
                if((!indices.Contains(l)) && (l % 2 == 1))
                {
                    indices.Add(l);
                }
            }

            foreach(Booking item in pendingBookings.ToList())
            {
                // Buchungen im Intervall [start, start+ nächste 30 Tage] 
                if ((item.startTime >= start) && (item.endTime <= end))
                {
                    // Alle Buchungen für den nächsten Tag
                    if ((item.startTime >= start) && (item.startTime <= start.AddDays(1)))
                    {
                        toBeDistributed.Add(item);
                        pendingBookings.Remove(item);
                    }

                    // Buchungen mit dem Index, der in der Liste help existiert
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

        /// <summary>
        /// Startet den Verteilungsmechanismus, indem Buchungen - durch eine andere Funktion - generiert werden und diese Verteilt werden.
        /// Solange Buchungen in der List pendingBookings sind, wird die Methode triggerBookingDistribution aufgerufen, und die einzelnen Auslastungen berechnet. 
        /// </summary>
        /// <returns>Thread der den Verteilungsmechanismus startet.</returns>
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
        
        /// <summary>
        /// Berechnet die Auslastung des Standortes pro Tick.
        /// </summary>
        /// <returns>Die berechnete Auslastung des Standortes.</returns>
        private double calculateLocationWorkload()
        {
            DateTime time = exScenario.start.Add(TimeSpan.FromTicks(tickCount * tickLength.Ticks));
            DateTime end = time + tickLength;
            var numberOfPlugs = 0;

            foreach(Zone zone in exScenario.location.zones)
            {
                foreach (Station station in zone.stations)
                {
                    numberOfPlugs += station.maxParallelUseable;
                }
            }

            // Zählt die Buchungen, die im Zeitraum liegen und bei denen eine Station gesetzt ist.
            //int count  = exScenario.bookings.Count( e => (e.startTime >= time) && (e.startTime <= end) && (e.station != null));

            List<Booking> boo = exScenario.bookings.FindAll(e => (e.startTime >= time) && (e.startTime <= end) && (e.station != null));
            var count = boo.Count;

            boo.OrderBy(e => e.startTime);
            var workload = 0.0;
            var usedTogether = 0.0;

            if (count >= 2)
            {
                    for (int i = 0; i < count - 1; ++i)
                    {
                        if (boo[i].endTime > boo[i + 1].startTime)
                        {
                            ++usedTogether;
                        }
                    }
            } else
            {
                ++usedTogether;
            }
                workload = (double)(usedTogether * 100.0) / (double)numberOfPlugs;
                return workload;
        }

        /// <summary>
        /// Berechnet die Auslastung aller Stationen pro Tick.
        /// </summary>
        /// <returns>Die Auslastung alles Stationen in einer Liste.</returns>
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

                    // Zählt die Buchungen, die diese bestimmte Station zugewiesen bekommen haben.
                    //int usedPlugs = exScenario.bookings.Count(e => e.startTime >= time && e.startTime <= end && e.station == station);
                    

                    List<Booking> boo = exScenario.bookings.FindAll(e => e.startTime >= time && e.startTime <= end && e.station == station);
                    var usedPlugs = boo.Count;

                    boo.OrderBy(e => e.startTime);
                    var usedTogether = 0.0;
                    if (usedPlugs >= 2)
                    {
                        for (int l = 0; l < usedPlugs - 1; ++l)
                        {
                            if (boo[l].endTime > boo[l + 1].startTime)
                            {
                                ++usedTogether;
                            }
                        }
                    } else
                    {
                        ++usedTogether;
                    }

                    workload.Add((double)((usedTogether) * 100.0) / (double)plugs);


                }
            }
            return workload;
        }
    }
}
