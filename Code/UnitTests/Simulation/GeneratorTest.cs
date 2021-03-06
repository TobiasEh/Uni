﻿/*using NUnit.Framework;
using NUnit.Framework.Internal;
using Sopro.Models.Administration;
using Sopro.Models.Infrastructure;
using Sopro.Models.Simulation;
using Sopro.Models.User;
using System;
using System.Collections.Generic;
using System.Linq;

namespace UnitTests.Simulation
{
    [TestFixture]
    class GeneratorTest
    {
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

        private static Location location = new Location()
        {
            id = "locationidk",
            zones = new List<Zone>() { z1 },
            name = "Berlin",
            emergency = 0.05,
        };

        private static Vehicle vehicle = new Vehicle()
        {
            capacity = 120,
            model = "model",
            socStart = 24,
            socEnd = 50,
            plugs = new List<PlugType>()
            {
                PlugType.CCS
            }
        };

        private static Vehicle vehicle2 = new Vehicle()
        {
            capacity = 140,
            model = "model2",
            socStart = 60,
            socEnd = 100,
            plugs = new List<PlugType>()
            {
                PlugType.CCS
            }
        };

        private DateTime start = DateTime.Now.AddDays(2).AddHours(1).AddMinutes(4);
        private int bookingsCountPerDay = 20;
        private int duration = 30;
        
        private static Rushhour rushhour = new Rushhour() 
        { 
            start = DateTime.Now.AddDays(3), 
            end = DateTime.Now.AddDays(3).AddHours(3), 
            bookings = 10, strategy = new NormalDistribution() 
        };
        
        private static Rushhour rushhour2 = new Rushhour() 
        { 
            start = DateTime.Now.AddDays(4).AddHours(2), 
            end = DateTime.Now.AddDays(4).AddHours(5), 
            bookings = 10, strategy = new NormalDistribution() 
        };

        private static Rushhour rushhour3 = new Rushhour() 
        { 
            start = DateTime.Now.AddDays(3).AddHours(2), 
            end = DateTime.Now.AddDays(3).AddHours(5), 
            bookings = 11, strategy = new NormalDistribution() 
        };

        private static Rushhour rushhour4 = new Rushhour() 
        { 
            start = DateTime.Now.AddDays(3).AddHours(2), 
            end = DateTime.Now.AddDays(3).AddHours(5), 
            bookings = 5, 
            strategy = new NormalDistribution() 
        };

        [Test]
        public void generateBookingsBAttributesNotNullTest()
        {
            List<Vehicle> vehicles = new List<Vehicle>() { vehicle, vehicle2 };
            List<Booking> result = new List<Booking>();

            Scenario s = new Scenario()
            {
                bookingCountPerDay = bookingsCountPerDay,
                duration = duration,
                location = location,
                start = start,
                vehicles = vehicles,
                rushhours = new List<Rushhour>()
            };

            ExecutedScenario scenario = new ExecutedScenario(s);

            result = Generator.generateBookings(scenario);
            Assert.IsTrue(result.All<Booking>(e => e.socEnd > 0 && e.socStart < 100 && e.plugs != null
                && e.startTime != null && e.endTime != null && e.user != null && e.capacity > 0 && e.location == location));
        }

        [Test]
        public void generateBookingsBAttributesNotNullTestWithRushhour()
        {
            List<Rushhour> rushhours = new List<Rushhour>() { rushhour };
            List<Vehicle> vehicles = new List<Vehicle>() { vehicle, vehicle2 };
            List<Booking> result = new List<Booking>();

            Scenario s = new Scenario
            {
                bookingCountPerDay = bookingsCountPerDay,
                duration = duration,
                location = location,
                start = start,
                vehicles = vehicles,
                rushhours = rushhours
            };

            ExecutedScenario scenario = new ExecutedScenario(s);

            result = Generator.generateBookings(scenario);
            Assert.IsTrue(result.All<Booking>(e => e.socEnd > 0 && e.socStart < 100 && e.plugs != null
                && e.startTime != null && e.endTime != null && e.user != null && e.capacity > 0 && e.location == location));
        }
        [Test]
        public void generateBookingsBAttributesNotNullTestWit2Rushhour()
        {
            List<Rushhour> rushhours = new List<Rushhour>() { rushhour, rushhour2 };
            List<Vehicle> vehicles = new List<Vehicle>() { vehicle, vehicle2 };
            List<Booking> result = new List<Booking>();
            Scenario s = new Scenario
            {
                bookingCountPerDay = bookingsCountPerDay,
                duration = duration,
                location = location,
                start = start,
                vehicles = vehicles,
                rushhours = rushhours
            };

            ExecutedScenario scenario = new ExecutedScenario(s);

            result = Generator.generateBookings(scenario);
            Assert.IsTrue(result.All<Booking>(e => e.socEnd > 0 && e.socStart < 100 && e.plugs != null
                && e.startTime != null && e.endTime != null && e.user != null && e.capacity > 0 && e.location == location));
        }

        [Test]
        public void generateBookingsBAttributesNotNullTestWitRushhourWithLowerBookingsThanBookingCountPerDay()
        {
            List<Rushhour> rushhours = new List<Rushhour>() { rushhour4 };
            List<Vehicle> vehicles = new List<Vehicle>() { vehicle, vehicle2 };
            List<Booking> result = new List<Booking>();
            Scenario s = new Scenario
            {
                bookingCountPerDay = bookingsCountPerDay,
                duration = duration,
                location = location,
                start = start,
                vehicles = vehicles,
                rushhours = rushhours
            };

            ExecutedScenario scenario = new ExecutedScenario(s);

            result = Generator.generateBookings(scenario);
            Assert.IsTrue(result.All<Booking>(e => e.socEnd > 0 && e.socStart < 100 && e.plugs != null
                && e.startTime != null && e.endTime != null && e.user != null && e.capacity > 0 && e.location == location));
        }

        [Test]
        public void generateBookingsCount()
        {
            List<Vehicle> vehicles = new List<Vehicle>() { vehicle, vehicle2 };
            List<Booking> result = new List<Booking>();
            Scenario s = new Scenario 
            { 
                bookingCountPerDay = bookingsCountPerDay, 
                duration = duration, 
                location = location, 
                start = start, 
                vehicles = vehicles, 
                rushhours = new List<Rushhour>()};

            ExecutedScenario scenario = new ExecutedScenario(s);

            result = Generator.generateBookings(scenario);
            Assert.IsTrue(result.Distinct().Count() == bookingsCountPerDay * duration);
        }

        [Test]
        public void generateBookingsCountWithRushhour()
        {
            List<Rushhour> rushhours = new List<Rushhour>() { rushhour };
            List<Vehicle> vehicles = new List<Vehicle>() { vehicle, vehicle2 };
            List<Booking> result = new List<Booking>();
            Scenario s = new Scenario
            {
                bookingCountPerDay = bookingsCountPerDay,
                duration = duration,
                location = location,
                start = start,
                vehicles = vehicles,
                rushhours = rushhours
            };

            ExecutedScenario scenario = new ExecutedScenario(s);

            result = Generator.generateBookings(scenario);
            Assert.IsTrue(result.Count() == bookingsCountPerDay * duration);
        }

        [Test]
        public void generateBookingsCountWith2Rushhours()
        {
            List<Rushhour> rushhours = new List<Rushhour>() { rushhour, rushhour2 };
            List<Vehicle> vehicles = new List<Vehicle>() { vehicle, vehicle2 };
            List<Booking> result = new List<Booking>();
            Scenario s = new Scenario
            {
                bookingCountPerDay = bookingsCountPerDay,
                duration = duration,
                location = location,
                start = start,
                vehicles = vehicles,
                rushhours = rushhours
            };

            ExecutedScenario scenario = new ExecutedScenario(s);

            result = Generator.generateBookings(scenario);
            Console.WriteLine(result.Count() + " " + bookingsCountPerDay*duration);
            Assert.IsTrue(result.Count() == bookingsCountPerDay * duration);
        }

        [Test]
        public void generateBookingsCountWith2RushhoursSameDayMoreBookingsThanBookingCountPerDay()
        {
            List<Rushhour> rushhours = new List<Rushhour>() { rushhour, rushhour3 };
            List<Vehicle> vehicles = new List<Vehicle>() { vehicle, vehicle2 };
            List<Booking> result = new List<Booking>();
            Scenario s = new Scenario
            {
                bookingCountPerDay = bookingsCountPerDay,
                duration = duration,
                location = location,
                start = start,
                vehicles = vehicles,
                rushhours = rushhours
            };
            ExecutedScenario scenario = new ExecutedScenario(s);

            result = Generator.generateBookings(scenario);

            Assert.IsTrue(result.Count() == bookingsCountPerDay * duration);
        }

        [Test]
        public void generateBookingsCountWithRushhoursBookingsLowerThanBookingsCountPerDay()
        {
            List<Rushhour> rushhours = new List<Rushhour>() { rushhour4 };
            List<Vehicle> vehicles = new List<Vehicle>() { vehicle, vehicle2 };
            List<Booking> result = new List<Booking>();
            Scenario s = new Scenario
            {
                bookingCountPerDay = bookingsCountPerDay,
                duration = duration,
                location = location,
                start = start,
                vehicles = vehicles,
                rushhours = rushhours
            };

            ExecutedScenario scenario = new ExecutedScenario(s);

            result = Generator.generateBookings(scenario);
            Console.WriteLine(result.Distinct().Count() + " " + result.Count() + " " + (bookingsCountPerDay * duration));
            Assert.IsTrue(result.Distinct().Count() == bookingsCountPerDay * duration);
        }

        [Test]
        public void generatedBookingsCountEmploye()
        {
            List<Vehicle> vehicles = new List<Vehicle>() { vehicle, vehicle2 };
            List<Booking> result = new List<Booking>();
            Scenario s = new Scenario 
            { 
                bookingCountPerDay = bookingsCountPerDay, 
                duration = duration, 
                location = location, 
                start = start, 
                vehicles = vehicles,
                rushhours = new List<Rushhour>()
            };

            ExecutedScenario scenario = new ExecutedScenario(s);

            result = Generator.generateBookings(scenario);
            double numberOfBooking = bookingsCountPerDay * duration;
            double countEmployee = result.Count(e => e.priority.Equals(UserType.EMPLOYEE));
            double countVIP = result.Count(e => e.priority.Equals(UserType.VIP));
            double countGuest = result.Count(e => e.priority.Equals(UserType.GUEST));
            double percentageEmployee = countEmployee / numberOfBooking;
            double percentageVIP = countVIP  / numberOfBooking;
            double percentageGuest = countGuest / numberOfBooking;

            Assert.IsTrue(percentageEmployee > percentageGuest + percentageVIP && percentageEmployee > 0.1);
        }

        [Test]
        public void generatedBookingsCountEmployeeWithRushhours()
        {
            List<Rushhour> rushhours = new List<Rushhour>() { rushhour };
            List<Vehicle> vehicles = new List<Vehicle>() { vehicle, vehicle2 };
            List<Booking> result = new List<Booking>();
            Scenario s = new Scenario
            {
                bookingCountPerDay = bookingsCountPerDay,
                duration = duration,
                location = location,
                start = start,
                vehicles = vehicles,
                rushhours = rushhours,
            };

            ExecutedScenario scenario = new ExecutedScenario(s);

            result = Generator.generateBookings(scenario);
            double numberOfBooking = bookingsCountPerDay * duration;
            double countEmployee = result.Count(e => e.priority.Equals(UserType.EMPLOYEE));
            double countVIP = result.Count(e => e.priority.Equals(UserType.VIP));
            double countGuest = result.Count(e => e.priority.Equals(UserType.GUEST));
            double percentageEmployee = countEmployee / numberOfBooking;
            double percentageVIP = countVIP / numberOfBooking;
            double percentageGuest = countGuest / numberOfBooking;
            Assert.IsTrue(percentageEmployee > percentageGuest + percentageVIP && percentageEmployee > 0.1);
        }
    }
}
*/