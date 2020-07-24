using Sopro.Models.Administration;
using Sopro.Models.Infrastructure;
using Sopro.Models.User;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Sopro.Interfaces.AdministrationSimulation;
using System.Data;

namespace Sopro.Models.Simulation
{
    public static class Generator
    {
        [Range(0, 1)]
        public static double probabilityVIP { get; set; } = 0.05;
        [Range(0, 1)]
        public static double probabilityGUEST { get; set; } = 0.05;

        private static Random rand = new Random();

        public static List<Booking> generateBookings(Scenario scenario)
        {
            // Sets duration in Days.
            int duration = (int)(scenario.duration * Simulator.tickLength.TotalDays);
            List<Booking> bookingList = new List<Booking>();
            // Iterates each day of the duration of the scenario.
            for (int i = 0; i < duration; i++)
            {
                bool exists = false;
                int bookingCountPerDayCopy = scenario.bookingCountPerDay;
                DateTime rushhourStartTimeMinimum = new DateTime();
                foreach (Rushhour rushhour in scenario.rushhours.Where(x => x.start.Day == scenario.start.AddDays(i).Day))
                {
                    // Sets exists to true, stating a rushhour was found for the given day.
                    exists = true;
                    // Sets the smallest start time of "all" rushhours occuring the given day. If there was none a new one will be initiated,
                    // in case there is a newer, smaller rushhour start time it will be set to the smaller.
                    rushhourStartTimeMinimum = rushhourStartTimeMinimum == new DateTime() ? rushhour.start : rushhourStartTimeMinimum > rushhour.start ? rushhour.start : rushhourStartTimeMinimum;
                    // Generates satring times for the synthetic bookings.
                    var startTimes = rushhour.run();
                    // If the rushhour should be producing more bookings than the bookingsPerDay cap it will be set to latter.
                    if (rushhour.bookings > bookingCountPerDayCopy)
                    {
                        startTimes.RemoveRange(bookingCountPerDayCopy - 1 < 0 ? 0 : bookingCountPerDayCopy - 1, rushhour.run().Count - bookingCountPerDayCopy);

                    }
                    if (startTimes.Count != 0)
                        // Adds the Booking with start Times of each item in startTimes with random vehicles for capacity, plugs, socEnd, socStart.
                        // And random end Time in an intervall between 1 and 7 with a random priority of the booking.
                        foreach (DateTime start in startTimes)
                        {
                            int r = rand.Next(scenario.vehicles.Count);
                            bookingList.Add(
                                new Booking
                                {
                                    capacity = scenario.vehicles[r].capacity,
                                    plugs = scenario.vehicles[r].plugs,
                                    socEnd = scenario.vehicles[r].socEnd,
                                    socStart = scenario.vehicles[r].socStart,
                                    user = "megarandombookinggeneratorduud",
                                    startTime = start,
                                    endTime = start.AddHours(rand.Next(1, 8)),
                                    station = null,
                                    active = false,
                                    priority = setPrio(),
                                    location = scenario.location
                                }
                                ); ;
                            bookingCountPerDayCopy--;
                        };
                }
                // Fills up bookingList to match bookingCountPerDay.
                if (bookingCountPerDayCopy != 0 && exists == true)
                {
                    // Generates a temporary list og linear distributed bookings
                    List<Booking> linBList = new List<Booking>(); ;
                    LinearDist(linBList, scenario, i);
                    // And removes {count of rushhour bookings} linear distributed bookings starting at the first rushhour generated  
                    linBList.RemoveRange(linBList.FindIndex(x => x.startTime >= rushhourStartTimeMinimum), scenario.bookingCountPerDay - bookingCountPerDayCopy);
                    bookingList.AddRange(linBList);
                }
                // Fills up bookingList if there was no russhhour on that day.
                if (exists == false) LinearDist(bookingList, scenario, i);
            }
            return bookingList;
        }


        // Sets Priority of synthetic bookings randomly with given probabilities.
        private static UserType setPrio()
        {
            double prob = rand.NextDouble();
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

        // Generates Bookings, distributed linearily.
        // Granularity in 'Minutes'.
        public static void LinearDist(List<Booking> bookingList, Scenario scenario, int i)
        {
            for (int j = 0; j < scenario.bookingCountPerDay; j++)
            {
                int r = rand.Next(scenario.vehicles.Count);
                bookingList.Add(new Booking
                {
                    capacity = scenario.vehicles[r].capacity,
                    plugs = scenario.vehicles[r].plugs,
                    socEnd = scenario.vehicles[r].socEnd,
                    socStart = scenario.vehicles[r].socStart,
                    user = "linDuuudOida",
                    startTime = scenario.start.AddDays(i).AddMinutes(((24 * 60) / scenario.bookingCountPerDay) * j),
                    endTime = scenario.start.AddDays(i).AddMinutes(((24 * 60) / scenario.bookingCountPerDay) * j).AddHours(rand.Next(1, 8)),
                    station = null,
                    active = false,
                    priority = setPrio(),
                    location = scenario.location
                }
                );
            }
        }


    }
}
