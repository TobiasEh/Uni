using Sopro.Interfaces.PersistenceController;
using Sopro.Models.Administration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sopro.Persistence.PersBooking
{
    public class BookingService : BookingRepository, IBookingService
    {
        public List<Booking> import()
        {
            return new List<Booking>();
        }
        public void emport(List<Booking> list)
        {

        }
    }
}
