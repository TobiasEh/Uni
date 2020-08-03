using Sopro.Interfaces.AdministrationController;
using Sopro.Models.Administration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sopro.Models.Communication
{
    public class Message
    {
        public string id = Guid.NewGuid().ToString();
        public string eventName { get; set; }
        public IBooking booking { get; set; }
        public string email { get; set; }
        public bool read { get; set; } = false;

        private BookingsStatusNotification notification = new BookingsStatusNotification();

        public string createMessageText()
        {
            string message = notification.generate(booking, eventName);
            return message;
        }

    }
}
