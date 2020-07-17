using System;
using NUnit.Framework;
using System.Collections.Generic;
using Microsoft.Extensions.Caching.Memory;
using Sopro.Controllers;
using Sopro.Interfaces;
using Sopro.Models.Infrastructure;
using Sopro.Models.Administration;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace UnitTests.Administration
{
    [TestFixture]
    class DistributionTimerTest
    {
        private MemoryCache cache = new MemoryCache(new MemoryCacheOptions());
        private List<ILocation> locations;
        private List<Booking> bookings;
        
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

        private static ILocation location = new Location()
        {
            id = "locationidk",
            zones = new List<Zone>() { zone },
            name = "Berlin",
            emergency = 0.05,
            // Verteile zur vollen Zeitheinheit
            normalizedDistributionTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Hour, DateTime.Now.Minute + 1, DateTime.Now.Second)
        };

        private static Booking booking1 = new Booking()
        {
            capacity = 120,
            startTime = DateTime.Now.AddDays(2),
            endTime = DateTime.Now.AddDays(2).AddHours(4),
            location = (Location)location,
            user = "user@me.com",
            plugs = new List<PlugType>() { PlugType.TYPE2 },
            priority = Sopro.Models.User.UserType.EMPLOYEE,
            socStart = 24,
            socEnd = 44,
        };

        private static Booking booking2 = new Booking()
        {
            capacity = 1000,
            startTime = DateTime.Now.AddDays(2).AddHours(2),
            endTime = DateTime.Now.AddDays(2).AddHours(5),
            location = (Location)location,
            user = "user2@me.com",
            plugs = new List<PlugType>() { PlugType.CCS },
            priority = Sopro.Models.User.UserType.EMPLOYEE,
            socStart = 50,
            socEnd = 100,
        };


        [Test]
        public void distributionTimerStartTest()
        {
            // Mock logger
            var loggerFactory = LoggerFactory.Create(builder =>
            {
                builder
                    .AddFilter("Microsoft", LogLevel.Warning)
                    .AddFilter("System", LogLevel.Warning)
                    .AddFilter("LoggingConsoleApp.Program", LogLevel.Debug)
                    .AddConsole()
                    .AddEventLog();
            });
            var logger = loggerFactory.CreateLogger<DistributionTimer>();

            // Create TimerService
            DistributionTimer t = new DistributionTimer(cache, logger);
            t.StartAsync(new System.Threading.CancellationToken());

            // Configure location
            location.schedule = new Schedule();
            location.distributor = new Distributor(location.schedule, location)
            {
                strategy = new DummyDistribution()
            };
            
            bookings = new List<Booking>() { booking1, booking2 };
            locations = new List<ILocation>() { location };

            cache.Set(CacheKeys.LOCATION, locations);
            cache.Set(CacheKeys.BOOKING, bookings);

            // Es soll ab der nächsten vollen Zeiteinheit jede Zeiteinheit verteilt werden. 
            // Wir warten also n Zeiteinheiten um sicher zu gehen, dass verteilt wurde.
            Task.Delay(180000).Wait();

            // Timer Service beenden
            t.Dispose();

            Assert.IsTrue(location.schedule.bookings.Count > 0);
        }
    }
}


