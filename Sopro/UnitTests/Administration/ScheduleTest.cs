using NUnit.Framework;
using Sopro.Models.Administration;
using Sopro.Models.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;

namespace UnitTests.Administration
{
    [TestFixture]
    class ScheduleTest
    {
        
        //List<Booking> bookings { get; set; }
        Booking b1 = new Booking
        {
            capacity = 120,
            socStart = 22,
            socEnd = 44,
            user = "empfaenger1324@gmail.com",
            startTime = DateTime.Now.AddDays(1).AddHours(3),
            endTime = DateTime.Now.AddDays(1).AddHours(6),
            location = lTest,
            plugs = new List<PlugType>() { PlugType.CCS },
        };

        Booking b2 = new Booking
        {
            capacity = 120,
            socStart = 22,
            socEnd = 44,
            user = "empfaenger1324@gmail.com",
            startTime = DateTime.Now.AddDays(1).AddHours(3),
            endTime = DateTime.Now.AddDays(1).AddHours(6),
            location = lTest,
            plugs = new List<PlugType>() { PlugType.TYPE2 },
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

        static Station s1 = new Station
        {
            plugs = new List<Plug>() { p1, p2 },
            maxPower = 200,
            manufacturer = "hi",
            maxParallelUseable = 4
        };

        static Zone z1 = new Zone
        {
            stations = new List<Station>() { s1 },
            site = 'A',
            maxPower = 1000
        };

        static Location lTest = new Location()
        {
            name = "TestLocation",
            emergency = 1,
            zones = new List<Zone>() { z1 }
        };

        [Test]
        public void TestScheduleConstructor()
        {
            Schedule s = new Schedule();
            Assert.IsNotNull(s.bookings);
            //Assert.IsInstanceOf(List<Booking>, s.bookings);
        }

        [Test]
        public void TestAddBookingTrue()
        {
            Schedule s1 = new Schedule();
            Assert.IsTrue(s1.addBooking(b1));
        }
        [Test]
        public void TestAddBookingContains()
        {
            Schedule s2 = new Schedule();
            s2.addBooking(b1);
            Assert.IsFalse(s2.addBooking(b1));
        }
        [Test]
        public void TestAddBookingCount()
        {
            Schedule s3 = new Schedule();
            s3.addBooking(b1);
            s3.addBooking(b2);
            Assert.AreEqual(2, s3.bookings.Count());
        }
        [Test]
        public void TestRemoveBookingTrue()
        {
            Schedule s4 = new Schedule();
            s4.addBooking(b1);
            Assert.IsTrue(s4.removeBooking(b1));
        }
        [Test]
        public void TestRemoveBookingContains()
        {
            Schedule s5 = new Schedule();
            s5.addBooking(b1);
            Assert.IsFalse(s5.removeBooking(b2));
        }
        [Test]
        public void TestRemoveBookingCount()
        {
            Schedule s6 = new Schedule();
            s6.addBooking(b1);
            s6.addBooking(b2);
            s6.removeBooking(b1);
            Assert.AreEqual(1, s6.bookings.Count());
        }
        [Test]
        public void TestCleanTrue()
        {
            Schedule s7 = new Schedule();
            s7.addBooking(b1);
            s7.addBooking(b2);
            Assert.IsTrue(s7.clean(DateTime.Now.AddDays(1).AddHours(7)));
        }
        [Test]
        public void TestCleanWrong()
        {
            Schedule s8 = new Schedule();
            s8.addBooking(b1);
            s8.addBooking(b2);
            Assert.IsFalse(s8.clean(DateTime.Now.AddDays(1).AddHours(3)));
        }
        [Test]
        public void TestToggleCheckTrue()
        {
            Schedule s9 = new Schedule();
            s9.addBooking(b1);
            s9.addBooking(b2);
            s9.toggleCheck(b2);
            Assert.IsTrue(s9.bookings[1].active);
        }
    }
}
