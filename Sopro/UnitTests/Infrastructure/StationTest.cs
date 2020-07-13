using NUnit.Framework;
using Sopro.Models.Infrastructure;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace UnitTests.Infrastructure
{
    [TestFixture]
    class StationTest
    {
        [Test]
        public void testStationCreateValid()
        {
            Station station = new Station
            {
                //id = "abc",
                plugs = new List<Plug>{ new Plug { power = 10 }, new Plug { power = 20 } },
                maxPower = 200,
                manufacturer = "hi",
                maxParallelUseable = 4
            };

            var validationResults = new List<ValidationResult>();
            Assert.IsTrue(Validator.TryValidateObject(station, new ValidationContext(station), validationResults, true));
            Assert.AreEqual(0, validationResults.Count);
        }

        [Test]
        public void testStationCreateInvalidPlugs()
        {
            Station station = new Station
            {
                //id = "abc",
                plugs = new List<Plug>{ },
                maxPower = 200,
                manufacturer = "hi",
                maxParallelUseable = 4
            };

            var validationResults = new List<ValidationResult>();
            var actual = Validator.TryValidateObject(station, new ValidationContext(station), validationResults, true);
            Assert.AreEqual(1, validationResults.Count);

            var msg = validationResults[0];
            Assert.AreEqual("plugs", msg.MemberNames.ElementAt(0));
        }

        [Test]
        public void testStationCreateInvalidMaxPower()
        {
            Station station = new Station
            {
                //id = "abc",
                plugs = new List<Plug> { new Plug { power = 10 }, new Plug { power = 20 } },
                maxPower = -200,
                manufacturer = "hi",
                maxParallelUseable = 4
            };

            var validationResults = new List<ValidationResult>();
            var actual = Validator.TryValidateObject(station, new ValidationContext(station), validationResults, true);
            Assert.AreEqual(1, validationResults.Count);

            var msg = validationResults[0];
            Assert.AreEqual("maxPower", msg.MemberNames.ElementAt(0));
        }

        [Test]
        public void testStationCreateInvalidMaxParallelUseable()
        {
            Station station = new Station
            {
                //id = "abc",
                plugs = new List<Plug> { new Plug { power = 10 }, new Plug { power = 20 } },
                maxPower = 200,
                manufacturer = "hi",
                maxParallelUseable = -2
            };

            var validationResults = new List<ValidationResult>();
            var actual = Validator.TryValidateObject(station, new ValidationContext(station), validationResults, true);
            Assert.AreEqual(1, validationResults.Count);

            var msg = validationResults[0];
            Assert.AreEqual("maxParallelUseable", msg.MemberNames.ElementAt(0));
        }

        [Test]
        public void testStationCreateAddPlug()
        {
            Station station = new Station
            {
                //id = "abc",
                plugs = new List<Plug> { new Plug { power = 10 }, new Plug { power = 20 } },
                maxPower = 200,
                manufacturer = "hi",
                maxParallelUseable = 4
            };

            int plugs_before = station.plugs.Count;
            station.addPlug(new Plug { power = 50 });
            Assert.IsTrue(station.plugs.Count > plugs_before);
        }

        [Test]
        public void testStationCreateDeletePlug()
        {
            Plug p = new Plug { power = 10 };
            Station station = new Station
            {
                
                plugs = new List<Plug> { p, new Plug { power = 20 } },
                maxPower = 200,
                manufacturer = "hi",
                maxParallelUseable = 4
            };

            int plugs_before = station.plugs.Count;
            station.deletePlug(p);
            Assert.IsTrue(station.plugs.Count < plugs_before);
        }
    }
}
