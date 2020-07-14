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

namespace UnitTests.Administration
{
    [TestFixture]
    class DistributionTimerTest
    {
        private IMemoryCache cache;
        private string cacheKey = CacheKeys.LOCATION;
        private List<ILocation> locations;
        private Location location1 = new Location();
        private ILogger<DistributionTimer> logger;
        
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


        [SetUp]
        public void setUp()
        {
            location1.emergency = 120;
            location1.name = "Ludwigsburg";
            location1.addZone(zone);
            locations = new List<ILocation>() { location1 };
            cache.Set(locations, cacheKey);
        }

        [Test]
        private void asyncTest()
        {
            setUp();
            DistributionTimer timer = new DistributionTimer(cache, logger);
            t
        }
    }
}
