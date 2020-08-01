using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Sopro.Models.Administration;
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

        static Station s3 = new Station
        {
            plugs = new List<Plug> { p1 },
            maxPower = 200,
            manufacturer = "Blau",
            maxParallelUseable = 1
        };

        static Station s4 = new Station
        {
            plugs = new List<Plug> { p3 },
            maxPower = 200,
            manufacturer = "Rot",
            maxParallelUseable = 1
        };

        static Station s5 = new Station
        {
            plugs = new List<Plug> { p1, p3 },
            maxPower = 200,
            manufacturer = "Lila",
            maxParallelUseable = 2
        };

        static Station s6 = new Station
        {
            plugs = new List<Plug> { p1, p3 },
            maxPower = 200,
            manufacturer = "Orange",
            maxParallelUseable = 2
        };

        static Station s7 = new Station
        {
            plugs = new List<Plug> { p1, p3 },
            maxPower = 200,
            manufacturer = "Grün",
            maxParallelUseable = 2
        };

        static Station s8 = new Station
        {
            plugs = new List<Plug> { p1, p3 },
            maxPower = 200,
            manufacturer = "Pink",
            maxParallelUseable = 2
        };

        static Station s9 = new Station
        {
            plugs = new List<Plug> { p1, p3 },
            maxPower = 200,
            manufacturer = "Gelb",
            maxParallelUseable = 2
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
            stations = new List<Station> { s3, s4 },
            site = 'C',
            maxPower = 1000
        };

        static Zone z4 = new Zone
        {
            stations = new List<Station> { s5, s6, s7},
            site = 'D',
            maxPower = 1000
        };

        static Zone z5 = new Zone
        {
            stations = new List<Station> { s8, s9 },
            site = 'E',
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
            zones = new List<Zone>() { z4 },
            name = "Ludwigsburg",
            emergency = 0.05,
        };

        private static Location l5 = new Location()
        {
            id = "locationidk5",
            zones = new List<Zone>() { z5 },
            name = "uwu",
            emergency = 0.05
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
            location = l5,
        };

        private static ExecutedScenario executedScenario = new ExecutedScenario(scenario);
        private static ExecutedScenario executedScenariob = new ExecutedScenario(scenariob);
        private static ExecutedScenario executedScenario2 = new ExecutedScenario(scenario2);
        private static ExecutedScenario executedScenario3 = new ExecutedScenario(scenario3);
        private static ExecutedScenario executedScenario3b = new ExecutedScenario(scenario3b);
        private static ExecutedScenario executedScenario4 = new ExecutedScenario(scenario4);
        private static ExecutedScenario executedScenario5 = new ExecutedScenario(scenario5);

        [Test]
        public async Task testAnalyzerCCSOnlyNoRushhour()
        {
            Simulator sim = new Simulator()
            {
                exScenario = executedScenario
            };

            await sim.run();

            printScenarioDetail(executedScenario);

            Analyzer.upperTreshold = 98;
            Analyzer.lowerTreshold = 70;
            Evaluation evaluation = Analyzer.analyze(executedScenario);

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

            printScenarioDetail(executedScenariob);

            Analyzer.upperTreshold = 98;
            Analyzer.lowerTreshold = 70;
            Evaluation evaluation = Analyzer.analyze(executedScenariob);

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

            printScenarioDetail(executedScenario2);

            Analyzer.upperTreshold = 98;
            Analyzer.lowerTreshold = 70;
            Evaluation evaluation = Analyzer.analyze(executedScenario2);

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

            Analyzer.upperTreshold = 0.98;
            Analyzer.lowerTreshold = 0.70;
            Evaluation evaluation = Analyzer.analyze(executedScenario3);

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

            Analyzer.upperTreshold = 0.98;
            Analyzer.lowerTreshold = 0.70;
            Evaluation evaluation = Analyzer.analyze(executedScenario3b);

            testResults(evaluation);
            testValues(evaluation);
            testSuggestion(evaluation);
        }

        // Seltsames stochastisches Testverhalten. Manchmal ergibt sich die
        // successRate zu ca. 16666,666666666668 ?
        // Problem: Menge der Stationen unter den Zonen ist nicht disjunkt lol
        [Test]
        public async Task testAnalyzerTooMuchInfrastructure()
        {
            Simulator sim = new Simulator()
            {
                exScenario = executedScenario4
            };

            await sim.run();

            printScenarioDetail(executedScenario4);

            Analyzer.upperTreshold = 0.98;
            Analyzer.lowerTreshold = 0.70;
            Evaluation evaluation = Analyzer.analyze(executedScenario4);

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
                exScenario = executedScenario5
            };

            await sim.run();

            Analyzer.upperTreshold = 0.98;
            Analyzer.lowerTreshold = 0.70;
            Evaluation evaluation = Analyzer.analyze(executedScenario5);

            testResults(evaluation);
            testValues(evaluation);
            testSuggestion(evaluation);
        }

        public void testValues(Evaluation evaluation)
        {
            Console.WriteLine("[AnalyzerTest.cs]");
            Console.WriteLine("Erfolgsrate:\t\t\t" + evaluation.bookingSuccessRate.ToString());
            Console.WriteLine("Notwendige Auslastung:\t\t" + evaluation.neccessaryWorkload.ToString());
            Console.WriteLine("Anteil Type 2 [Akz. Buchungen]:\t" + evaluation.plugDistributionAccepted[0].ToString());
            Console.WriteLine("Anteil Type-2 [Abg. Buchungen]:\t" + evaluation.plugDistributionDeclined[0].ToString());
            Console.WriteLine("Anteil CSS [Akz. Buchungen]:\t" + evaluation.plugDistributionAccepted[1].ToString());
            Console.WriteLine("Anteil CCS [Abg. Buchungen]:\t" + evaluation.plugDistributionDeclined[1].ToString());

            if (evaluation.suggestions != null && evaluation.suggestions.Count > 0 && evaluation.suggestions[0] != null)
                Console.WriteLine("Vorschläge:\t\t\t" + evaluation.suggestions[0].suggestion);

            Console.WriteLine("Unnötige Auslastung:\t\t" + evaluation.unneccessaryWorkload.ToString());

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

        private static void printScenarioDetail(ExecutedScenario s)
        {
            Console.WriteLine("Generierte Buchungen: ");
            printDetailedBookingList(s.generatedBookings, 0);
            Console.WriteLine("Davon akzeptierte Buchungen: ");
            printDetailedBookingList(s.location.schedule.bookings, 0);

            Console.WriteLine("Location workload:");
            foreach (double d in s.getLocationWorkload())
            {
                Console.Write(d.ToString() + ", ");
            }
            Console.WriteLine("");
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
    }
}
