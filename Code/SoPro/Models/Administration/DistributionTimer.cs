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
    /// <summary>
    /// Klasse die an jedem Standort die tägliche Buchungsverteilung aktiviert, indem
    /// sie die run()-Methode auf dem Distributor Objekt des Standortes aufruft.
    /// Diese Klasse ist als asynchrone Aufgabe implementiert.
    /// </summary>
    public class DistributionTimer : IHostedService, IDisposable
    {
        private readonly ILogger<DistributionTimer> _logger;
        private IMemoryCache _cache;
        private Timer _timer;

        /// <summary>
        /// Konstruktor des Verteilungstimers.
        /// </summary>
        /// <param name="cache">Der Cache, der die Buchungsanfragen enthält.</param>
        /// <param name="logger">Logger um Informationen auszugeben.</param>
        public DistributionTimer(IMemoryCache cache, ILogger<DistributionTimer> logger)
        {
            _logger = logger;
            _cache = cache;
        }

        /// <summary>
        /// Führe auf dem Distributor einer Location die run()-Methode aus,
        /// sofern die Buchungsverteilungszeit der aktuellen Zeit gleicht.
        /// </summary>
        /// <param name="state">Zustandsobjekt des Timers.</param>
        private async void triggerBookingDistribution(object state)
        {
            // Rufe die run()-Methoden asynchron auf.
            await Task.Run(() =>
            {
                _logger.LogInformation("Asynchoner Dienst Buchungsverteilung wird ausgeführt...");
                List<ILocation> locations; 
                if(_cache.TryGetValue(CacheKeys.LOCATION, out locations))
                {
                    foreach (ILocation l in locations)
                    {
                        _logger.LogInformation("Verteile Buchungen auf Standort...");
                        _logger.LogInformation("Verteilungszeit:" + l.normalizedDistributionTime.Hour.ToString());
                        _logger.LogInformation("Aktuelle Zeit: " + DateTime.Now.Hour.ToString());
                        if (l.normalizedDistributionTime.Hour == DateTime.Now.Hour)
                        {
                            string s = "Standort: " + l.name;
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

        /// <summary>
        /// Startet den Buchungsverteilungs Dienst.
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Buchungsverteilungs Dienst läuft...");
            // Wann die nächste Zeiteinheit startet.
            DateTime nextHour = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Hour + 1, 0, 0);
            // Zeit bis zur nächsten Zeiteinheit.
            TimeSpan startTime = nextHour.Subtract(DateTime.Now);
            // Timer ab nächster Zeiteinheit für jede folgende Zeitheinheit.
            _timer = new Timer(triggerBookingDistribution, null, startTime, TimeSpan.FromHours(1));
            return Task.CompletedTask;
        }

        /// <summary>
        /// Beendet die den Buchungsverteilungs Dienst.
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Buchungsverteilungs Dienst wird beendet...");

            _timer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}
