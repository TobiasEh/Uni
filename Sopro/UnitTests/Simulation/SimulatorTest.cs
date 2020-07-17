using NUnit.Framework;
using Sopro.Interfaces;
using Sopro.Models.Administration;
using Sopro.Models.Infrastructure;
using Sopro.Models.Simulation;
using System;
using System.Collections.Generic;
using System.Text;

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
            plugs = new List<Plug> { p1, p2 },
            maxPower = 200,
            manufacturer = "hi",
            maxParallelUseable = 4
        };

        static Station s2 = new Station
        {
            plugs = new List<Plug> { p3 },
            maxPower = 200,
            manufacturer = "hi",
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

        private static Schedule s = new Schedule();

        private static ILocation l = new Location()
        {
            id = "locationidk",
            zones = new List<Zone>() { z1 },
            name = "Berlin",
            emergency = 0.05,
            schedule = s,
            distributor = new Distributor(s, l)
        };

        private static Scenario scenario = new Scenario()
        {
            duration = 10,
            bookingCountPerDay = 20,
            vehicles = new List<Vehicle>() { v1, v2 },
            rushhours = new List<Rushhour>() { r1 },
            start = DateTime.Now.AddHours(1),
            location = l
        };

        private static ExecutedScenario executedScenario = new ExecutedScenario(scenario);

        [Test]
        public void testSimulator()
        {
            Simulator sim = new Simulator()
            {
                exScenario = executedScenario
            };
            sim.run();
            Assert.IsTrue(l.schedule.bookings.Count > 0);
        }
    }
}
