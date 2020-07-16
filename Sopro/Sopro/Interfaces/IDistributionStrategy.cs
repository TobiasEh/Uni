using Sopro.Models.Administration;
using System.Collections.Generic;


namespace Sopro.Interfaces
{
    public interface IDistributionStrategy
    {
        bool distribute(List<Booking> bookings, Schedule schedule, int puffer);
    }
}
