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
            public int used { get; set; }
            public int total;
            public DateTime day;

            public Workload(DateTime _day, int concurrentCount)
            {
                day = _day;
                used = 0;
                total = concurrentCount * 60 * 24;
            }

            public Workload(DateTime _day, int concurrentCount, int _used)
            {
                day = _day;
                used = _used;
                total = concurrentCount * 60 * 24;
            }

            public double getWorkload()
            {
                return used / total;
            }

            public double getWorkload(int duration)
            {
                return (used + duration) / total;
            }
        }

        class UsedTimeSlots
        {
            public Station station { get; set; }
            public List<List<DateTime>> used { get; set; }

            public UsedTimeSlots(Station _station)
            {
                station = _station;
                used = new List<List<DateTime>>();
            }
        }

        public bool distribute(List<Booking> bookings, Schedule schedule, int puffer)
        {

            //Sort Bookinglist into new one (priority needed)
            List<Booking> b = bookings.OrderBy(o => o.priority).ToList();

            //Save location, backup capacity
            Location l = (Location)b.First().location;
            double backup = l.emergency;
            List<Workload> wl = new List<Workload>();
            int concurrentCount = 0; //total count of concurrent bookings of location

            //Create new UsedTimeSlot object
            List<UsedTimeSlots> usedTimeSlots = new List<UsedTimeSlots>();

            //Add Stations into UsedTimeSlotList
            foreach (Zone z in l.zones)
            {
                foreach (Station s in z.stations)
                {
                    usedTimeSlots.Add(new UsedTimeSlots(s));
                    concurrentCount += s.maxParallelUseable;
                }
            }

            //Init all existing bookings in schedule into the workload wl/UsedTimeSlots
            foreach (Booking bo in schedule.bookings)
            {
                foreach (UsedTimeSlots u in usedTimeSlots)
                {
                    if (bo.station.Equals(u.station))
                    {
                        List<DateTime> temp = new List<DateTime>();
                        temp.Add(bo.startTime);
                        temp.Add(bo.endTime);
                        u.used.Add(temp);
                        if (bo.startTime.Day.Equals(bo.endTime.Day))
                        {
                            TimeSpan span = bo.endTime.Subtract(bo.startTime);
                            if (wl.Exists(x => x.day.Day.Equals(bo.startTime.Day)))
                            {
                                wl.Find(x => x.day.Day.Equals(bo.startTime.Day)).used += (int)span.TotalMinutes;
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
                            TimeSpan spanStart = d.Subtract(bo.startTime);
                            TimeSpan spanEnd = bo.endTime.Subtract(d);
                            if (wl.Exists(x => x.day.Day.Equals(bo.startTime.Day)))
                            {
                                wl.Find(x => x.day.Day.Equals(bo.startTime.Day)).used += (int)spanStart.TotalMinutes;
                            }
                            else
                            {
                                Workload w = new Workload(bo.startTime, concurrentCount, (int)spanStart.TotalMinutes);
                                wl.Add(w);
                            }
                            if (wl.Exists(x => x.day.Day.Equals(bo.endTime.Day)))
                            {
                                wl.Find(x => x.day.Day.Equals(bo.endTime.Day)).used += (int)spanEnd.TotalMinutes;
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

            //Each Booking
            foreach (Booking bo in b)
            {
                /*double perc = (bo.socEnd - bo.socStart) / 100;
                double capNeeded = bo.capacity * perc;*/

                bool inserted = false;
                //Check every Station
                foreach (UsedTimeSlots u in usedTimeSlots)
                {
                    //Has needed plugs
                    if (hasRequestedPlugs(bo, u.station))
                    {
                        /*int power = 0;
                        foreach (Plug p in u.station.plugs)
                        {
                            power += p.power;
                        }*/
                        PlugType selected = selectPlug(bo.plugs);
                        int dur = calculateDuration(bo.socStart, bo.socEnd, bo.capacity, u.station.plugs.Find(x => x.type.Equals(selected)).power, puffer);
                        //capNeeded / power * 60;
                        //Check capacity cap
                        if (wl.Exists(x => x.day.Day.Equals(bo.startTime.Day)))
                        {
                            if ((wl.Find(x => x.day.Day.Equals(bo.startTime.Day)).getWorkload(dur) + backup) > 1)
                            {
                                break;
                            }
                        }
                        //Check if booking duration is possible in the time periode
                        if (bo.startTime.AddMinutes(dur) > bo.endTime)
                        {
                            break;
                        }

                        //no Booking -> Add booking
                        if (!u.used.Any())
                        {
                            //Add booking
                            bo.station = u.station;
                            bo.endTime = bo.startTime.AddMinutes(dur);
                            bo.plugs.Clear();
                            bo.plugs.Add(selected);
                            List<DateTime> temp = new List<DateTime>();
                            temp.Add(bo.startTime);
                            temp.Add(bo.endTime);
                            u.used.Add(temp);
                            schedule.addBooking(bo);
                            inserted = true;
                            if (wl.Exists(x => x.day.Day.Equals(bo.endTime.Day)))
                            {
                                wl.Find(x => x.day.Day.Equals(bo.endTime.Day)).used += dur;
                            }
                            else
                            {
                                Workload w = new Workload(bo.endTime, concurrentCount, dur);
                                wl.Add(w);
                            }
                        }
                        else
                        {
                            //Find free spot for current booking
                            for (int offset = 0; bo.startTime.AddMinutes(offset + dur) < bo.endTime; offset += 15)
                            {
                                //Check if there is a free sport
                                if (spotFree(u.used, bo.startTime.AddMinutes(offset), bo.startTime.AddMinutes(offset + dur), u.station))
                                {
                                    //Add booking
                                    bo.station = u.station;
                                    bo.startTime = bo.startTime.AddMinutes(offset);
                                    bo.endTime = bo.startTime.AddMinutes(offset + dur);
                                    bo.plugs.Clear();
                                    bo.plugs.Add(selected);
                                    List<DateTime> temp = new List<DateTime>();
                                    temp.Add(bo.startTime);
                                    temp.Add(bo.endTime);
                                    u.used.Add(temp);
                                    schedule.addBooking(bo);
                                    inserted = true;
                                    if (wl.Exists(x => x.day.Day.Equals(bo.endTime.Day)))
                                    {
                                        wl.Find(x => x.day.Day.Equals(bo.endTime.Day)).used += dur;
                                    }
                                    else
                                    {
                                        Workload w = new Workload(bo.endTime, concurrentCount, dur);
                                        wl.Add(w);
                                    }
                                    //go to next booking
                                    break;
                                }
                            }
                        }
                    }
                    //if booking was inserted dont look for another fiting station
                    if (inserted)
                    {
                        break;
                    }
                }
            }
            return true;
        }

        //Checks if station has the requested PlugType
        private bool hasRequestedPlugs(Booking b, Station s)
        {
            foreach (PlugType pt in b.plugs)
            {
                bool found = false;
                foreach (Plug p in s.plugs)
                {
                    if (pt.Equals(p.type))
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

        //Returns true if Date1(start1, end1) and Date2(start2, end2) dont have any shared time
        private bool betweenD(DateTime start1, DateTime end1, DateTime start2, DateTime end2)
        {
            if (start1 > end2 || end1 < start2)
            {
                return true;
            }
            return false;
        }

        //Looks if the current booking defined by start/end can be inserted in the station
        private bool spotFree(List<List<DateTime>> spots, DateTime start, DateTime end, Station station)
        {
            int concurrent = 0;
            foreach (List<DateTime> d in spots)
            {
                //Check Timeslots
                if (!betweenD(d.First(), d.Last(), start, end))
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

        //Calculates needed charging duration and rounds up to the next 15 min. intervall
        private int calculateDuration(int socStart, int socEnd, int capacity, int power, int puffer)
        {
            double soc = socEnd - socStart;
            double perc = soc / 100;

            int neededCapacity = Convert.ToInt32(Math.Round(capacity * perc));
            double dur = neededCapacity / power * 60 + puffer;
            int duration = Convert.ToInt32(dur);



            int remainder = duration % 15;
            if (remainder == 0)
            {
                return duration;
            }

            return duration + 15 - remainder;
        }

        //Selects first Plug
        private PlugType selectPlug(List<PlugType> plugs)
        {
            return plugs.First();
        }
    }
}
