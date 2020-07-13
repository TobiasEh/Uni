using Sopro.Models.Administration;
using Sopro.Models.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using Sopro.Interfaces.AdministrationSimulation;

namespace Sopro.Models.Simulation
{
    public class Generator : IGenerator
    {
        public List<Booking> generateBookings(Scenario scenario) 
        {
            List<Booking> bookingList = new List<Booking>();
            for(int i = 0; i< scenario.duration; i++) //days
            {
                bool exists = false;
                if (scenario.rushhours.Count != 0)
                {
                    foreach (Rushhour rh in scenario.rushhours.Where(x => x.start.Day >= scenario.start.Day && x.start.Day == scenario.start.AddDays(i).Day))
                    {
                        exists = true;
                        var startTimes = rh.run();
                        if (rh.bookings > scenario.bookingCountPerDay)
                        {
                            startTimes.RemoveRange(scenario.bookingCountPerDay - 1, rh.run().Count - scenario.bookingCountPerDay);
                        }
                        foreach (DateTime start in startTimes)
                        {
                            int r = new Random().Next(scenario.vehicles.Count);
                            bookingList.Add(
                                new Booking
                                {
                                    capacity = scenario.vehicles[r].capacity,
                                    plugs = new List<PlugType>() { scenario.vehicles[r].plugs.type },
                                    socEnd = scenario.vehicles[r].socEnd,
                                    socStart = scenario.vehicles[r].socStart,
                                    user = "megarandombookinggeneratorduud",
                                    startTime = start,
                                    endTime = start.AddHours(new Random().Next(1, 8)),
                                    station = null,
                                    active = false,
                                    priority = User.UserType.ASSISTANCE,
                                    location = scenario.location
                                }
                                );
                        }
                        
                    }
                    if(exists == false)
                        LinearDist(bookingList, scenario, i);

                }
                else
                {
                    LinearDist(bookingList,scenario,i);
                    

                    // fill with bookings linear
                }
            }
            

            return new List<Booking>();
        }
        public void LinearDist(List<Booking> bookingList, Scenario scenario, int i)
        {
            int r = new Random().Next(scenario.vehicles.Count);
            for (int j = 0; j < scenario.bookingCountPerDay; j++)
            {
                bookingList.Add(new Booking
                {
                    capacity = scenario.vehicles[r].capacity,
                    plugs = new List<PlugType>() { scenario.vehicles[r].plugs.type },
                    socEnd = scenario.vehicles[r].socEnd,
                    socStart = scenario.vehicles[r].socStart,
                    user = "megarandombookinggeneratorduud",
                    startTime = scenario.start.AddDays(i).AddMinutes((24 * 60) / scenario.bookingCountPerDay),
                    endTime = scenario.start.AddDays(i).AddHours(j).AddHours(new Random().Next(1, 8)),
                    station = null,
                    active = false,
                    priority = User.UserType.ASSISTANCE,
                    location = scenario.location
                }
                );
            }
            
        }

    }
}
