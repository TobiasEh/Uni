using Sopro.Interfaces.AdministrationController;
using Sopro.Interfaces.Persistence;
using System.Collections.Generic;

namespace Sopro.Interfaces.PersistenceController
{
    public interface IBookingService : IBookingRepository
    {
        public List<IBooking> import(string path);
        public void export(List<IBooking> list, string path);
    }
}
