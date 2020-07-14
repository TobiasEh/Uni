using System;
using NUnit.Framework;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Caching.Memory;
using Sopro.Controllers;
using Sopro.Interfaces;
using Sopro.Models.Infrastructure;
using Sopro.Models.Administration;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace UnitTests.Administration
{
    [TestFixture]
    class DistributionTimerTest
    {
        private IMemoryCache cache = new MemoryCache(new MemoryCacheOptions());
        private string cacheKeyL = CacheKeys.LOCATION;
        private string cacheKeyB = CacheKeys.BOOKING;
        private List<ILocation> locations;
        private List<Booking> bookings;
        
        private ILogger<DistributionTimer> logger = new Logger<DistributionTimer>(new LoggerFactory());
        
        private static Plug plug1 = new Plug()
        {
            power = 100,
            type = PlugType.CCS
        };
        private static Plug plug2 = new Plug()
        {
            power = 100,
            type = PlugType.TYPE2,
        };

        private static Station station = new Station()
        {
            manufacturer = "AHG",
            maxPower = 300,
            maxParallelUseable = 3,
            plugs = new List<Plug>() { plug1 , plug2}

        };
        private static Zone zone = new Zone()
        {
            maxPower = 1000,
            site = 'A',
            stations = new List<Station>() { station }
        };

        private static Location location1 = new Location()
        {
            emergency = 120,
            name = "Ludwigsburg",
            zones = new List<Zone>() { zone },

        };

        private Booking booking1 = new Booking()
        {
            capacity = 120,
            startTime = DateTime.Now.AddDays(2),
            endTime = DateTime.Now.AddDays(2).AddHours(4),
            location = location1,
            user = "user@me.com",
            plugs = new List<PlugType>() { PlugType.TYPE2 },
            priority = Sopro.Models.User.UserType.EMPLOYEE,
            socStart = 24,
            socEnd = 44,
        };

        private Booking booking2 = new Booking()
        {
            capacity = 1000,
            startTime = DateTime.Now.AddDays(2).AddHours(2),
            endTime = DateTime.Now.AddDays(2).AddHours(5),
            location = location1,
            user = "user2@me.com",
            plugs = new List<PlugType>() { PlugType.CCS },
            priority = Sopro.Models.User.UserType.EMPLOYEE,
            socStart = 50,
            socEnd = 100,
        };

        [SetUp]
        public void setUp()
        {
            Distributor dis = new Distributor(new Schedule(), location1);
            location1.distributor = dis;
            bookings = new List<Booking>() { booking1, booking2};
            locations = new List<ILocation>() { location1 };
            cache.Set(locations, cacheKeyL);
            cache.Set(bookings, cacheKeyB);
        }


        [Test]
        public void distributionTimerStartTest()
        {
            StackTrace stackTrace = new StackTrace();
            var mockCookieManager = new Mock();

        }
    }
}
