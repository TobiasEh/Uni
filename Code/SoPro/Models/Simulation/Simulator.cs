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
    /// Klasse ist für die Simulation von Szenarien zuständig.
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

            // Berechnet Indizes für die Buchungen in pendingBooking, welche der toBeDistributed Liste hinzugefügt werden sollen.
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
                // Buchungen im Intervall [start, start+ nächste 30 Tage].
                if ((item.startTime >= start) && (item.endTime <= end))
                {
                    // Alle Buchungen für den nächsten Tag.
                    if ((item.startTime >= start) && (item.startTime <= start.AddDays(1)))
                    {
                        toBeDistributed.Add(item);
                        pendingBookings.Remove(item);
                    }

                    // Buchungen mit dem Index, der in der Liste help existiert.
                    if (indices.Contains(pendingBookings.IndexOf(item)))
                    {
                        toBeDistributed.Add(item);
                        pendingBookings.Remove(item);
                    }
                }
            }
            // Console.WriteLine("Pending:" + pendingBookings.Count.ToString());
            // Console.WriteLine("To be distributed:" + toBeDistributed.Count.ToString());
            
            exScenario.location.distributor.strategy = new StandardDistribution();

            if ((toBeDistributed != null) && (toBeDistributed.Count > 0))
            {
                if (!exScenario.location.distributor.run(start, toBeDistributed))
                    return false;
            }
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
                exScenario.bookings = new List<Booking>();
                exScenario.bookings.AddRange(exScenario.generatedBookings);
                pendingBookings = exScenario.bookings.ToList();

                if (!triggerBookingDistribution())
                    return false;
                
                //exScenario.updateWorkload(calculateLocationWorkload(), calculateStationWorkload());

                ++tickCount;

                while (tickCount < exScenario.duration)
                {
                    if (pendingBookings.Count() > 0)
                    {
                        if (!triggerBookingDistribution())
                            return false;
                    }
                    
                    double location = calculateLocationWorkload();

                    List<double> station = calculateStationWorkload();
                    
                    if (!exScenario.updateWorkload(location, station))
                        return false;
                    ++tickCount;
                }

                int req = exScenario.getFulfilledRequests();
                exScenario.fulfilledRequests = exScenario.location.schedule.bookings.Count();
                
                return true;
            });    
        }

        private double calculateLocationWorkload()
        {
            DateTime simulatedDay = exScenario.start + tickCount * tickLength;
            TimeSpan pollingRate = new TimeSpan(0, 15, 0);
            TimeSpan start = new TimeSpan(6, 0, 0);
            TimeSpan end = new TimeSpan(20, 0, 0);

            List<Booking> relevantBookings = filterBookingsAfterDay(simulatedDay);
            List<double> polledLocationWorkloads = new List<double>();

            while (start < end)
            {
                int accumulatedPower = 0;
                foreach (Booking b in relevantBookings)
                {
                    if (bookingIsTakingPlace(b, start))
                        accumulatedPower += b.station.plugs[0].power;
                }
                // TODO: AccPower nicht jedes Mal neu berechnen. Einmal reicht.
                polledLocationWorkloads.Add(accumulatedPower / (double)getLocationMaxAccumulatedPower());
                start += pollingRate;
            }
            // foreach (double d in polledLocationWorkloads) { Console.Write(d.ToString() + "; "); }
            return polledLocationWorkloads.Average() * 100;
        }

        private int getLocationMaxAccumulatedPower()
        {
            ILocation location = exScenario.location;
            int accumulatedMaxPower = 0;

            foreach (Zone zone in location.zones)
            {
                foreach (Station station in zone.stations)
                {
                    int plugCount = station.maxParallelUseable;
                    List<int> plugPowers = new List<int>();
                    foreach (Plug plug in station.plugs)
                    {
                        plugPowers.Add(plug.power);
                    }
                    int powersToConsider = plugCount > station.plugs.Count ? station.plugs.Count : plugCount;
                    var result = plugPowers.OrderByDescending(x => x).Take(powersToConsider);
                    accumulatedMaxPower += result.Sum();
                }
            }
            return accumulatedMaxPower;
        }

        private bool bookingIsTakingPlace(Booking b, TimeSpan time)
        {
            if (time.Hours >= b.startTime.Hour && time.Hours < b.endTime.Hour)
            {
                if (time.Hours == b.startTime.Hour && time.Minutes < b.startTime.Minute)
                    return false;
                return true;
            }
            return false;
        }

        private List<Booking> filterBookingsAfterDay(DateTime day)
        {
            List<Booking> relevantBookings = new List<Booking>();
            foreach (Booking b in exScenario.location.schedule.bookings)
            {
                if (b.startTime.Date.Equals(day.Date))
                    relevantBookings.Add(b);
            }
            return relevantBookings;
        }
        
        /// <summary>
        /// Berechnet die Auslastung des Standortes pro Tick.
        /// </summary>
        /// <returns>Die berechnete Auslastung des Standortes.</returns>
        /*
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

            List<Booking> boo = exScenario.location.schedule.bookings.FindAll(e => (e.startTime >= time) && (e.startTime <= end) && (e.station != null));

            var count = boo.Count;

            boo.Sort((e, f) => e.startTime.CompareTo(f.startTime));
            var workload = 0.0;
            var usedTogether = 0.0;

            double[] countPlugs = new double[count];

            // Berechnen wie viele Buchungen überlappend Stecker benutzen.
            if (count >= numberOfPlugs)
            {
                for (int x = 0; x < count - 1; ++x)
                {
                    // Fall: Buchung und nächste Buchung überlappen sich
                    if (boo[x].endTime > boo[x + 1].startTime)
                    {
                        usedTogether += 2;
                        countPlugs[x] = usedTogether;

                        int y = x + 1;
                        while (y < numberOfPlugs)
                        {
                            // Fall: Buchung überlappt mit der nächten + y Buchung, ist das der fall schriebe +1 in die Zelle
                            if (boo[x].endTime > boo[y].startTime)
                            {
                                ++usedTogether;
                                countPlugs[x] = usedTogether;
                            }
                            // Fall: Buchung überlappt sich nicht mit der nächsten + y Buchung, ist das der fall gehe zu nächste Buchung (x+1) und setzt usedTogether auf 0.0
                            else
                            {
                                usedTogether = 0.0;
                                break;
                            }
                            ++y;
                        }
                    }
                    // Fall: Buchung und nächste Buchung überlappen sich nicht -> es wird 1 in die Liste geschrieben, da ein Plug benutzt wurde 
                    else
                    {
                        usedTogether = 1.0;
                        countPlugs[x] = usedTogether;
                    }
                }
                usedTogether = countPlugs.Max();
            // Falls die anzahl der Buchungen kleiner als die maximalen nutzbaren sind, muss anders vorgegangen werden.
            } else if (count >= 2)
            {
                for (int x = 0; x < count - 1; ++x)
                {
                    if (boo[x].endTime > boo[x + 1].startTime)
                    {
                        usedTogether += 2;
                        countPlugs[x] = usedTogether;

                        int y = x;
                        while (y < count)
                        {
                            if (boo[x].endTime > boo[y].startTime)
                            {
                                ++usedTogether;
                                countPlugs[x] = usedTogether;
                            }
                            else
                            {
                                continue;
                            }
                            ++y;
                        }
                    }
                    else
                    {
                        usedTogether = 1.0;
                        countPlugs[x] = usedTogether;
                    }
                }
                usedTogether = countPlugs.Max();
            } else if(count == 1)
            {
                ++usedTogether;
            }

            workload = (double)(usedTogether * 100.0) / (double)numberOfPlugs;
            return workload;
        }
        */
        /// <summary>
        /// Berechnet die Auslastung aller Stationen pro Tick.
        /// </summary>
        /// <returns>Die Auslastung alles Stationen in einer Liste.</returns>
        private List<double> calculateStationWorkload()
        {
            DateTime time = exScenario.start.Add(TimeSpan.FromTicks(tickCount * tickLength.Ticks));
            DateTime end = time + tickLength;
            List<double> workload = new List<double>();

            Station station;
            //Console.WriteLine("Zones: " + exScenario.location.zones.Count.ToString());
            //Console.WriteLine("Stations: " + exScenario.location.zones[0].stations.Count.ToString());
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
