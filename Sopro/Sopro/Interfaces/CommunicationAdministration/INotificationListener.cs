using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sopro.Models.Administration;
using Sopro.Models.Communication;

namespace Sopro.Interfaces.CommunicationAdministration
{
    public interface INotificationListener
    {
        public void update(Booking booking, String eventName);
    }
}
