using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Sopro.Models.History;
using Sopro.Models.Infrastructure;
using Sopro.Models.Simulation;

namespace UnitTests.History
{
    [TestFixture]
    class AnalyzerTest
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
            rushhours = new List<Rushhour>() { },
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

        private static ExecutedScenario executedScenario = new ExecutedScenario(scenario);
        private static ExecutedScenario executedScenariob = new ExecutedScenario(scenariob);
        private static ExecutedScenario executedScenario2 = new ExecutedScenario(scenario2);
        private static ExecutedScenario executedScenario3 = new ExecutedScenario(scenario3);
        private static ExecutedScenario executedScenario3b = new ExecutedScenario(scenario3b);

        [TestCase]
        public async Task testAnalyzerCCSOnlyNoRushhour()
        {
            Simulator sim = new Simulator()
            {
                exScenario = executedScenario
            };

            await sim.run();

            Analyzer.analyze(executedScenario);
            Evaluation evaluation = Analyzer.evaluation;

            testResults(evaluation);
            testValues(evaluation);
        }

        [Test]
        public async Task testAnalyzerCCSOnly()
        {
            Simulator sim = new Simulator()
            {
                exScenario = executedScenariob
            };

            await sim.run();

            Analyzer.analyze(executedScenariob);
            Evaluation evaluation = Analyzer.evaluation;

            testResults(evaluation);
            testValues(evaluation);
        }

        [Test]
        public async Task testAnalyzerTypeTwoOnlyNoRushhour()
        {
            Simulator sim = new Simulator()
            {
                exScenario = executedScenario2
            };

            await sim.run();
            Analyzer.analyze(executedScenario2);
            Evaluation evaluation = Analyzer.evaluation;

            testResults(evaluation);
            testValues(evaluation);
        }

        [Test]
        public async Task testSimulatorBothPlugsNoRushhour()
        {
            Simulator sim = new Simulator()
            {
                exScenario = executedScenario3
            };


            await sim.run();
            Analyzer.analyze(executedScenario3);
            Evaluation evaluation = Analyzer.evaluation;

            testResults(evaluation);
            testValues(evaluation);
        }

        [Test]
        public async Task testSimulatorBothPlugsRushhour()
        {
            Simulator sim = new Simulator()
            {
                exScenario = executedScenario3b
            };


            await sim.run();
            Analyzer.analyze(executedScenario3b);
            Evaluation evaluation = Analyzer.evaluation;

            testResults(evaluation);
            testValues(evaluation);
        }

        public void testValues(Evaluation evaluation)
        {
            Assert.IsTrue(evaluation.bookingSuccessRate >= 0);
            
        }

        public void testResults(Evaluation evaluation)
        {
            Assert.IsNotNull(evaluation);
            Assert.IsNotNull(evaluation.bookingSuccessRate);
            Assert.IsNotNull(evaluation.neccessaryWorkload);
            Assert.IsNotNull(evaluation.plugDistributionAccepted);
            Assert.IsNotNull(evaluation.plugDistributionDeclined);
            Assert.IsNotNull(evaluation.suggestions);
            Assert.IsNotNull(evaluation.unneccessaryWorkload);
        }
    }
}
