/*using NUnit.Framework;
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

namespace UnitTests.History
{
    [TestFixture]
    class HistoryTest
    {
        private IEvaluatable scenario;
        private List<Booking> bookings = new List<Booking>();
        private ILocation location;
        private List<Zone> zones = new List<Zone>();
        private List<Station> stations = new List<Station>();

        
        [OneTimeSetUp]
        public void SetUp()
        {
            List<double> loactionWorkload = new List<double>() { 10.3, 55.3, 86.642, };
            List<List<double>> stationWorkload = new List<List<double>>()
            {
                new List<double>() { 33.2, 66.2, 45.7 },
                new List<double>() { 53.7, 89.3, 92.3 },
                new List<double>() { 34.1, 99.9, 13.3 }
            };

            scenario = new ExecutedScenario(bookings);

            for (int i = 0; i < 3; i++)
            {
                ((ExecutedScenario)scenario).updateWorkload(loactionWorkload[i], stationWorkload[i]);
            }
            Station station1 = new Station()
            {
                manufacturer = "man",
                maxParallelUseable = 2,
                maxPower = 500,
                plugs = new List<Plug>()
                {
                    new Plug() { power = 250, type = PlugType.CCS},
                    new Plug() { power = 250, type = PlugType.CCS }
                }
            };
            Station station2 = new Station()
            {
                manufacturer = "neu",
                maxParallelUseable = 1,
                maxPower = 500,
                plugs = new List<Plug>()
                {
                    new Plug() { power = 250, type = PlugType.CCS },
                    new Plug() { power = 250, type = PlugType.TYPE2 }
                }
            };
            Station station3 = new Station()
            {
                manufacturer = "fac",
                maxParallelUseable = 3,
                maxPower = 750,
                plugs = new List<Plug>()
                {
                    new Plug() { power = 500, type = PlugType.CCS },
                    new Plug() { power = 500, type = PlugType.CCS },
                    new Plug() { power = 250, type = PlugType.TYPE2 }
                }
            };
            stations.Add(station1);
            stations.Add(station2);
            stations.Add(station3);
            Zone zone1 = new Zone() { maxPower = 1750, site = 'A', stations = stations };
            zones.Add(zone1);
            location = new Location() { name = "location1", zones = zones };
            Booking bookingAcc1 = new Booking()
            {
                capacity = 1000,
                plugs = new List<PlugType>() { PlugType.CCS },
                location = location,
                socStart = 20,
                socEnd = 40,
                startTime = DateTime.Now.AddDays(1),
                endTime = DateTime.Now.AddDays(1).AddHours(4),
                user = "email@user.com",
                station = station2,
            };
            Booking bookingAcc2 = new Booking()
            {
                capacity = 1500,
                plugs = new List<PlugType>() { PlugType.TYPE2 },
                location = location,
                socStart = 60,
                socEnd = 100,
                startTime = DateTime.Now.AddDays(1.5),
                endTime = DateTime.Now.AddDays(1.5).AddHours(4),
                user = "email2@user.com",
                station = station1,
            };
            Booking bookingDel1 = new Booking()
            {
                capacity = 999,
                plugs = new List<PlugType>() { PlugType.CCS },
                location = location,
                socStart = 10,
                socEnd = 60,
                startTime = DateTime.Now.AddDays(1),
                endTime = DateTime.Now.AddDays(1).AddHours(4),
                user = "email4@user.com",
                station = null,
            };
            Booking bookingAcc3 = new Booking()
            {
                capacity = 1000,
                plugs = new List<PlugType>() { PlugType.CCS },
                location = location,
                socStart = 10,
                socEnd = 70,
                startTime = DateTime.Now.AddDays(1),
                endTime = DateTime.Now.AddDays(1).AddHours(4),
                user = "email5@user.com",
                station = station2,
            };
            bookings.Add(bookingAcc1);
            bookings.Add(bookingAcc2);
            bookings.Add(bookingAcc3);
            bookings.Add(bookingDel1);
            ((ExecutedScenario)scenario).location = location;
            ((ExecutedScenario)scenario).bookings = bookings;
            ((ExecutedScenario)scenario).fulfilledRequests = 3;
        }

        [Test]
        public void evaluationNotNullTest()
        {
            Evaluation evaluation = Analyzer.analyze(scenario);

            Assert.IsTrue(evaluation.suggestions != null);
            Assert.IsTrue(evaluation.unneccessaryWorkload <= 100.0 && evaluation.unneccessaryWorkload >= 0.0);
            Assert.IsTrue(evaluation.neccessaryWorkload <= 100.0 && evaluation.neccessaryWorkload >= 0.0);
            Assert.IsTrue(evaluation.bookingSuccessRate <= 100.0 && evaluation.bookingSuccessRate >= 0.0);
            Assert.IsTrue(evaluation.suggestions.Count >= 0 && evaluation.suggestions.Count <= 1);
            Assert.IsTrue(evaluation.plugDistributionDeclined.Count == 2);
            Assert.IsTrue(evaluation.plugDistributionAccepted.Count == 2);

        }

        [Test]
        public void suggestionTest()
        {
            Evaluation evaluation = Analyzer.analyze(scenario);
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

    }
}
*/