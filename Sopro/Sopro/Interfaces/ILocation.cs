using Sopro.Models.Administration;
using Sopro.Models.Infrastructure;
using System;
using System.Collections.Generic;

namespace Sopro.Interfaces
{
    public interface ILocation
    {
        public Schedule schedule { get; set; }
        public Distributor distributor { get; set; }
        public bool addZone(Zone zone);
        public bool deleteZone(Zone zone);
        public List<Zone> zones { get; set; }
    }
}
