using Sopro.Models.Administration;
using Sopro.Models.Infrastructure;
using System;
using System.Collections.Generic;

namespace Sopro.Interfaces
{
    public interface ILocation
    {
        public string id { get; }
        public List<Zone> zones { get; set; }
        public string name { get; set; }
        public double emergency { get; set; }
        public Schedule schedule { get; set; }
        public Distributor distributor { get; set; }
        public DateTime normalizedDistributionTime { get; set; }

        bool addZone(Zone zone);
        bool deleteZone(Zone zone);
        
    }
}
