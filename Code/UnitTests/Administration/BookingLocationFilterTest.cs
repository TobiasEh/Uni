using NUnit.Framework;
using Sopro.Models.Administration;
using Sopro.Models.Infrastructure;
using System;
using System.Collections.Generic;

namespace UnitTests.Administration
{
    [TestFixture]
    class BookingLocationFilterTest
    {
        static Location location = new Location()
        {
            name = "Ludwigshaven",
            emergency = 0.67,
        };

        static Location location2 = new Location()
        {
            name = "München",
            emergency = 0.23,
        };

        static Booking booking1 = new Booking()
        {
            capacity = 23,
            startTime = DateTime.Now.AddDays(2.5),
            endTime = DateTime.Now.AddDays(3),
            location = location,
        };

        static Booking booking2 = new Booking()
        {
            capacity = 99,
            startTime = DateTime.Now.AddDays(1),
            endTime = DateTime.Now.AddDays(1.25),
            location = location,
        };

        static Booking booking3 = new Booking()
        {
            capacity = 45,
            startTime = DateTime.Now.AddDays(3),
            endTime = DateTime.Now.AddDays(3.75),
            location = location2,
        };

        static List<Booking> bookings = new List<Booking>() { booking1, booking2, booking3 };

        [Test]
        public void filterWithoutTimespan()
        {
            BookingLocationFilter locFilter = new BookingLocationFilter(location);
            List<Booking> result = locFilter.filter(DateTime.Now, bookings);
            Assert.IsTrue(result.Contains(booking1) && result.Contains(booking2) && result.Count == 2);
        }

        [Test]
        public void filterWithTimespan()
        {
            BookingLocationFilter locFilter = new BookingLocationFilter(location, 2);
            List<Booking> result = locFilter.filter(DateTime.Now, bookings);
            Assert.IsTrue(result.Contains(booking2) && result.Count == 1);
        }
        [Test]
        public void returnNull()
        {
            BookingLocationFilter locFilter = new BookingLocationFilter(new Location() { name = "Hamburg"});
            List<Booking> result = locFilter.filter(DateTime.Now, bookings);
            Assert.IsTrue(result.Count == 0);
        }
    }
}
