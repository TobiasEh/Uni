using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NCrontab;
using Sopro.Interfaces;

namespace Sopro.Models.Administration
{
    //TODO
    public class DistributionTimer : IHostedService, IDisposable
    {
        private readonly ILogger<DistributionTimer> _logger;
        private DateTime distributionTime;
        private List<Distributor> distributor;
        private Timer _timer;

        private CrontabSchedule _schedule;
        private DateTime _nextRun;


        public DistributionTimer(DateTime distributionTime, ILogger<DistributionTimer> logger)
        {

        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
