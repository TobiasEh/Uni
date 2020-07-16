using NUnit.Framework;
using Sopro.Models.Simulation;
using System;
using System.Collections.Generic;
using Sopro.Models.Administration;
using Sopro.Interfaces;
using Sopro.Models.Infrastructure;
using NUnit.Framework.Internal;
using System.Linq;
using Sopro.Models.User;

namespace UnitTests.Simulation
{
    [TestFixture]
    class GeneratorTest
    {
        private ILocation location = new Location()
        {
            name = "Ludwigsburg",
            emergency = 30.2,
        };

        private Vehicle vehicle = new Vehicle()
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

        private Vehicle vehicle2 = new Vehicle()
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
        private Generator generator = new Generator();
        private int bookingsCountPerDay = 20;
        private int duration = 30;
        private Rushhour rushhour = new Rushhour() { start = DateTime.Now.AddDays(3), end = DateTime.Now.AddDays(3).AddHours(3), bookings = 10, strategy = new NormalDistribution() };

        [Test]
        public void generateBookingsBAttributesNotNullTest()
        {
            List<Vehicle> vehicles = new List<Vehicle>() { vehicle, vehicle2 };
            List<Booking> result = new List<Booking>();
            ExecutedScenario scenario = new ExecutedScenario()
            {
                bookingCountPerDay = bookingsCountPerDay, 
                duration = duration,
                location = location, 
                start = start,
                vehicles = vehicles,
                rushhours = new List<Rushhour>()
            };
            result = generator.generateBookings(scenario);
            Assert.IsTrue(result.All<Booking>(e => e.socEnd > 0 && e.socStart < 100 && e.plugs != null
                && e.startTime != null && e.endTime != null && e.user != null && e.capacity > 0 && e.location == location));
        }

        [Test]
        public void generateBookingsBAttributesNotNullTestWithRushhour()
        {
            List<Rushhour> rushhours = new List<Rushhour>() { rushhour };
            List<Vehicle> vehicles = new List<Vehicle>() { vehicle, vehicle2 };
            List<Booking> result = new List<Booking>();
            ExecutedScenario scenario = new ExecutedScenario()
            {
                bookingCountPerDay = bookingsCountPerDay,
                duration = duration,
                location = location,
                start = start,
                vehicles = vehicles,
                rushhours = rushhours
            };
            result = generator.generateBookings(scenario);
            Assert.IsTrue(result.All<Booking>(e => e.socEnd > 0 && e.socStart < 100 && e.plugs != null
                && e.startTime != null && e.endTime != null && e.user != null && e.capacity > 0 && e.location == location));
        }

        [Test]
        public void generateBookingsCount()
        {
            List<Vehicle> vehicles = new List<Vehicle>() { vehicle, vehicle2 };
            List<Booking> result = new List<Booking>();
            ExecutedScenario scenario = new ExecutedScenario() { bookingCountPerDay = bookingsCountPerDay, duration = duration, 
                location = location, start = start, vehicles = vehicles, rushhours = new List<Rushhour>()};
            result = generator.generateBookings(scenario);
            Assert.IsTrue(result.Count == bookingsCountPerDay * duration);
        }

        [Test]
        public void generateBookingsCountWithRushhour()
        {
            List<Rushhour> rushhours = new List<Rushhour>() { rushhour };
            List<Vehicle> vehicles = new List<Vehicle>() { vehicle, vehicle2 };
            List<Booking> result = new List<Booking>();
            ExecutedScenario scenario = new ExecutedScenario()
            {
                bookingCountPerDay = bookingsCountPerDay,
                duration = duration,
                location = location,
                start = start,
                vehicles = vehicles,
                rushhours = rushhours
            };
            result = generator.generateBookings(scenario);
            Assert.IsTrue(result.Count() == bookingsCountPerDay * duration);
        }

        [Test]
        public void generatedBookingsCountEmploye()
        {
            List<Vehicle> vehicles = new List<Vehicle>() { vehicle, vehicle2 };
            List<Booking> result = new List<Booking>();
            ExecutedScenario scenario = new ExecutedScenario() 
            { 
                bookingCountPerDay = bookingsCountPerDay, 
                duration = duration, 
                location = location, 
                start = start, 
                vehicles = vehicles,
                rushhours = new List<Rushhour>()
            };
            result = generator.generateBookings(scenario);
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
            ExecutedScenario scenario = new ExecutedScenario()
            {
                bookingCountPerDay = bookingsCountPerDay,
                duration = duration,
                location = location,
                start = start,
                vehicles = vehicles,
                rushhours = rushhours,
            };
            result = generator.generateBookings(scenario);
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
