using NUnit.Framework;
using sopro2020_abgabe.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;

namespace UnitTests.Infrastructure
{
    [TestFixture]
    class ZoneTest
    {
        Plug p1 = new Plug
        {
            id = "abc",
            power = 20,
            type = PlugType.CCS
        };

        Plug p2 = new Plug
        {
            id = "abcd",
            power = 40,
            type = PlugType.TYPE2
        };

        Plug p3 = new Plug
        {
            id = "abcawdd",
            power = 50,
            type = PlugType.TYPE2
        };

        Station s = new Station
        {
            id = "abc",
            plugs = { p1, p2 },
            maxPower = 200,
            manufacturer = "hi",
            maxParallelUseable = 4
        };

        Station s2 = new Station
        {
            id = "abcadwdad",
            plugs = { p3 },
            maxPower = 200,
            manufacturer = "hi",
            maxParallelUseable = 4
        };

        public void testZoneCreateValid()
        {
            Zone zone = new Zone
            {
                stations = { s },
                id = "abc",
                site = 'A',
                maxPower = 1000
            };

            var validationResults = new List<ValidationResult>();
            Assert.IsTrue(Validator.TryValidateObject(zone, new ValidationContext(zone), validationResults, true));
            Assert.AreEqual(0, validationResults.Count);
        }

        public void testZoneCreateInvalidStations()
        {
            Zone zone = new Zone
            {
                stations = { },
                id = "abc",
                site = 'A',
                maxPower = 1000
            };

            var validationResults = new List<ValidationResult>();
            var actual = Validator.TryValidateObject(zone, new ValidationContext(zone), validationResults, true);
            Assert.AreEqual(1, validationResults.Count);

            var msg = validationResults[0];
            Assert.AreEqual("stations", msg.MemberNames.ElementAt(0));
        }

        public void testZoneCreateInvalidMaxPower()
        {
            Zone zone = new Zone
            {
                stations = { s },
                id = "abc",
                site = 'A',
                maxPower = -1000
            };

            var validationResults = new List<ValidationResult>();
            var actual = Validator.TryValidateObject(zone, new ValidationContext(zone), validationResults, true);
            Assert.AreEqual(1, validationResults.Count);

            var msg = validationResults[0];
            Assert.AreEqual("maxPower", msg.MemberNames.ElementAt(0));
        }

        public void testZoneCreateAddStation()
        {
            Zone zone = new Zone
            {
                stations = { s },
                id = "abc",
                site = 'A',
                maxPower = 1000
            };

            int stations_before = zone.stations.Count;
            zone.addStation(s2);
            Assert.IsTrue(zone.stations.Count > stations_before);
        }

        public void testZoneCreateDeleteStation()
        {
            Zone zone = new Zone
            {
                stations = { s, s2 },
                id = "abc",
                site = 'A',
                maxPower = 1000
            };

            int stations_before = zone.stations.Count;
            zone.deleteStation(s2);
            Assert.IsTrue(zone.stations.Count < stations_before);
        }
    }
}
