using Sopro.Interfaces;
using Sopro.Models.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Sopro.Models.Administration
{
    public class AdHocDistribution : IDistributionStrategy
    {
        public AdHocDistribution()
        {

        }

        class UsedTimeSlots
        {
            public Station station { get; set; }
            public List<List<DateTime>> used { get; set; }

            public UsedTimeSlots(Station _station)
            {
                station = _station;
            }
        }

        public bool distribute(List<Booking> bookings, Schedule schedule, int puffer)
        {

            //Sort Bookinglist into new one (priority needed)
            List<Booking> b = bookings.OrderBy(o => o.priority).ToList();

            //Save location
            Location l = (Location)b.First().location;
            double backup = l.emergency;
            int concurrentCount = 0;

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

            //Init all existing bookings
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
                        //no Booking -> Add booking
                        if (!u.used.Any())
                        {
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
                        }
                        else
                        {
                            for (int offset = 0; bo.startTime.AddMinutes(offset + dur) < bo.endTime; offset += 15)
                            {
                                if (spotFree(u.used, bo.startTime.AddMinutes(offset), bo.startTime.AddMinutes(offset + dur), u.station))
                                {
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
                                    //go to next booking
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
            double perc = (socEnd - socStart) / 100;
            double neededCapacity = Convert.ToInt32(Math.Round(capacity * perc));
            int duration = Convert.ToInt32(Math.Round(neededCapacity / power * 60)) + puffer;

            int remainder = duration % 15;
            if (remainder == 0)
            {
                return duration;
            }

            return duration + 15 - remainder;
        }

        private PlugType selectPlug(List<PlugType> plugs)
        {
            return plugs.First();
        }
    }
}
