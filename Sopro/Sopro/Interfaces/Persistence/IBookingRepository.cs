using Sopro.Models.Administration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sopro.Interfaces.Persistence
{
    public interface IBookingRepository
    {
        public List<Booking> import();
        public void emport(List<Booking> list);
    }
}
