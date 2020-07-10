using Sopro.Models.Infrastructure;

namespace Sopro.Interfaces
{
    public interface ILocation
    {
        Schedule schedule { get; set; }
        Distributer distributer { get; set; }
        bool addZone(Zone zone);
        bool deleteZone(Zone zone);
    }
}
