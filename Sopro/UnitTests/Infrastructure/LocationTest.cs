using NUnit.Framework;
using sopro2020_abgabe.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;

namespace UnitTests.Infrastructure
{
    [TestFixture]
    class LocationTest
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

        Station s1 = new Station
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

        Zone z1 = new Zone
        {
            stations = { s1 },
            id = "abc",
            site = 'A',
            maxPower = 1000
        };

        Zone z2 = new Zone
        {
            stations = { s2 },
            id = "abc",
            site = 'B',
            maxPower = 1000
        };

        public void testLocationCreateValid()
        {
            Location location = new Location
            {
                zones = { z1 },
                id = "xas",
                name ="hi",
                emergency = 3.5
            };

            var validationResults = new List<ValidationResult>();
            Assert.IsTrue(Validator.TryValidateObject(location, new ValidationContext(location), validationResults, true));
            Assert.AreEqual(0, validationResults.Count);
        }

        public void testLocationCreateInvalidZones()
        {
            Location location = new Location
            {
                zones = { },
                id = "xas",
                name = "hi",
                emergency = 3.5
            };

            var validationResults = new List<ValidationResult>();
            var actual = Validator.TryValidateObject(location, new ValidationContext(location), validationResults, true);
            Assert.AreEqual(1, validationResults.Count);

            var msg = validationResults[0];
            Assert.AreEqual("zones", msg.MemberNames.ElementAt(0));
        }

        public void testLocationCreateInvalidName()
        {
            Location location = new Location
            {
                zones = { z1 },
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

        public void testLocationCreateInvalidEmergency()
        {
            Location location = new Location
            {
                zones = { z1 },
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

        public void testLocationAddZone()
        {
            Location location = new Location
            {
                zones = { z1 },
                id = "xas",
                name = "hi",
                emergency = -3.5
            };

            int zones_before = location.zones.Count;
            location.addZone(z2);
            Assert.IsTrue(location.zones.Count > zones_before);
        }

        public void testLocationAddZone()
        {
            Location location = new Location
            {
                zones = { z1, z2 },
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
