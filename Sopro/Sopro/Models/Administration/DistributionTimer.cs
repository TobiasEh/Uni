using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Sopro.Controllers;
using Sopro.Interfaces;

namespace Sopro.Models.Administration
{
    public class DistributionTimer : IHostedService, IDisposable
    {
        private readonly ILogger<DistributionTimer> _logger;
        private IMemoryCache _cache;
        private Timer _timer;

        public DistributionTimer(IMemoryCache cache, ILogger<DistributionTimer> logger)
        {
            _logger = logger;
            _cache = cache;
        }

        private async void triggerBookingDistribution(object state)
        {
            _logger.LogInformation("Trying to distribute...");

            await Task.Run(() =>
            {
                _logger.LogInformation("Running async task distribution...");
                List<ILocation> locations; 
                if(_cache.TryGetValue(CacheKeys.LOCATION, out locations))
                {
                    foreach (ILocation l in locations)
                    {
                        _logger.LogInformation("Distributing locations...");
                        _logger.LogInformation("Distribution time:" + l.normalizedDistributionTime.Hour.ToString());
                        _logger.LogInformation("Current time: " + DateTime.Now.Hour.ToString());
                        if (l.normalizedDistributionTime.Hour == DateTime.Now.Hour)
                        {
                            string s = "Location: " + l.name;
                            _logger.LogInformation(s);
                            List<Booking> bookings;
                            if (_cache.TryGetValue(CacheKeys.BOOKING, out bookings))
                            {
                                l.distributor.run(bookings);
                            }
                        }
                    }
                }
            });
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Booking distribution service is running.");
            // Wann die nächste Zeiteinheit startet.
            DateTime nextHour = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Hour + 1, 0, 0);
            // Zeit bis zur nächsten Zeiteinheit.
            TimeSpan startTime = nextHour.Subtract(DateTime.Now);
            // Timer ab nächster Zeiteinheit für jede folgende Zeitheinheit.
            _timer = new Timer(triggerBookingDistribution, null, startTime, TimeSpan.FromHours(1));
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Booking distribution service is stopping.");

            _timer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}
