using Sopro.Models.Administration;
using Sopro.Models.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;

namespace Sopro.Models.Simulation
{
    public static class Generator
    {
        public static double probabilityVIP { get; set; } = 0.05;
        public static double probabilityGUEST { get; set; } = 0.05;
        public static double probabilityADHOC { get; set; } = 0.10;

        private static Random gen = new Random();
        private static int startHour = 6;
        private static int endHour = 20;

        public static List<Booking> generateBookings(Scenario scenario)
        {
            Console.WriteLine("duration in days : {0}", (scenario.duration * Simulator.tickLength.TotalSeconds) / (24 * 60 * 60));

            if ((scenario.duration * Simulator.tickLength.TotalSeconds) / (24 * 60 * 60) < 1) return distributeLessThanDay(scenario);

            double totalSeconds = scenario.duration * Simulator.tickLength.TotalSeconds;
            double totalDays = totalSeconds / (24 * 60 * 60);
            int duration = (int) totalDays;
            double leftover = totalDays % 1;
            List<Booking> bookingList = new List<Booking>();

            for (int i = 0; i < duration; i++)
            {
                bool exists = false;
                if (scenario.rushhours.Count != 0)
                {
                    int dailyBookingCount = scenario.bookingCountPerDay;
                    DateTime minRushhourStart = new DateTime();

                    foreach (Rushhour rushhour in scenario.rushhours.Where(x => x.start.Day == scenario.start.AddDays(i).Day))
                    {
                        exists = true;
                        minRushhourStart = minRushhourStart == new DateTime() ? rushhour.start : minRushhourStart > rushhour.start ? rushhour.start : minRushhourStart;
                        var startTimes = rushhour.run();
                        if (rushhour.bookings > dailyBookingCount)
                        {
                            startTimes.RemoveRange(dailyBookingCount - 1 < 0 ? 0 : dailyBookingCount - 1, rushhour.run().Count - dailyBookingCount);
                        }
                        if (startTimes.Count != 0)
                            foreach (DateTime start in startTimes)
                            {
                                bookingList.Add(generateBooking(start, start.AddHours(gen.Next(1, 8)), scenario));
                                dailyBookingCount--;
                            };
                    }
                    if (dailyBookingCount != 0 && exists == true)
                    {
                        List<Booking> linDistBookings = new List<Booking>(); ;
                        linearBookingDistribution(linDistBookings, scenario, i);
                        linDistBookings.RemoveRange(linDistBookings.FindIndex(x => x.startTime >= minRushhourStart), scenario.bookingCountPerDay - dailyBookingCount);
                        bookingList.AddRange(linDistBookings);
                    }
                    if (exists == false) linearBookingDistribution(bookingList, scenario, i);
                }
                else
                {
                    linearBookingDistribution(bookingList, scenario, i);
                }
            }
            if (leftover != 0 && leftover * 24 * 60 >= (24 * 60) / scenario.bookingCountPerDay)
            {
                bookingList.AddRange(distributeLessThanDay(scenario));
            }

            return bookingList;
        }

