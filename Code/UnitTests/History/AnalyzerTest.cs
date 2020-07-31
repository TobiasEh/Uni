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
        private static TimeSpan oneDay = new TimeSpan(1, 0, 0, 0);

        private static Rushhour r1 = new Rushhour()
        {

            start = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 12, 0, 0) + oneDay,
            end = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 14, 0, 0) + oneDay,
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

        private static Location l4 = new Location()
        {
            id = "locationidk4",
            zones = new List<Zone>() { z1, z2, z3 },
            name = "Ludwigsburg",
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

        private static Scenario scenario4 = new Scenario()
        {
            duration = 1,
            bookingCountPerDay = 15,
            vehicles = new List<Vehicle>() { v1, v2 },
            rushhours = new List<Rushhour>() { r1 },
            start = DateTime.Now.AddDays(1),
            location = l4,
        };

        private static Scenario scenario5 = new Scenario()
        {
            duration = 100,
            bookingCountPerDay = 150,
            vehicles = new List<Vehicle>() { v1, v2 },
            rushhours = new List<Rushhour>() { r1 },
            start = DateTime.Now.AddDays(1),
            location = l4,
        };

        private static ExecutedScenario executedScenario = new ExecutedScenario(scenario);
        private static ExecutedScenario executedScenariob = new ExecutedScenario(scenariob);
        private static ExecutedScenario executedScenario2 = new ExecutedScenario(scenario2);
        private static ExecutedScenario executedScenario3 = new ExecutedScenario(scenario3);
        private static ExecutedScenario executedScenario3b = new ExecutedScenario(scenario3b);
        private static ExecutedScenario executedSxenario4 = new ExecutedScenario(scenario4);
        private static ExecutedScenario executedSxenario5 = new ExecutedScenario(scenario5);

        [TestCase]
        public async Task testAnalyzerCCSOnlyNoRushhour()
        {
            Simulator sim = new Simulator()
            {
                exScenario = executedScenario
            };

            await sim.run();

            Analyzer.upperTreshold = 90.00;
            Analyzer.lowerTreshold = 50.00;
            Analyzer.analyze(executedScenario);
            Evaluation evaluation = Analyzer.evaluation;

            testResults(evaluation);
            testValues(evaluation);
            testSuggestion(evaluation);
        }

        [Test]
        public async Task testAnalyzerCCSOnly()
        {
            Simulator sim = new Simulator()
            {
                exScenario = executedScenariob
            };

            await sim.run();

            Analyzer.upperTreshold = 90.00;
            Analyzer.lowerTreshold = 50.00;

            Analyzer.analyze(executedScenariob);
            Evaluation evaluation = Analyzer.evaluation;

            testResults(evaluation);
            testValues(evaluation);
            testSuggestion(evaluation);
        }

        [Test]
        public async Task testAnalyzerTypeTwoOnlyNoRushhour()
        {
            Simulator sim = new Simulator()
            {
                exScenario = executedScenario2
            };

            await sim.run();

            Analyzer.upperTreshold = 90.00;
            Analyzer.lowerTreshold = 50.00;
            Analyzer.analyze(executedScenario2);
            Evaluation evaluation = Analyzer.evaluation;

            testResults(evaluation);
            testValues(evaluation);
            testSuggestion(evaluation);
        }

        [Test]
        public async Task testAnalyzerBothPlugsNoRushhour()
        {
            Simulator sim = new Simulator()
            {
                exScenario = executedScenario3
            };


            await sim.run();

            Analyzer.upperTreshold = 90.00;
            Analyzer.lowerTreshold = 50.00;
            Analyzer.analyze(executedScenario3);
            Evaluation evaluation = Analyzer.evaluation;

            testResults(evaluation);
            testValues(evaluation);
            testSuggestion(evaluation);
        }

        [Test]
        public async Task testAnalyzerBothPlugsRushhour()
        {
            Simulator sim = new Simulator()
            {
                exScenario = executedScenario3b
            };


            await sim.run();

            Analyzer.upperTreshold = 90.00;
            Analyzer.lowerTreshold = 50.00;
            Analyzer.analyze(executedScenario3b);
            Evaluation evaluation = Analyzer.evaluation;

            testResults(evaluation);
            testValues(evaluation);
            testSuggestion(evaluation);
        }

        [Test]
        public async Task testAnalyzerToMuchInfrastructure()
        {
            Simulator sim = new Simulator()
            {
                exScenario = executedSxenario4
            };

            await sim.run();

            Analyzer.upperTreshold = 10.00;
            Analyzer.lowerTreshold = 5.00;
            Analyzer.analyze(executedSxenario4);
            Evaluation evaluation = Analyzer.evaluation;

            testResults(evaluation);
            testValues(evaluation);
            testSuggestion(evaluation);

            string[] strings = evaluation.suggestions[0].suggestion.Split(' ');
            Assert.IsTrue(strings[8].Equals("weniger."));
        }

        [Test]
        public async Task testAnalyzerMoreThanOneTick()
        {
            Simulator sim = new Simulator()
            {
                exScenario = executedSxenario5
            };

            await sim.run();

            Analyzer.upperTreshold = 10.00;
            Analyzer.lowerTreshold = 5.00;
            Analyzer.analyze(executedSxenario5);
            Evaluation evaluation = Analyzer.evaluation;

            testResults(evaluation);
            testValues(evaluation);
            testSuggestion(evaluation);
        }

        public void testValues(Evaluation evaluation)
        {
            
            Console.WriteLine("BookingSuccessRate");
            Console.WriteLine(evaluation.bookingSuccessRate);
            Console.WriteLine("neccessaryWorkload");
            Console.WriteLine(evaluation.neccessaryWorkload);
            Console.WriteLine("plugDistributionAccepted[0]");
            Console.WriteLine(evaluation.plugDistributionAccepted[0]);
            Console.WriteLine("plugDistributionAccepted[1]");
            Console.WriteLine(evaluation.plugDistributionAccepted[1]);
            Console.WriteLine("plugsDistributionDeclined[0]");
            Console.WriteLine(evaluation.plugDistributionDeclined[0]);
            Console.WriteLine("plugsDistributionDeclined[1]");
            Console.WriteLine(evaluation.plugDistributionDeclined[1]);
            if (evaluation.suggestions != null && evaluation.suggestions.Count > 0 && evaluation.suggestions[0] != null)
            {
                Console.WriteLine("suggestions[0]");
                Console.WriteLine(evaluation.suggestions[0].suggestion);
            }
            
            Console.WriteLine("unneccessaryWorkload");
            Console.WriteLine(evaluation.unneccessaryWorkload);

            Assert.IsTrue(evaluation.unneccessaryWorkload >= 0);
            Assert.IsTrue(evaluation.unneccessaryWorkload <= 100);
            Assert.IsTrue(evaluation.plugDistributionDeclined[0] <= 1);
            Assert.IsTrue(evaluation.plugDistributionDeclined[0] >= 0);
            Assert.IsTrue(evaluation.plugDistributionDeclined[1] <= 1);
            Assert.IsTrue(evaluation.plugDistributionDeclined[1] >= 0);
            Assert.IsTrue(evaluation.plugDistributionAccepted[0] >= 0);
            Assert.IsTrue(evaluation.plugDistributionAccepted[0] <= 1);
            Assert.IsTrue(evaluation.plugDistributionAccepted[1] >= 0);
            Assert.IsTrue(evaluation.plugDistributionAccepted[1] <= 1);
            Assert.IsTrue(evaluation.bookingSuccessRate <= 100);
            Assert.IsTrue(evaluation.bookingSuccessRate >= 0);
        }


        public void testSuggestion(Evaluation evaluation)
        {
            if (evaluation.suggestions != null && evaluation.suggestions.Count > 0 && evaluation.suggestions[0] != null)
            {
                string[] strings = evaluation.suggestions[0].suggestion.Split(' ');
                Assert.IsTrue(int.Parse(strings[3]) >= 0);
                Assert.IsTrue(int.Parse(strings[6]) >= 0);
            }
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
