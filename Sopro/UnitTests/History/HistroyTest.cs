using NUnit.Framework;
using NUnit.Framework.Internal;
using Sopro.Interfaces;
using Sopro.Interfaces.HistorySimulation;
using Sopro.Models.Administration;
using Sopro.Models.History;
using Sopro.Models.Infrastructure;
using Sopro.Models.Simulation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTests.History
{
    [TestFixture]
    class HistoryTest
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

        private static ExecutedScenario executedScenario = new ExecutedScenario(scenario);
        private static ExecutedScenario executedScenariob = new ExecutedScenario(scenariob);
        private static ExecutedScenario executedScenario2 = new ExecutedScenario(scenario2);
        private static ExecutedScenario executedScenario3 = new ExecutedScenario(scenario3);

        [Test]
        public async Task testSimulatorCCSOnlyNoRushhour()
        {
            Simulator sim = new Simulator()
            {
                exScenario = executedScenario
            };

            executedScenario.generatedBookings.ForEach(e => Console.WriteLine(e.startTime + "\t" + e.endTime + "\t" + e.plugs[0].ToString() +"\t" + e.priority.ToString())); 
            await sim.run();
            l.schedule.bookings.ForEach(e => Console.WriteLine(e.startTime + "\t" + e.endTime + "\t" + e.plugs[0].ToString() + "\t" + e.priority.ToString() + "\t" + e.station.manufacturer));

            Console.WriteLine(executedScenario);
            Console.WriteLine("generatedBookings: " + executedScenario.generatedBookings.Count());
            Console.WriteLine("booking.count: " + executedScenario.bookings.Count());
            Console.WriteLine("locationworkload.count: " + executedScenario.getLocationWorkload().Count());
            Console.WriteLine("getfullfiledrequest: " + executedScenario.getFulfilledRequests());
            foreach (List<double> e in executedScenario.getStationWorkload())
            {
                foreach(double f in e)
                {
                    Console.WriteLine("stationworkload: " + f);
                }
            }
            
            foreach(double e in executedScenario.getLocationWorkload())
            {
                Console.WriteLine(e);
            }

            Evaluation _evaluation = Analyzer.analyze(executedScenario);
            Console.WriteLine(_evaluation.suggestions[0].ToString());
            evaluationNotNullTest(_evaluation);

            

        }

        public void evaluationNotNullTest(Evaluation evaluation)
        {
            Assert.IsTrue(evaluation.suggestions != null);
            Assert.LessOrEqual(evaluation.unneccessaryWorkload, 100);
            Assert.GreaterOrEqual(evaluation.unneccessaryWorkload, 0.0);
            Assert.LessOrEqual(evaluation.neccessaryWorkload, 100);
            Assert.GreaterOrEqual(evaluation.neccessaryWorkload, 0.0);
            Assert.LessOrEqual(evaluation.bookingSuccessRate, 100);
            Assert.GreaterOrEqual(evaluation.bookingSuccessRate, 0.0);
            Assert.GreaterOrEqual(evaluation.suggestions.Count, 0);
            Assert.LessOrEqual(evaluation.suggestions.Count, 1);
        }

        
        /*public void suggestionTest(Evaluation evaluation)
        {
            
            string[] splitted = evaluation.suggestions[0].suggestion.Split(" ");
            double station;
            double.TryParse(splitted[7], out station);
            double zone;
            double.TryParse(splitted[10], out zone);
            Console.WriteLine(station + " " + zone);
            if(splitted[12] == "less")
            {
                Assert.IsTrue(station < stations.Count);
                Assert.IsTrue(zone < zones.Count);
            }
            
        }
        [Test]
        public void evaluationRightCaluclatedBookingSuccessRateTest()
        {
            Evaluation evaluation = Analyzer.analyze(scenario);
            Assert.IsTrue(evaluation.bookingSuccessRate == (100.0 * 3.0 / 4.0));
        }

        [Test]
        public void evaluationRightCaluclatedUnneccWorkloadTest()
        {
            Evaluation evaluation = Analyzer.analyze(scenario);
            Assert.IsTrue(evaluation.unneccessaryWorkload == (100 - 86.642));
        }
        [Test]
        public void evaluationRightCaluclatedNeccWorkloadTest()
        {
            Evaluation evaluation = Analyzer.analyze(scenario);
            Assert.IsTrue(evaluation.neccessaryWorkload == (100 - 75));
        }
        [Test]
        public void evaluationRightCaluclatedPlugDistrAccType2Test()
        {
            Evaluation evaluation = Analyzer.analyze(scenario);
            //Type-2
            Assert.IsTrue(evaluation.plugDistributionAccepted[0] == (1.0 / 3.0));
        }
        [Test]
        public void evaluationRightCaluclatedPlugDistrAccCSSTest()
        {
            Evaluation evaluation = Analyzer.analyze(scenario);
            //CSS
            Assert.IsTrue(evaluation.plugDistributionAccepted[1] == (2.0 / 3.0));
        }
        [Test]
        public void evaluationRightCaluclatedPlugDistrDeclinedType2Test()
        {
            Evaluation evaluation = Analyzer.analyze(scenario);
            //Type-2
            Assert.IsTrue(evaluation.plugDistributionDeclined[0] == 0.0);
        }
        [Test]
        public void evaluationRightCaluclatedPlugDistrDeclinedCSSTest()
        {
            Evaluation evaluation = Analyzer.analyze(scenario);
            //CSS
            Assert.IsTrue(evaluation.plugDistributionDeclined[1] == 1);
        }
        */
    }
}
