using Sopro.Interfaces;
using Sopro.Models.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Sopro.Models.Administration
{
    public class StandardDistribution : IDistributionStrategy
    {
        public StandardDistribution()
        {

        }
         
        class Workload
        {
            public int Used { get; set; }
            public int Total;
            public DateTime Day;

            public Workload(DateTime _day, int concurrentCount)
            {
                Day = _day;
                Used = 0;
                Total = concurrentCount * 60 * 60 * 24;
            }

            public Workload(DateTime _day, int concurrentCount, int _used)
            {
                Day = _day;
                Used = _used;
                Total = concurrentCount * 60  * 60 * 24;
            }

            public double GetWorkload()
            {
                return (double)Used / (double)Total;
            }

            public double GetWorkload(int duration)
            {
                double d = ((double)Used + (double)duration) / (double)Total;
                return ((double)Used + (double)duration) / (double)Total;
            }
        }

        public bool distribute(List<Booking> bookings, Schedule schedule, int puffer)
        {
            // Sortiert Buchungsliste nach Proirität.
            int[] map = { 4, 0, 1, 2, 3 };
            List<Booking> sortedBookings = bookings.OrderBy(o => map[(int)(o.priority)]).ToList();
            

            // Speichert location und emergency ab.
            Location location = (Location)sortedBookings.First().location;
            double backup = location.emergency;
            List<Workload> wl = new List<Workload>();
            int concurrentCount = 0;

            //List<UsedTimeSlots> usedTimeSlots = new List<UsedTimeSlots>();
            List<Station> stations = new List<Station>();

            // Erstelle Liste aller Stationen
            foreach (Zone z in location.zones)
            {
                foreach (Station s in z.stations)
                {
                    stations.Add(s);
                    concurrentCount += s.maxParallelUseable;
                }
            }

            // Berechne aktuelle Workload.
            foreach (Booking bo in schedule.bookings)
            {
                if (bo.startTime.Day.Equals(bo.endTime.Day))
                {
                    TimeSpan span = bo.endTime - bo.startTime;
                    if (wl.Exists(x => x.Day.Day.Equals(bo.startTime.Day)))
                    {
                        wl.Find(x => x.Day.Day.Equals(bo.startTime.Day)).Used += (int)span.TotalMinutes;
                    }
                    else
                    {
                        Workload w = new Workload(bo.startTime, concurrentCount, (int)span.TotalMinutes);
                        wl.Add(w);
                    }
                }
                else
                {
                    DateTime d = new DateTime(bo.startTime.Year, bo.startTime.Month, bo.startTime.Day).AddDays(1);
                    TimeSpan spanStart = d - bo.startTime;
                    TimeSpan spanEnd = bo.endTime - d;
                    if (wl.Exists(x => x.Day.Day.Equals(bo.startTime.Day)))
                    {
                        wl.Find(x => x.Day.Day.Equals(bo.startTime.Day)).Used += (int)spanStart.TotalMinutes;
                    }
                    else
                    {
                        Workload w = new Workload(bo.startTime, concurrentCount, (int)spanStart.TotalMinutes);
                        wl.Add(w);
                    }
                    if (wl.Exists(x => x.Day.Day.Equals(bo.endTime.Day)))
                    {
                        wl.Find(x => x.Day.Day.Equals(bo.endTime.Day)).Used += (int)spanEnd.TotalMinutes;
                    }
                    else
                    {
                        Workload w = new Workload(bo.endTime, concurrentCount, (int)spanEnd.TotalMinutes);
                        wl.Add(w);
                    }
                }
            }

            // Versuche jede Buchung einzufügen.
            foreach (Booking booking in sortedBookings)
            {
                bool inserted = false;
                // Überprüfe aktuelle Station.
                int duration = 0;
                PlugType plugType;
                Station station;
                DateTime startTime;
                
                    (inserted, duration, plugType, station, startTime) = BestSpotForBooking(booking, schedule.bookings, stations, puffer);
                    if(inserted)
                    {
                        
                        if (wl.Exists(x => x.Day.Day.Equals(startTime.Day)))
                        {
                            if ((wl.Find(x => x.Day.Day.Equals(startTime.Day)).GetWorkload(duration) + backup) > 1)
                            {
                                continue;
                            }
                        }
                        if(booking.startTime.AddMinutes(duration) > booking.endTime)
                        {
                            continue;
                        }
                        booking.startTime = startTime;
                        booking.endTime = booking.startTime.AddMinutes(duration).AddSeconds(-1);
                        booking.plugs.Clear();
                        booking.plugs.Add(plugType);
                        booking.station = station;
                        schedule.addBooking(booking);
                        if (wl.Exists(x => x.Day.Day.Equals(booking.endTime.Day)))
                        {
                            wl.Find(x => x.Day.Day.Equals(booking.endTime.Day)).Used += duration;
                        }
                        else
                        {
                            Workload w = new Workload(booking.endTime, concurrentCount, duration);
                            wl.Add(w);
                        }
                    } else
                    {
                        break;
                    }
                }

            return true;
        }

        /// <summary>Überprüfe ob die Station die benötigten PlugTypen hat.</summary
        /// <param name="plugType">Zu überprüfender PlugType.</param>
        /// <param name="station">zu überprüfende Station.</param>
        /// <returns>true: falls alle PlugTyp in der Station vorhanden ist
        ///          sonst: false</returns>
        private bool HasRequestedPlugs(PlugType plugType, Station station)
        {
            bool found = false;
            foreach (Plug plug in station.plugs)
            {
                if (plug.type.Equals(plugType))
                {
                    found = true;
                }
            }
            if (!found)
            {
                return false;
            }
            return true;
        }

        /// <summary>Prüfe ob die beiden Zeitintervalle sich überschneiden sind.</summary>
        /// <param name="start1">Startzeit des ersten Intervalls.</param>
        /// <param name="end1">Endzeit des ersten Intervalls.</param>
        /// <param name="start2">Startzeit des zweiten Intervalls.</param>
        /// <param name="end2">Endzeit des zweiten Intervalls.</param>
        /// <returns>true: wenn beide Intervalle nicht überlappen
        ///          sonst: false</returns>
        private bool DateOverlapping(DateTime start1, DateTime end1, DateTime start2, DateTime end2)
        {
            if (start1 <= end2 && end1 >= start2)
            {
                return true;
            }
            return false;
        }

        /// <summary>Prüft, ob die aktuelle Buchung noch in der Station eingefügt werden kann.</summary>
        /// <param name="schedule">Liste der schon eingefügten Buchungen</param>
        /// <param name="station">Zu prüfende Station.</param>
        /// <param name="start">Startzeit der Buchung.</param>
        /// <param name="end">Endzeit der Buchung.</param>
        /// <returns>true: wenn Station noch einen Platz für die Buchung frei hat
        ///          sonst: false</returns>
        private bool CheckCurrentBooking(List<Booking> schedule, DateTime start, DateTime end, Station station)
        {
            int concurrent = 0;
            int usedPower = 0;
            foreach (Booking b in schedule)
            {
                if (b.station.uniqueId.Equals(station.uniqueId))
                {
                    if(DateOverlapping(start, end, b.startTime, b.endTime))
                    {
                        concurrent++;
                        usedPower += station.plugs.Find(x => x.type.Equals(b.plugs.First())).power;
                    }
                }
            }
            if (!(usedPower <= station.maxPower))
            {
                return false;
            } 
            if (concurrent < station.maxParallelUseable)
            {
                return true;
            }
            return false;
        }

        /// <summary>Berechnet die benötigte Ladedauer und rundet diese auf das nächste 15 Min. Intervall.</summary>
        /// <param name="socStart">Start-Ladezustand (SOC).</param>
        /// <param name="socEnd">End-Ladezustand (SOC).</param>
        /// <param name="capacity">Max. Kapazität der Buchung/Autos.</param>
        /// <param name="power">Ladestärke der Station.</param>
        /// <param name="puffer">Pufferzeit zwischen Buchungen.</param>
        private int CalculateDuration(int socStart, int socEnd, int capacity, int power, int puffer)
        {
            double soc = socEnd - socStart;
            double perc = soc / 100;

            int neededCapacity = Convert.ToInt32(Math.Round(capacity * perc));
            double dur = ((double)neededCapacity / (double)power) * 60 + puffer;
            int duration = Convert.ToInt32(dur);

            int remainder = duration % 15;
            if (remainder == 0)
            {
                return duration;
            }

            return duration + 15 - remainder;
        }

        /// <summary>Rundet den Zeitpunkt auf das nächste d.Minuten Intervall. </summary>
        /// <param name="dt">Zeitpunkt.</param>
        /// <param name="d">Rundungsparameter.</param>
        /// <returns>Aufgerundetes Datum.</returns>
        private DateTime RoundUp(DateTime dt, TimeSpan d)
        {
            return new DateTime((dt.Ticks + d.Ticks - 1) / d.Ticks * d.Ticks, dt.Kind);
        }

        /// <summary>
        /// Überprüft ob Buchung eingefügt werden kann und gibt dann das beste Tupel für diese Buchung zurück
        /// </summary>
        /// <param name="booking">Zu überprüfenden Buchung</param>
        /// <param name="schedule">Liste der bereits eingefügten Buchungen</param>
        /// <param name="stations">Liste der Stationen der Location</param>
        /// <param name="puffer">Pufferzeit in Minuten</param>
        /// <returns>Tupel: (true, beste Dauer, PlugType, Station, Startzeit), falls Buchung angenommen werden kann
        ///                 sonst: false</returns>
        private (bool, int, PlugType, Station, DateTime) BestSpotForBooking (Booking booking, List<Booking> schedule, List<Station> stations, int puffer)
        {
            bool ok = false;
            int bestDuration = int.MaxValue;
            PlugType bestPlug = booking.plugs.First();
            Station bestStation = null;
            booking.startTime = RoundUp(booking.startTime, TimeSpan.FromMinutes(15));
            DateTime bestStartTime = booking.startTime;

            foreach (Station station in stations)
            {
                foreach(PlugType plugType in booking.plugs)
                {
                    if (HasRequestedPlugs(plugType, station))
                    {
                        
                        int duration = CalculateDuration(booking.socStart, booking.socEnd, booking.capacity, station.plugs.Find(x => x.type.Equals(plugType)).power, puffer);
                        for (int offset = 0; booking.startTime.AddMinutes(offset + duration) < booking.endTime; offset += 15)
                        {
                            if (CheckCurrentBooking(schedule, booking.startTime.AddMinutes(offset), booking.startTime.AddMinutes(offset + duration), station))
                            {
                                if(bestDuration > duration)
                                {
                                    ok = true;
                                    bestDuration = duration;
                                    bestPlug = plugType;
                                    bestStation = station;
                                    bestStartTime = booking.startTime.AddMinutes(offset);
                                    break;
                                }
                            }
                        }
                    }
                } 
            }

            return (ok, bestDuration, bestPlug, bestStation, bestStartTime);
        }
    }
}
