using Sopro.Interfaces.CommunicationAdministration;
using Sopro.Models.Communication;
using System;

namespace Sopro.Models.Administration
{
    /// <summary>
    /// Klasse ist für den Aufruf der Benachrichtigung des Users zuständig.
    /// </summary>
    public class NotificationManager
    {
        private INotificationListener listener { get; set; }

        /*public NotificationManager()
        {
            listener = new BookingsStatusNotification();
        }*/

        /// <summary>
        /// Ruft update Methode der Klasse listener auf.
        /// Dadurch wird ein User Benachrichtigt.
        /// </summary>
        /// <param name="booking">Die betreffende Buchung.</param>
        /// <param name="eventName">String, der darauf hinweißt welche Nachricht versand wird.</param>
        public void notify(Booking booking, String eventName)
        {
            listener.update(booking, eventName);
        }
    }
}
