using Sopro.Interfaces.AdministrationController;
using Sopro.Interfaces.Persistence;
using System.Collections.Generic;


namespace Sopro.Persistence.PersBooking
{
    public class BookingRepository : IBookingRepository
    {
        public List<IBooking> import()
        {
            return new List<IBooking>();
        }

        public void export(List<IBooking> list)
        {
           
        }
    }
}
