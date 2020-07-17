using Sopro.Interfaces.CommunicationAdministration;
using Sopro.Models.Communication;
using System;

namespace Sopro.Models.Administration
{
    public class NotificationManager
    {
        private INotificationListener listener { get; set; }

        public NotificationManager()
        {
            listener = new BookingsStatusNotification();
        }

        public void notify(Booking booking, String eventName)
        {
            listener.update(booking, eventName);
        }
    }
}
