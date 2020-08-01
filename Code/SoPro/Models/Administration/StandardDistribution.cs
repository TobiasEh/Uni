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
                Total = concurrentCount * 60 * 24;
            }

            public Workload(DateTime _day, int concurrentCount, int _used)
            {
                Day = _day;
                Used = _used;
                Total = concurrentCount * 60 * 24;
            }

            public double GetWorkload()
            {
                return (double)Used / (double)Total;
            }

            public double GetWorkload(int duration)
            {
                return ((double)Used + (double)duration) / (double)Total;
            }
        }

        class UsedTimeSlots
        {
            public Station Station { get; set; }
            public List<List<DateTime>> Used { get; set; } = new List<List<DateTime>>();

            public UsedTimeSlots(Station _station)
            {
                Station = _station;
                Used = new List<List<DateTime>>();
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

            List<UsedTimeSlots> usedTimeSlots = new List<UsedTimeSlots>();

            // Füge alle Stations in usedTimeSlots.
            foreach (Zone z in location.zones)
            {
                foreach (Station s in z.stations)
                {
                    usedTimeSlots.Add(new UsedTimeSlots(s));
                    concurrentCount += s.maxParallelUseable;
                }
            }

            // Füge alle bereits gebuchten Bookings in usedTimeSlots ein und berechne ahtuelle Workload.
            foreach (Booking bo in schedule.bookings)
            {
                foreach (UsedTimeSlots u in usedTimeSlots)
                {
                    if (bo.station.Equals(u.Station))
                    {
                        List<DateTime> temp = new List<DateTime>();
                        temp.Add(bo.startTime);
                        temp.Add(bo.endTime);
                        u.Used.Add(temp);
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
                }
            }

            // Versuche jede Buchung einzufügen.
            foreach (Booking booking in sortedBookings)
            {
                bool inserted = false;
                // Überprüfe aktuelle Station.
                foreach (UsedTimeSlots station in usedTimeSlots)
                {
                    // Hat die Station die benötigten Plugs.
                    if (HasRequestedPlugs(booking, station.Station))
                    {
                        PlugType selected = SelectPlug(booking.plugs);
                        int duration = CalculateDuration(booking.socStart, booking.socEnd, booking.capacity, station.Station.plugs.Find(x => x.type.Equals(selected)).power, puffer);
                        booking.startTime = RoundUp(booking.startTime, TimeSpan.FromMinutes(15));
                        if (wl.Exists(x => x.Day.Day.Equals(booking.startTime.Day)))
                        {
                            if ((wl.Find(x => x.Day.Day.Equals(booking.startTime.Day)).GetWorkload(duration) + backup) > 1)
                            {
                                break;
                            }
                        }
                        // Ist noch genug Workload verfügbar für die aktuelle Buchung.
                        if (booking.startTime.AddMinutes(duration) > booking.endTime)
                        {
                            break;
                        }

                        if (!station.Used.Any())
                        {
                            booking.station = station.Station;
                            booking.endTime = booking.startTime.AddMinutes(duration).AddSeconds(-1);
                            booking.plugs.Clear();
                            booking.plugs.Add(selected);
                            List<DateTime> temp = new List<DateTime>();
                            temp.Add(booking.startTime);
                            temp.Add(booking.endTime);
                            station.Used.Add(temp);
                            schedule.addBooking(booking);
                            inserted = true;
                            if (wl.Exists(x => x.Day.Day.Equals(booking.endTime.Day)))
                            {
                                wl.Find(x => x.Day.Day.Equals(booking.endTime.Day)).Used += duration;
                            }
                            else
                            {
                                Workload w = new Workload(booking.endTime, concurrentCount, duration);
                                wl.Add(w);
                            }
                        }
                        else
                        {
                            // Versuche Buchung dynamisch einzuspielen.
                            for (int offset = 0; booking.startTime.AddMinutes(offset + duration) < booking.endTime; offset += 15)
                            {
                                // Gibt es noch einen Platz für die Buchung.
                                if (CheckCurrentBooking(station.Used, booking.startTime.AddMinutes(offset), booking.startTime.AddMinutes(offset + duration), station.Station))
                                {
                                    booking.station = station.Station;
                                    booking.startTime = booking.startTime.AddMinutes(offset);
                                    booking.endTime = booking.startTime.AddMinutes(duration).AddSeconds(-1);
                                    booking.plugs.Clear();
                                    booking.plugs.Add(selected);
                                    List<DateTime> temp = new List<DateTime>();
                                    temp.Add(booking.startTime);
                                    temp.Add(booking.endTime);
                                    station.Used.Add(temp);
                                    schedule.addBooking(booking);
                                    inserted = true;
                                    if (wl.Exists(x => x.Day.Day.Equals(booking.endTime.Day)))
                                    {
                                        wl.Find(x => x.Day.Day.Equals(booking.endTime.Day)).Used += duration;
                                    }
                                    else
                                    {
                                        Workload w = new Workload(booking.endTime, concurrentCount, duration);
                                        wl.Add(w);
                                    }
                                    break;
                                }
                            }
                        }
                    }
                    if (inserted)
                    {
                        break;
                    }
                }
            }
            return true;
        }

        /// <summary>Überprüfe ob die Station die benötigten PlugTypen hat.</summary
        /// <param name="booking">Zu überprüfenden Buchung.</param>
        /// <param name="station">zu überprüfende Station.</param>
        /// <returns>true: falls alle PlugTyps der Buchung in der Station vorhanden sind
        ///          sonst: false</returns>
        private bool HasRequestedPlugs(Booking booking, Station station)
        {
            foreach (PlugType plugType in booking.plugs)
            {
                bool found = false;
                foreach (Plug plug in station.plugs)
                {
                    if (plugType.Equals(plug.type))
                    {
                        found = true;
                    }
                }
                if (!found)
                {
                    return false;
                }
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
            if (start1 > end2 || end1 < start2)
            {
                return true;
            }
            return false;
        }

        /// <summary>Prüft, ob die aktuelle Buchung noch in der Station eingefügt werden kann.</summary>
        /// <param name="spots">Bereits belegte Zeitperioden.</param>
        /// <param name="station">Zu prüfende Station.</param>
        /// <param name="start">Startzeit der Buchung.</param>
        /// <param name="end">Endzeit der Buchung.</param>
        /// <returns>true: wenn Station noch einen Platz für die Buchung frei hat
        ///          sonst: false</returns>
        private bool CheckCurrentBooking(List<List<DateTime>> spots, DateTime start, DateTime end, Station station)
        {
            int concurrent = 0;
            foreach (List<DateTime> spot in spots)
            {
                if (!DateOverlapping(spot.First(), spot.Last(), start, end))
                {
                    concurrent++;
                }
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

        /// <summary>Wählt ein passendes Plug aus der Liste aus.</summary>
        /// <param name="plugs">Verfügbare Plugs der Buchung.</param>
        /// <returns>Ausgewähltes Plug.</returns>
        private PlugType SelectPlug(List<PlugType> plugs)
        {
            return plugs.First();
        }

        /// <summary>Rundet den Zeitpunkt auf das nächste d.Minuten Intervall. </summary>
        /// <param name="dt">Zeitpunkt.</param>
        /// <param name="d">Rundungsparameter.</param>
        /// <returns>Aufgerundetes Datum.</returns>
        private DateTime RoundUp(DateTime dt, TimeSpan d)
        {
            return new DateTime((dt.Ticks + d.Ticks - 1) / d.Ticks * d.Ticks, dt.Kind);
        }
    }
}
