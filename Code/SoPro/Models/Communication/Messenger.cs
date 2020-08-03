using Sopro.Interfaces.AdministrationController;
using Sopro.Models.Administration;
using System.Collections.Generic;

namespace Sopro.Models.Communication
{
    /// <summary>
    /// Klasse ist für das Senden von Nachrichten zuständig.
    /// </summary>
    public class Messenger
    {
        public static List<Message> messages = new List<Message>();
        
        public static void newMessage(IBooking _booking, string _eventName, string _email)
        {
            Message message = new Message()
            {
                booking = _booking,
                eventName = _eventName,
                email = _email,
            };
            addMessage(message);
        }

        private static void addMessage(Message message)
        {
            messages.Add(message);
        }

    }
}
