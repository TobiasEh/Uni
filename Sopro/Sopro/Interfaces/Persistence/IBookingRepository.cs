using Sopro.Interfaces.AdministrationController;
using System.Collections.Generic;

namespace Sopro.Interfaces.Persistence
{
    public interface IBookingRepository
    {
        public List<IBooking> import();
        public void export(List<IBooking> list);
    }
}
