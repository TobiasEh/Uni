using NUnit.Framework;
using Sopro.Models.Administration;
using Sopro.Models.Infrastructure;
using Sopro.Models.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UnitTests.Administration
{
    class DistributionTestUli
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

        static Station s1 = new Station
        {
            plugs = new List<Plug>() { p1, p2 },
            maxPower = 200,
            manufacturer = "hi",
            maxParallelUseable = 1
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

        static Schedule s = new Schedule();
        static int puffer = 15;

        [Test]
        public void testlauf()
        {
            List<Booking> b = new List<Booking>();
            Booking b1 = new Booking
            {
                capacity = 120,
                socStart = 22,
                socEnd = 44,
                user = "User@userexample.com",
                startTime = DateTime.Now.AddDays(1).AddHours(-6),
                endTime = DateTime.Now.AddDays(1).AddHours(6),
                location = lTest,
                plugs = new List<PlugType>() { PlugType.CCS },
                priority = UserType.VIP,
            };
            b.Add(b1);
            StandardDistribution sd = new StandardDistribution();
            sd.distribute(b, s, puffer);

            if(!s.bookings.Any())
            {
                Assert.IsTrue(1 == 2);
            }
            foreach(Booking bo in s.bookings)
            {
                Assert.IsTrue(bo.location.Equals(lTest));
            }
        }
        

        

    }
}
