using Sopro.Models.Administration;
using Sopro.Models.Infrastructure;

namespace Sopro.Interfaces
{
    public interface ILocation
    {
        Schedule schedule { get; set; }
        Distributor distributor { get; set; }
        bool addZone(Zone zone);
        bool deleteZone(Zone zone);
    }
}
