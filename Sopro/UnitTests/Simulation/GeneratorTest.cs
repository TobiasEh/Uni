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
using Sopro.Models.User;

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
        private Generator generator = new Generator(0.05, 0.05);
        private int bookingsCountPerDay = 20;
        private int duration = 30;

        [Test]
        public void generateBookingsBAttributesNotNullTest()
        {
            
            List<Booking> result = new List<Booking>();
            ExecutedScenario scenario = new ExecutedScenario() { bookingCountPerDay = bookingsCountPerDay, duration = duration, location = location, start = start, };
            result = generator.generateBookings(scenario);
            Assert.IsTrue(result.All<Booking>(e => e.socEnd > 0  && e.socStart > 100 && e.plugs != null 
                && e.startTime != null && e.endTime != null && e.user != null && e.capacity > 0));
        }

        [Test]
        public void generateBookingsCount()
        {
            List<Booking> result = new List<Booking>();
            ExecutedScenario scenario = new ExecutedScenario() { bookingCountPerDay = bookingsCountPerDay, duration = duration, location = location, start = start, };
            result = generator.generateBookings(scenario);
            Assert.IsTrue(result.Count == bookingsCountPerDay * duration);
        }

        [Test]
        public void generatedBookingsCountEmploye()
        {
            List<Booking> result = new List<Booking>();
            ExecutedScenario scenario = new ExecutedScenario() { bookingCountPerDay = bookingsCountPerDay, duration = duration, location = location, start = start, };
            result = generator.generateBookings(scenario);
            int numberOfBooking = bookingsCountPerDay * duration;
            int countEmployee = result.Count(e => e.priority.Equals(UserType.EMPLOYEE));
            int countVIP = result.Count(e => e.priority.Equals(UserType.VIP));
            int countGuest = result.Count(e => e.priority.Equals(UserType.GUEST));
            double percentageEmployee = countEmployee * 1 / numberOfBooking;
            double percentageVIP = countVIP * 1 / numberOfBooking;
            double percentageGuest = 1 - percentageEmployee - percentageVIP;
            Assert.IsTrue(percentageVIP >= 0.02 && percentageVIP <= 0.07 && percentageGuest >= 0.02 && percentageGuest <= 0.07);
        }


    }

}
