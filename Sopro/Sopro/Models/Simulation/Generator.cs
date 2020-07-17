using Sopro.Models.Administration;
using Sopro.Models.Infrastructure;
using Sopro.Models.User;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Sopro.Interfaces.AdministrationSimulation;
using System.Data;
/*
namespace Sopro.Models.Simulation
{
    public static class Generator 
    {
        public static double probabilityVIP { get; set; } = 0.05;
        public static double probabilityGUEST { get; set; } = 0.05;

        public static List<Booking> generateBookings(Scenario scenario) 
        {
            List<Booking> bookingList = new List<Booking>();
            for(int i = 0; i < scenario.duration; i++) //days
            {
               bool exists = false;
                if (scenario.rushhours.Count != 0)
                {
                    int bpdC = scenario.bookingCountPerDay;
                    DateTime minRHstart = new DateTime();
                    foreach (Rushhour rh in scenario.rushhours.Where(x => x.start.Day == scenario.start.AddDays(i).Day))
                    {
                        exists = true;
                        minRHstart = minRHstart == new DateTime() ? rh.start : minRHstart > rh.start ? rh.start : minRHstart;
                        var startTimes = rh.run();
                        if (rh.bookings > bpdC)
                        {
                            startTimes.RemoveRange(bpdC - 1 < 0 ? 0 : bpdC - 1, rh.run().Count -bpdC);
                            
                        }
                        if (startTimes.Count != 0)
                        foreach (DateTime start in startTimes)
                        {
                            int r = new Random().Next(scenario.vehicles.Count);
                            bookingList.Add(
                                new Booking
                                {
                                    capacity = scenario.vehicles[r].capacity,
                                    plugs = scenario.vehicles[r].plugs,
                                    socEnd = scenario.vehicles[r].socEnd,
                                    socStart = scenario.vehicles[r].socStart,
                                    user = "",
                                    startTime = start,
                                    endTime = start.AddHours(new Random().Next(1, 8)),
                                    station = null,
                                    active = false,
                                    priority = setPrio(),
                                    location = scenario.location
                                }
                                ); ;
                            bpdC--;
                        };
                    }
                    if (bpdC != 0 && exists == true)
                    {
                        List<Booking> linBList = new List<Booking>();
                        LinearDist(linBList, scenario, i);
                        linBList.RemoveRange(linBList.FindIndex(x => x.startTime >= minRHstart), scenario.bookingCountPerDay-bpdC);
                        bookingList.AddRange(linBList);
                    }
                    if (exists == false) LinearDist(bookingList, scenario, i);
                        

                }
                else
                {
                    LinearDist(bookingList,scenario,i);
                }
            }
                

            return bookingList;
        }

        private static UserType setPrio()
        {
            double prob = new Random().NextDouble();
            if (prob > probabilityVIP + probabilityGUEST)
            {
                return UserType.EMPLOYEE;
            }
            else
            {
                if (prob > probabilityGUEST)
                {
                    return UserType.VIP;
                }
                else
                {
                    return UserType.GUEST;
                }
            }
        }

        public static void LinearDist(List<Booking> bookingList, Scenario scenario, int i)
        {
            int r = new Random().Next(scenario.vehicles.Count);

            for (int j = 0; j < scenario.bookingCountPerDay; j++)
            {
                bookingList.Add(new Booking
                {
                    capacity = scenario.vehicles[r].capacity,
                    plugs = scenario.vehicles[r].plugs,
                    socEnd = scenario.vehicles[r].socEnd,
                    socStart = scenario.vehicles[r].socStart,
                    user = "",
                    startTime = scenario.start.AddDays(i).AddMinutes((24 * 60) / scenario.bookingCountPerDay),
                    endTime = scenario.start.AddDays(i).AddHours(j).AddHours(new Random().Next(1, 8)),
                    station = null,
                    active = false,
                    priority = setPrio(),
                    location = scenario.location
                });
            }
        }
        

    }
}
*/

namespace Sopro.Models.Simulation
{
    public static class Generator
    {
        public static double probabilityVIP { get; set; } = 0.05;
        public static double probabilityGUEST { get; set; } = 0.05;
        public static double probabilityADHOC { get; set; } = 0.10;

        public static List<Booking> generateBookings(Scenario scenario)
        {
            List<Booking> generatedBookings = new List<Booking>();

            // Generate n bookings for each simulated tick
            for (int i = 0; i < scenario.duration; ++i)
            {

            }

            return generatedBookings;
        }
    }

}