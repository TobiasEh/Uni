using Sopro.Interfaces.CommunicationAdministration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sopro.Models.Communication
{
    public class BookingsStatusNotification : INotificationListener
    {
        private List<String> commands;
        private Messenger messenger;
        public void update(Booking booking, String eventName)
        {

        }
        private void generateMessageAccepted(Booking booking)
        {

        }

        private void generateMessageDeclined(Booking booking)
        {

        }
        private void generateMessageCheckIn(Booking booking)
        {

        }
        private void generateMessageCheckOut(Booking booking)
        {

        }
    }
}