        private static List<Booking> distributeLessThanDay(Scenario scenario)
        {
            List<Booking> bookingList = new List<Booking>();
            double leftover = (scenario.duration * Simulator.tickLength.TotalSeconds / 24 * 60 * 60) % 1;
            int dailyBookingCount = (int)(scenario.bookingCountPerDay * leftover);

            if (scenario.rushhours.FindAll(x => x.start >= scenario.start && x.start <= scenario.start.AddSeconds(scenario.duration * Simulator.tickLength.TotalSeconds)).Count != 0)
            {
                DateTime minRushhourStart = new DateTime();
                foreach (Rushhour rh in scenario.rushhours.Where(x => x.start >= scenario.start.AddDays((int)(scenario.duration * Simulator.tickLength.TotalSeconds / 24 * 60 * 60)) && x.start <= scenario.start.AddSeconds(scenario.duration * Simulator.tickLength.TotalSeconds)))
                {
                    minRushhourStart = minRushhourStart == new DateTime() ? rh.start : minRushhourStart > rh.start ? rh.start : minRushhourStart;
                    List<DateTime> startTimes = rh.run();
                    if (rh.end > scenario.start.AddSeconds(scenario.duration * Simulator.tickLength.TotalSeconds))
                    {
                        startTimes.RemoveAll(x => x > scenario.start.AddSeconds(scenario.duration * Simulator.tickLength.TotalSeconds));
                    }
                    if (startTimes.Count != 0)
                        foreach (DateTime start in startTimes)
                        {
                            bookingList.Add(generateBooking(start, start.AddHours(gen.Next(1, 8)), scenario));
                            dailyBookingCount--;
                            if (dailyBookingCount <= 0) break;
                        };
                    if (dailyBookingCount <= 0) break;
                }
                if (dailyBookingCount < 0)
                {
                    List<Booking> linDistBookings = new List<Booking>(); ;
                    linearBookingDistribution(linDistBookings, scenario, 0);
                    linDistBookings.RemoveRange(linDistBookings.FindIndex(x => x.startTime >= minRushhourStart), scenario.bookingCountPerDay - Math.Abs(dailyBookingCount));
                    bookingList.AddRange(linDistBookings);
                }
            }
            else
            {
                linearBookingDistribution(bookingList, scenario, 0);
                bookingList.RemoveAll(x => x.startTime > scenario.start.AddSeconds(scenario.duration * Simulator.tickLength.TotalSeconds) && x.startTime < scenario.start.AddDays((int)(scenario.duration * Simulator.tickLength.TotalSeconds / 24 * 60 * 60)));
                bookingList.RemoveRange(bookingList.Count - 1, bookingList.Count - dailyBookingCount);
            }
            return bookingList;
        }


        /// <summary>
        /// Roll user priority.
        /// </summary>
        /// <returns>Random user role.</returns>
        private static UserType rollPriority()
        {
            double probability = gen.NextDouble();

            if (probability > probabilityVIP + probabilityGUEST) return UserType.EMPLOYEE;
            if (probability > probabilityGUEST) return UserType.VIP;
            return UserType.GUEST;
        }

        /// <summary>
        /// Generates a booking using given values.
        /// </summary>
        /// <param name="startTime">Point of time where charging could begin.</param>
        /// <param name="endTime">Point of time where charging should be completed.</param>
        /// <param name="scenario">Simulated scenario.</param>
        /// <returns>A generated booking.</returns>
        private static Booking generateBooking(DateTime startTime, DateTime endTime, Scenario scenario)
        {
            int random = gen.Next(scenario.vehicles.Count);
            return new Booking
            {
                capacity = scenario.vehicles[random].capacity,
                plugs = scenario.vehicles[random].plugs,
                socEnd = scenario.vehicles[random].socEnd,
                socStart = scenario.vehicles[random].socStart,
                user = "",
                startTime = startTime,
                endTime = endTime,
                station = null,
                active = false,
                priority = rollPriority(),
                location = scenario.location
            };                     
        }

        /// <summary>
        /// Fills a given list with evenly distributed and generated bookings. 
        /// </summary>
        /// <param name="bookingList">A list of bookings to be filled.</param>
        /// <param name="scenario">The scenario being simulated.</param>
        /// <param name="passedDays">How many days have passed in the simulation.</param>
        public static void linearBookingDistribution(List<Booking> bookingList, Scenario scenario, int passedDays)
        {
            // Create an equal amount of linearly distributed bookings for each simulated day
            for (int j = 0; j < scenario.bookingCountPerDay; j++)
            {
                DateTime currentDay = scenario.start.AddDays(passedDays);
                // Booking request is set this many minutes after startHour
                double minutesUntilRequest = (((startHour - endHour) * 60) / scenario.bookingCountPerDay) * j;

                DateTime start = currentDay.AddHours(startHour).AddMinutes(minutesUntilRequest);
                // Length of booking timespan equals min(8, hours until midnight)
                DateTime end = start.AddHours(gen.Next(2, Math.Min(8, 24 - start.Hour)));

                Booking b = generateBooking(start, end, scenario);
                bookingList.Add(b);
            }
        }
    }
}
