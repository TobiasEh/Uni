using Sopro.Interfaces.AdministrationController;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sopro.Interfaces.Persistence
{
    public interface IBookingRepository
    {
        public List<IBooking> import();
        public void export(List<IBooking> list);
    }
}
