using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using NUnit.Framework;
using Sopro.Models.Administration;
using Sopro.Models.Infrastructure;
using Sopro.Models.Simulation;
using System;
using System.Collections.Generic;
using System.IO;
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
            bookingCountPerDay = 10,
            vehicles = new List<Vehicle>() { v1, v2 },
            rushhours = new List<Rushhour>() {  },
            start = DateTime.Now.AddHours(1),
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

            foreach(Booking b in executedScenario.generatedBookings)
            {
                Console.WriteLine(b.startTime.ToString() + "\t" + b.endTime.ToString() + "\t" + b.plugs[0].ToString() + "\t" + b.id + "\t" + ((b.socEnd - b.socStart) * b.capacity / 2000).ToString());
            }

            await sim.run();

            foreach(Booking b in l.schedule.bookings)
            {
                Console.WriteLine(b.startTime.ToString() + "\t" + b.endTime.ToString() + "\t" + b.plugs[0].ToString() + "\t" + b.id + "\t" + ((b.socEnd - b.socStart) * b.capacity / 2000).ToString());
            }

            Assert.IsTrue(l.schedule.bookings.Count > 0);
            foreach (Booking b in l.schedule.bookings)
            {
                Assert.IsTrue(b.station != null);
            }
        }

 
    }
}
