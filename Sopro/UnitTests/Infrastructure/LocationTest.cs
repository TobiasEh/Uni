using NUnit.Framework;
using Sopro.Models.Infrastructure;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;

namespace UnitTests.Infrastructure
{
    [TestFixture]
    class LocationTest
    {
        static Plug p1 = new Plug
        {
            id = "abc",
            power = 20,
            type = PlugType.CCS
        };

        static Plug p2 = new Plug
        {
            id = "abcd",
            power = 40,
            type = PlugType.TYPE2
        };

        static Plug p3 = new Plug
        {
            id = "abcawdd",
            power = 50,
            type = PlugType.TYPE2
        };

        static Station s1 = new Station
        {
            id = "abc",
            plugs = new List<Plug> { p1, p2 },
            maxPower = 200,
            manufacturer = "hi",
            maxParallelUseable = 4
        };

        static Station s2 = new Station
        {
            id = "abcadwdad",
            plugs = new List<Plug> { p3 },
            maxPower = 200,
            manufacturer = "hi",
            maxParallelUseable = 4
        };

        static Zone z1 = new Zone
        {
            stations = new List<Station> { s1 },
            id = "abc",
            site = 'A',
            maxPower = 1000
        };

        static Zone z2 = new Zone
        {
            stations = new List<Station> { s2 },
            id = "abc",
            site = 'B',
            maxPower = 1000
        };

        [Test]
        public void testLocationCreateValid()
        {
            Location location = new Location
            {
                zones = new List<Zone> { z1 },
                id = "xas",
                name ="hi",
                emergency = 3.5
            };

            var validationResults = new List<ValidationResult>();
            Assert.IsTrue(Validator.TryValidateObject(location, new ValidationContext(location), validationResults, true));
            Assert.AreEqual(0, validationResults.Count);
        }

        [Test]
        public void testLocationCreateInvalidName()
        {
            Location location = new Location
            {
                zones = new List<Zone> { z1 },
                id = "xas",
                name = "",
                emergency = 3.5
            };

            var validationResults = new List<ValidationResult>();
            var actual = Validator.TryValidateObject(location, new ValidationContext(location), validationResults, true);
            Assert.AreEqual(1, validationResults.Count);

            var msg = validationResults[0];
            Assert.AreEqual("name", msg.MemberNames.ElementAt(0));
        }

        [Test]
        public void testLocationCreateInvalidEmergency()
        {
            Location location = new Location
            {
                zones = new List<Zone> { z1 },
                id = "xas",
                name = "hi",
                emergency = -3.5
            };

            var validationResults = new List<ValidationResult>();
            var actual = Validator.TryValidateObject(location, new ValidationContext(location), validationResults, true);
            Assert.AreEqual(1, validationResults.Count);

            var msg = validationResults[0];
            Assert.AreEqual("emergency", msg.MemberNames.ElementAt(0));
        }

        [Test]
        public void testLocationAddZone()
        {
            Location location = new Location
            {
                zones = new List<Zone> { z1 },
                id = "xas",
                name = "hi",
                emergency = -3.5
            };

            int zones_before = location.zones.Count;
            location.addZone(z2);
            Assert.IsTrue(location.zones.Count > zones_before);
        }

        [Test]
        public void testLocationDeleteZone()
        {
            Location location = new Location
            {
                zones = new List<Zone> { z1, z2 },
                id = "xas",
                name = "hi",
                emergency = -3.5
            };

            int zones_before = location.zones.Count;
            location.deleteZone(z2);
            Assert.IsTrue(location.zones.Count < zones_before);
        }

    }
}
