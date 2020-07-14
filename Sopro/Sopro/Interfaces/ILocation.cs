using Sopro.Models.Administration;
using Sopro.Models.Infrastructure;
using System.Collections.Generic;

namespace Sopro.Interfaces
{
    public interface ILocation
    {
        Schedule schedule { get; set; }
        Distributor distributor { get; set; }
        public List<Zone> zones { get; set; }
        bool addZone(Zone zone);
        bool deleteZone(Zone zone);
        public string name { get; set; }
    }
}
