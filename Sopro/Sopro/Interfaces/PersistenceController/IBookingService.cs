using Sopro.Interfaces.Persistence;
using Sopro.Models.Administration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sopro.Interfaces.PersistenceController
{
    public interface IBookingService : IBookingRepository
    {
        public List<Booking> import();
        public void emport(List<Booking> list);
    }
}
