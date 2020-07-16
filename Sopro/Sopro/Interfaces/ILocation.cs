using Sopro.Models.Administration;
using Sopro.Models.Infrastructure;
using System;
using System.Collections.Generic;

namespace Sopro.Interfaces
{
    public interface ILocation
    {
        Schedule schedule { get; set; }
        Distributor distributor { get; set; }
        bool addZone(Zone zone);
        bool deleteZone(Zone zone);
        DateTime normalizedDistributionTime { get; set; }
        string name { get; set; }
        List<Zone> zones { get; set; }
    }
}
