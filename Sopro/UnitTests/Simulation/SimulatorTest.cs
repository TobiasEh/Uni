using NUnit.Framework;
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

        private static Rushhour r1 = new Rushhour()
        {
            start = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day + 1, 12, 0, 0),
            end = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day + 1, 14, 0, 0),
            bookings = 8,
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

        private static Location l = new Location()
        {
            id = "locationidk",
            zones = new List<Zone>() { z1 },
            name = "Berlin",
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

        private static ExecutedScenario executedScenario = new ExecutedScenario(scenario);

        [Test]
        public async Task testSimulator()
        {
            Simulator sim = new Simulator()
            {
                exScenario = executedScenario
            };

            printDetailedBookingList(executedScenario.generatedBookings);
            await sim.run();
            printDetailedBookingList(l.schedule.bookings);

            Assert.IsTrue(l.schedule.bookings.Count > 0);
            foreach (Booking b in l.schedule.bookings) 
            {
                int chargeDuration = (b.socEnd - b.socStart) * b.capacity / 2000;
                TimeSpan bookingDuration = b.endTime - b.startTime;
                Assert.IsTrue(b.station != null); 
                // User should be able to fulfill their request.
                Assert.IsTrue(bookingDuration.Hours >= chargeDuration);
                // Booking should not take longer than whatever is necessary to fulfill the request and one hour.
                Assert.IsTrue(bookingDuration.Hours < chargeDuration + 1);
            }
        }

        private static void printDetailedBookingList(List<Booking> bookings)
        {
            foreach (Booking b in bookings)
            {
                string timeDetail = b.startTime.ToString() + "\t" + b.endTime.ToString() + "\t";
                string chargeDetail = b.plugs[0].ToString() + "\t" + ((b.socEnd - b.socStart) * b.capacity / 2000).ToString() + "\t";
                Console.WriteLine(timeDetail + chargeDetail + b.id + "\t" + b.priority);
            }
        }

 
    }
}
