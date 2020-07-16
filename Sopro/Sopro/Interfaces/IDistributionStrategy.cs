using Sopro.Models.Administration;
using Sopro.ValidationAttributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sopro.Interfaces
{
    public interface IDistributionStrategy
    {
        bool distribute(List<Booking> bookings, Schedule schedule, int puffer);
    }
}
