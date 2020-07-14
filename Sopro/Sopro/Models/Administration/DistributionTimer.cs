using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Sopro.Controllers;
using Sopro.Interfaces;
using Sopro.Models.Infrastructure;

namespace Sopro.Models.Administration
{
    public class DistributionTimer : IHostedService, IDisposable
    {
        private readonly ILogger<DistributionTimer> _logger;
        private Timer _timer;
        private IMemoryCache _cache;

        public DistributionTimer(IMemoryCache cache, ILogger<DistributionTimer> logger)
        {
            _logger = logger;
            _cache = cache;
        }

        private void triggerBookingDistribution(object state)
        {
            var cacheKey = CacheKeys.LOCATION;
            List<ILocation> locations = (List<ILocation>)_cache.Get(cacheKey);
            foreach (Location l in locations)
            {
                if (l.normalizedDistributionTime.Hour == DateTime.Now.Hour)
                {
                    l.distributor.run();
                }
            }
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Booking distribution service is running.");
            DateTime nextHour = new DateTime(0, 0, 0, DateTime.Now.Hour + 1, 0, 0);
            TimeSpan startTime = nextHour.Subtract(DateTime.Now);
            _timer = new Timer(triggerBookingDistribution, null, startTime, TimeSpan.FromHours(1));
            Console.Out.WriteLine("startAsync");
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Booking distribution service is stopping.");
            Console.Out.WriteLine("stopAsync");

            _timer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}
