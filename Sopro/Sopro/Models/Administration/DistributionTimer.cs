using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Sopro.Interfaces;

namespace Sopro.Models.Administration
{
    //TODO
    public class DistributionTimer : ITrigger, IHostedService, IDisposable
    {
        private DateTime distributionTime;
        private List<Distributor> distributor;
        private Timer _timer;
        private readonly ILogger<DistributionTimer> _logger;

        public DistributionTimer(DateTime distributionTime, ILogger<DistributionTimer> logger)
        {
            this.distributionTime = distributionTime;
            this._logger = logger;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Distribution timer is running.");
            _timer = new Timer(triggerBookingDistribution, )
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public bool triggerBookingDistribution()
        {
            return false;
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
