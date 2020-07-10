using Sopro.Interfaces.CommunicationAdministration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sopro.Models.Administration
{
    public class NotificationManager
    {
        private INotificationListener listener { get; set; }

        public NotificationManager()
        {

        }

        public void notify(Booking booking, String eventName)
        {
            listener.update(booking, eventName);
        }
    }
}
