﻿using NUnit.Framework;
using Sopro.Models.Administration;
using Sopro.Models.Infrastructure;
using Sopro.Models.Simulation;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace UnitTests.SimulationTest
{
    [TestFixture]
    class SimulatorTest
    {
        private static Vehicle v1 = new Vehicle()
        {
            model = "Porsche1",
            capacity = 100, 
            socStart = 20,
            socEnd = 100,
            plugs = new List<PlugType>() { PlugType.CCS }
        };

        private static TimeSpan oneDay = new TimeSpan(1, 0, 0, 0);

        private static Rushhour r1 = new Rushhour()
        {
            start = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 12, 0, 0) + oneDay,
            end = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 14, 0, 0) + oneDay,
            //bookings = 8,
            strategy = new NormalDistribution()
        };

        private static Vehicle v2 = new Vehicle()
        {
            model = "Porsche2",
            capacity = 100,
            socStart = 60,
            socEnd = 100,
            plugs = new List<PlugType>() { PlugType.TYPE2 }
        };

        static Plug p1 = new Plug
        {
            power = 20,
            type = PlugType.CCS
        };

        static Plug p2 = new Plug
        {
            power = 40,
            type = PlugType.TYPE2
        };

        static Plug p3 = new Plug
        {
            power = 50,
            type = PlugType.TYPE2
        };

        static Station s1 = new Station
        {
            plugs = new List<Plug> { p1 },
            maxPower = 200,
            manufacturer = "Tobee",
            maxParallelUseable = 1
        };

        static Station s2 = new Station
        {
            plugs = new List<Plug> { p3 },
            maxPower = 200,
            manufacturer = "Blergh",
            maxParallelUseable = 4
        };

        static Zone z1 = new Zone
        {
            stations = new List<Station> { s1 },
            site = 'A',
            maxPower = 1000
        };

        static Zone z2 = new Zone
        {
            stations = new List<Station> { s2 },
            site = 'B',
            maxPower = 1000
        };

        static Zone z3 = new Zone
        {
            stations = new List<Station> { s1, s2 },
            site = 'c',
            maxPower = 1000
        };

        private static Location l = new Location()
        {
            id = "locationidk",
            zones = new List<Zone>() { z1 },
            name = "Berlin",
            emergency = 0.05,
        };

        private static Location l2 = new Location()
        {
            id = "locationidk2",
            zones = new List<Zone>() { z2 },
            name = "München",
            emergency = 0.05,
        };

        private static Location l3 = new Location()
        {
            id = "locationidk3",
            zones = new List<Zone>() { z3 },
            name = "Candyland",
            emergency = 0.05,
        };

        private static Scenario scenario = new Scenario()
        {
            duration = 1,
            bookingCountPerDay = 15,
            vehicles = new List<Vehicle>() { v1, v2 },
            rushhours = new List<Rushhour>() {  },
            start = DateTime.Now.AddDays(1),
            location = l
        };

        private static Scenario scenariob = new Scenario()
        {
            duration = 1,
            bookingCountPerDay = 15,
            vehicles = new List<Vehicle>() { v1, v2 },
            rushhours = new List<Rushhour>() { r1 },
            start = DateTime.Now.AddDays(1),
            location = l
        };

        private static Scenario scenario2 = new Scenario()
        {
            duration = 1,
            bookingCountPerDay = 15,
            vehicles = new List<Vehicle>() { v1, v2 },
            rushhours = new List<Rushhour>() { },
            start = DateTime.Now.AddDays(1),
            location = l2
        };

        private static Scenario scenario3 = new Scenario()
        {
            duration = 1,
            bookingCountPerDay = 15,
            vehicles = new List<Vehicle>() { v1, v2 },
            rushhours = new List<Rushhour>() { },
            start = DateTime.Now.AddDays(1),
            location = l3
        };

        private static Scenario scenario3b = new Scenario()
        {
            duration = 1,
            bookingCountPerDay = 15,
            vehicles = new List<Vehicle>() { v1, v2 },
            rushhours = new List<Rushhour>() { r1 },
            start = DateTime.Now.AddDays(1),
            location = l3
        };

        private static Scenario scenario4 = new Scenario()
        {
            duration = 2,
            bookingCountPerDay = 150,
            vehicles = new List<Vehicle>() { v1, v2 },
            rushhours = new List<Rushhour>() { r1 },
            start = DateTime.Now.AddDays(1),
            location = l3
        };

        private static ExecutedScenario executedScenario = new ExecutedScenario(scenario);
        private static ExecutedScenario executedScenariob = new ExecutedScenario(scenariob);
        private static ExecutedScenario executedScenario2 = new ExecutedScenario(scenario2);
        private static ExecutedScenario executedScenario3 = new ExecutedScenario(scenario3);
        private static ExecutedScenario executedScenario3b = new ExecutedScenario(scenario3b);
        private static ExecutedScenario executedScenario4 = new ExecutedScenario(scenario4);

        [Test]
        public async Task testSimulatorCCSOnlyNoRushhour()
        {
            Simulator sim = new Simulator()
            {
                exScenario = executedScenario
            };

            printDetailedBookingList(executedScenario.generatedBookings, 20);
            await sim.run();
            Console.WriteLine("");
            printDetailedBookingList(executedScenario.location.schedule.bookings, 20);
            printWorkload(executedScenario);
            Console.WriteLine("FullfilledRequests:");
            Console.WriteLine(executedScenario.getFulfilledRequests());
            validateResults((Location)executedScenario.location, 20);
        }

        [Test]
        public async Task testSimulatorCCSOnly()
        {
            Simulator sim = new Simulator()
            {
                exScenario = executedScenariob
            };

            printDetailedBookingList(executedScenariob.generatedBookings, 20);
            await sim.run();
            Console.WriteLine("");
            printDetailedBookingList(executedScenariob.location.schedule.bookings, 20);
            printWorkload(executedScenariob);
            Console.WriteLine("FullfilledRequests:");
            Console.WriteLine(executedScenariob.getFulfilledRequests());
            validateResults((Location)executedScenariob.location, 20);
        }

        [Test]
        public async Task testSimulatorTypeTwoOnlyNoRushhour()
        {
            Simulator sim = new Simulator()
            {
                exScenario = executedScenario2
            };

            printDetailedBookingList(executedScenario2.generatedBookings, 50);
            await sim.run();
            Console.WriteLine("");
            printDetailedBookingList(executedScenario2.location.schedule.bookings, 50);
            printWorkload(executedScenario2);
            Console.WriteLine("FullfilledRequests:");
            Console.WriteLine(executedScenario2.getFulfilledRequests());
            validateResults((Location)executedScenario2.location, 50);
        }

        [Test]
        public async Task testSimulatorBothPlugsNoRushhour()
        {
            Simulator sim = new Simulator()
            {
                exScenario = executedScenario3
            };

            printDetailedBookingList(executedScenario3.generatedBookings, 0);
            await sim.run();
            Console.WriteLine("");
            printDetailedBookingList(executedScenario3.location.schedule.bookings, 0);
            printWorkload(executedScenario3);
            Console.WriteLine("FullfilledRequests:");
            Console.WriteLine(executedScenario3.getFulfilledRequests());
            validateResults((Location)executedScenario3.location, 0);
        }

        [Test]
        public async Task testSimulatorBothPlugsRushhour()
        {
            Simulator sim = new Simulator()
            {
                exScenario = executedScenario3b
            };

            printDetailedBookingList(executedScenario3b.generatedBookings, 0);
            await sim.run();
            Console.WriteLine("");
            printDetailedBookingList(executedScenario3b.location.schedule.bookings, 0);
            printWorkload(executedScenario3b);
            Console.WriteLine("FullfilledRequests:");
            Console.WriteLine(executedScenario3b.getFulfilledRequests());
            validateResults((Location)executedScenario3b.location, 0);

        }

        [Test]
        public async Task testSimulatorMoreThanOneTick()
        {
            Simulator sim = new Simulator()
            {
                exScenario = executedScenario4
            };

            
            await sim.run();
            
            printWorkload(executedScenario4);
            validateResults((Location)executedScenario4.location, 0);

        }

        private static void validateResults(Location l, int power)
        {
            Assert.IsTrue(l.schedule.bookings.Count > 0);
            foreach (Booking b in l.schedule.bookings)
            {
                Assert.IsTrue(b.station != null);

                if (power > 0)
                {
                    int chargeDuration = (b.socEnd - b.socStart) * b.capacity / (100 * power);
                    TimeSpan bookingDuration = b.endTime - b.startTime;
                    // User should be able to fulfill their request.
                    Assert.IsTrue(bookingDuration.Hours >= chargeDuration);
                    // Booking should not take longer than whatever is necessary to fulfill the request and one hour.
                    Assert.IsTrue(bookingDuration.Hours <= chargeDuration + 1);
                    
                }
            }
        }

        private static void printDetailedBookingList(List<Booking> bookings, int power)
        {
            foreach (Booking b in bookings)
            {
                string timeDetail = b.startTime.ToString() + "\t" + b.endTime.ToString() + "\t";
                string duration = power == 0 ? "" : ((b.socEnd - b.socStart) * b.capacity / (100 * power)).ToString();
                string chargeDetail = b.plugs[0].ToString() + "\t" + duration + "\t";
                
                Console.WriteLine(timeDetail + chargeDetail + b.id + "\t" + b.priority);
            }
        }

        private static void printWorkload(ExecutedScenario executedScenario) 
        {
            Console.WriteLine("FullfilledRequests:");
            Console.WriteLine(executedScenario.getFulfilledRequests());
            Console.WriteLine("LocationWorkload: \t Count:" + executedScenario.getLocationWorkload().Count);
            foreach (double item in executedScenario.getLocationWorkload())
            {
                Console.WriteLine(item);
            }

            Console.WriteLine("StationWorkload: \t CountLists:" + executedScenario.getStationWorkload().Count);
            Console.WriteLine("StationWorkloads:");
            foreach (List<double> list in executedScenario.getStationWorkload())
            {
                Console.WriteLine("NewStation:");
                foreach (double item in executedScenario.getLocationWorkload())
                {
                    Console.WriteLine(item);
                }
            }

        }
 
    }
}
