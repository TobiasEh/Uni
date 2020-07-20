using System;
using Sopro.Models.Administration;

namespace Sopro.Interfaces.CommunicationAdministration
{
    public interface INotificationListener
    {
        public void update(Booking booking, String eventName);
    }
}
