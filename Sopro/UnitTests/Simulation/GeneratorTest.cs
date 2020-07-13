using NUnit.Framework;
using Sopro.Models.Simulation;
using System;
using System.Collections.Generic;
using System.Text;
using Sopro.Models.Administration;
using Sopro.Interfaces;
using Sopro.Models.Infrastructure;
using NUnit.Framework.Internal;
using System.Linq;

namespace UnitTests.Simulation
{
    [TestFixture]
    class GeneratorTest
    {
        private ILocation location = new Location()
        {
            name = "Ludwigsburg",
            emergency = 30.2,
        };

        private DateTime start = DateTime.Now.AddDays(2).AddHours(1).AddMinutes(4);
        private Generator generator = new Generator();

        [Test]
        public void generateBookingsBAttributesNotNullTest()
        {
            
            List<Booking> result = new List<Booking>();
            ExecutedScenario scenario = new ExecutedScenario() { bookingCountPerDay = 20, duration = 30, location = location, start = start, };
            result = generator.generateBookings(scenario);
            Assert.IsTrue(result.All<Booking>(e => e.socEnd > 0  && e.socStart > 100 && e.plugs != null 
                && e.startTime != null && e.endTime != null && e.user != null && e.capacity > 0));
        }

        [Test]
        public void generateBookingsCount()
        {
            List<Booking> result = new List<Booking>();
            ExecutedScenario scenario = new ExecutedScenario() { bookingCountPerDay = 20, duration = 30, location = location, start = start, };
            result = generator.generateBookings(scenario);
            Assert.IsTrue(result.Count == 20 * 30);
        }
    }

}
