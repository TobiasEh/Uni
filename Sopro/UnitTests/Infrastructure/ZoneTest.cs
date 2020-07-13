using NUnit.Framework;
using Sopro.Models.Infrastructure;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;

namespace UnitTests.Infrastructure
{
    [TestFixture]
    class ZoneTest
    {
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

        static Station s = new Station
        {
            
            plugs = new List<Plug>{ p1, p2 },
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

        [Test]
        public void testZoneCreateValid()
        {
            Zone zone = new Zone
            {
                stations = new List<Station> { s },
                
                site = 'A',
                maxPower = 1000
            };

            var validationResults = new List<ValidationResult>();
            Assert.IsTrue(Validator.TryValidateObject(zone, new ValidationContext(zone), validationResults, true));
            Assert.AreEqual(0, validationResults.Count);
        }

        [Test]
        public void testZoneCreateInvalidStations()
        {
            Zone zone = new Zone
            {
                stations = new List<Station>(){ },
                
                site = 'A',
                maxPower = 1000
            };

            var validationResults = new List<ValidationResult>();
            var actual = Validator.TryValidateObject(zone, new ValidationContext(zone), validationResults, true);
            Assert.AreEqual(1, validationResults.Count);

            var msg = validationResults[0];
            Assert.AreEqual("stations", msg.MemberNames.ElementAt(0));
        }

        [Test]
        public void testZoneCreateInvalidMaxPower()
        {
            Zone zone = new Zone
            {
                stations = new List<Station> { s },
                
                site = 'A',
                maxPower = -1000
            };

            var validationResults = new List<ValidationResult>();
            var actual = Validator.TryValidateObject(zone, new ValidationContext(zone), validationResults, true);
            Assert.AreEqual(1, validationResults.Count);

            var msg = validationResults[0];
            Assert.AreEqual("maxPower", msg.MemberNames.ElementAt(0));
        }

        [Test]
        public void testZoneCreateAddStation()
        {
            Zone zone = new Zone
            {
                stations = new List<Station>{ s },
                
                site = 'A',
                maxPower = 1000
            };

            int stations_before = zone.stations.Count;
            zone.addStation(s2);
            Assert.IsTrue(zone.stations.Count > stations_before);
        }

        [Test]
        public void testZoneCreateDeleteStation()
        {
            Zone zone = new Zone
            {
                stations = new List<Station> { s, s2 },
                
                site = 'A',
                maxPower = 1000
            };

            int stations_before = zone.stations.Count;
            zone.deleteStation(s2);
            Assert.IsTrue(zone.stations.Count < stations_before);
        }
    }
}
