/*
using Microsoft.AspNetCore.Mvc.Formatters;
using Sopro.Interfaces;
using Sopro.Models.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sopro.Models.Administration
{
    public class UsedTimeSlots
    {
        public Station station { get; set; }
        public List<List<DateTime>> used { get; set; }

        public UsedTimeSlots(Station _station)
        {
            station = _station;
        }
    }

    public class StandardDistribution : IDistributionStrategy
    {
        public bool distribute(List<Booking> bookings, Schedule schedule, int puffer)
        {
            //Sort Bookinglist into new one (priority needed)
            List<Booking> b = bookings.OrderBy(o => o.priority).ToList();

            //Save location
            Location l = b.First().location;

            //Create new UsedTimeSlot object
            List<UsedTimeSlots> usedTimeSlots = new List<UsedTimeSlots>();

            //Add Stations into UsedTimeSlotList
            foreach(Zone z in l.zones)
            {
                foreach(Station s in z.stations)
                {
                    usedTimeSlots.Add(new UsedTimeSlots(s));
                }
            }

            //Each Booking
            foreach(Booking bo in b)
            {
                double perc = (bo.socEnd - bo.socStart) /100;
                double capNeeded = bo.capacity * perc;
                //Check every Station
                foreach(UsedTimeSlots u in usedTimeSlots)
                {
                    //Has needed plugs
                    if(hasRequestedPlugs(bo, u.station))
                    {
                        int power = 0;
                        foreach(Plug p in u.station.plugs)
                        {
                            power += p.power;
                        }
                        double dur = capNeeded / power * 60;
                        //no Booking yes -> Add booking
                        if (!u.used.Any())
                        {
                            bo.station = u.station;
                            bo.endTime = bo.startTime.AddMinutes(dur + puffer);
                            List<DateTime> temp = new List<DateTime>();
                            temp.Add(bo.startTime);
                            temp.Add(bo.endTime);
                            u.used.Add(temp);
                            schedule.addBooking(bo);
                            //go to next booking
                            continue;
                        } else
                        {
                            for(int offset = 0; bo.startTime.AddMinutes(offset + dur) < bo.endTime; offset+=15)
                            {
                                if(spotFree(u.used, bo.startTime.AddMinutes(offset), bo.startTime.AddMinutes(offset + dur), u.station))
                                {
                                    bo.station = u.station;
                                    bo.startTime = bo.startTime.AddMinutes(offset);
                                    bo.endTime = bo.startTime.AddMinutes(offset + dur);
                                    List<DateTime> temp = new List<DateTime>();
                                    temp.Add(bo.startTime);
                                    temp.Add(bo.endTime);
                                    u.used.Add(temp);
                                    schedule.addBooking(bo);
                                    //go to next booking
                                    continue;
                                }
                            }
                            /*
                            int concurrent = 0;
                            //Used Timeslots
                            foreach (List<DateTime> d in u.used)
                            {
                                //Check Timeslots
                                if (!betweenD(d.First(), d.Last(), bo.startTime, bo.startTime.AddMinutes(dur)))
                                {
                                    concurrent++;
                                } 
                            }
                            //max concurrent uses of station not achived -> 
                            if(concurrent < u.station.maxParallelUseable)
                            {
                                bo.station = u.station;
                                bo.endTime = bo.startTime.AddMinutes(dur + puffer);
                                List<DateTime> temp = new List<DateTime>();
                                temp.Add(bo.startTime);
                                temp.Add(bo.startTime.AddMinutes(dur + puffer));
                                u.used.Add(temp);
                                schedule.addBooking(bo);
                                //go to next booking
                                continue;
                            } else
                            {
                                concurrent = 0;
                                foreach (List<DateTime> d in u.used)
                                {
                                    //Check Timeslots
                                    if (!betweenD(d.First(), d.Last(), bo.endTime.AddMinutes(-dur), bo.endTime))
                                    {
                                        concurrent++;
                                    }
                                }
                                if (concurrent < u.station.maxParallelUseable)
                                {
                                    bo.station = u.station;
                                    bo.startTime = bo.endTime.AddMinutes(-dur);
                                    bo.endTime = bo.endTime.AddMinutes(puffer);
                                    List<DateTime> temp = new List<DateTime>();
                                    temp.Add(bo.startTime);
                                    temp.Add(bo.startTime.AddMinutes(dur + puffer));
                                    u.used.Add(temp);
                                    schedule.addBooking(bo);
                                    //go to next booking
                                    continue;
                                }
                            }*/ /*
                        }
                    }
                }
            }
            return true;
        }

        private bool hasRequestedPlugs(Booking b, Station s)
        {
            foreach(PlugType pt in b.plugs)
            {
                bool found = false;
                foreach(Plug p in s.plugs)
                {
                    if(pt.Equals(p.GetType()))
                    {
                        found = true;
                    }
                }
                if(!found)
                {
                    return false;
                }
            }
            return true;
        }

        //Returns true if Date1(start1, end1) and Date2(start2, end2) dont have any shared time
        private bool betweenD(DateTime start1, DateTime end1, DateTime start2, DateTime end2)
        {
            if(start1 > end2 || end1 < start2)
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
            if(concurrent < station.maxParallelUseable)
            {
                return true;
            }
            return false;
        }
    }
}
*/