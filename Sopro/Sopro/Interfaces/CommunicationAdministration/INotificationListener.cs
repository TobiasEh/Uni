using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sopro.Models.Communication;

namespace Sopro.Interfaces.CommunicationAdministration
{
    interface INotificationListener
    {
        void update(Booking booking, String eventName);
    }
}
