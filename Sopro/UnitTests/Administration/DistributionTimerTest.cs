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
using System.Threading.Tasks;
using NUnit.Framework.Constraints;

namespace UnitTests.Administration
{
    [TestFixture]
    class DistributionTimerTest
    {
        private IMemoryCache cache = new MemoryCache(new MemoryCacheOptions());
        private string cacheKey = CacheKeys.LOCATION;
        private List<ILocation> locations;
        
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

        private Location location1 = new Location()
        {
            emergency = 120,
            name = "Ludwigsburg",
            zones = new List<Zone>() { zone },

        };

        [SetUp]
        public void setUp()
        {
            locations = new List<ILocation>() { location1 };
            cache.Set(locations, cacheKey);
        }

        [Test]
        public void distributionTimerStartTest()
        {
            setUp();
            DistributionTimer timer = new DistributionTimer(cache, logger);
            Task task = timer.StartAsync(new System.Threading.CancellationToken());
            Assert.IsTrue(task.Equals(Task.CompletedTask));
        }

        [Test]
        public void distributionTimerEndTest()
        {
            setUp();
            DistributionTimer timer = new DistributionTimer(cache, logger);
            Task task = timer.StartAsync(new System.Threading.CancellationToken());
            task = timer.StopAsync(new System.Threading.CancellationToken());
            Assert.IsTrue(task.Equals(Task.CompletedTask));
        }
    }
}
