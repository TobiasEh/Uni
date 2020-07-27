using Sopro.Interfaces.CommunicationAdministration;
using Sopro.Models.Administration;
using Sopro.Models.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Sopro.Models.Communication
{
    /// <summary>
    /// Klasse ist für die Erstellung und das Versenden der Nachrichten, die an den User geschickt werden zuständig.
    /// </summary>
    public class BookingsStatusNotification : INotificationListener
    {
        private List<string> commands { get; set; }
        private Messenger messenger;

        /// <summary>
        /// Konstruktor, in dem alle Nachrichtenfragmente erzeugt werden.
        /// </summary>
        public BookingsStatusNotification()
        {
            messenger = new Messenger();
            commands = new List<string>();
            // Indizes.
            // 0.
            commands.Add("Ihre Buchung im Büro {0} mit den Daten:" +
                "\n\tStart-SoC: {1}," +
                "\n\tEnd-Soc: {2}," +
                "\nist hiermit ");
            // 1.
            commands.Add("bestätigt.\n");
            // 2.
            commands.Add("abgelehnt.\n");
            // 3.
            commands.Add("eingecheckt.\n");
            // 4.
            commands.Add("ausgecheckt.\n");
            // 5.
            commands.Add("\nIhre Ladezeit beginnt am {0} um {1} Uhr und endet am {2} um {3} Uhr.\n" +
                "Bitte begeben Sie sich, kurz bevor Ihre Ladezeit  beginnt, mit Ihrem Auto zur Ladestation mit der Nummer {4}.\n" +
                "Vergessen Sie nicht sich einzuchecken, wenn Sie Ihr Auto angesteckt haben, da ansonsten Ihr Ladeplatz womöglich weitervergeben wird.");
            // 6.
            commands.Add("\nBitte versuchen Sie erneu eine Buchung zu erstellen.\n" +
                "Ihre gesamten Daten waren:\n" +
                "\tStart-Soc: {0},\n" +
                "\tEnd-Soc: {1},\n" +
                "\tStartzeitpunkt: {2},\n" +
                "\tEndzeitpunkt: {3},\n" +
                "\tKapazität: {4}.\n" +
                "Wir bitten entstandene Unanehmlichkeiten zu entschuldigen.");
            // 7.
            commands.Add("\nSie könne jetzt Ihr Auto verlasse.\n" +
                "Zur Erinnerung: Ihre Ladezeit endet am {0} um {1} Uhr.");
            // 8.
            commands.Add("Bitte verlassen Sie nun die Ladestation.\n\n" +
                "Wir wünschen noch einen schönen Tag.");
        }

        /// <summary>
        /// Es wird eine Nachricht generiert entsprechend der übergebenen Bezeichung, bezüglich der übergebene Buchung.
        /// Es wird die Methode zu senden von Nachrichten aufgerufen.
        /// </summary>
        /// <param name="booking">Buchung für die eine Nachricht generiet wird.</param>
        /// <param name="eventName">Bezeichung, was für eine Art von Nachricht erzeugt werden soll.</param>
        public void update(Booking booking, string eventName)
        {
            string message;
            if (eventName.Equals(NotificationEvent.ACCEPTED))
            {
                message = generateMessageAccepted(booking);
                messenger.sendMessage(message, booking.user);
            }
            else if (eventName.Equals(NotificationEvent.DECLINED))
            {
                message = generateMessageDeclined(booking);
                messenger.sendMessage(message, booking.user);
            }
            else if (eventName.Equals(NotificationEvent.CHECKIN))
            {
                message = generateMessageCheckIn(booking);
                messenger.sendMessage(message, booking.user);
            }
            else if (eventName.Equals(NotificationEvent.CHECKOUT))
            {
                message = generateMessageCheckOut(booking);
                messenger.sendMessage(message, booking.user);
            }
        }

        /// <summary>
        /// Generiert eine "Akzeptiert"-Nachricht zu entsprechenden Buchung.
        /// </summary>
        /// <param name="booking">Buchung zu der die Nachricht erzeugt wird.</param>
        /// <returns></returns>
        private string generateMessageAccepted(Booking booking)
        {
            string site = "";
            foreach (Zone item in booking.location.zones)
            {
                if (item.stations.Contains(booking.station))
                    site = item.site.ToString();
            } 
            string message = string.Format(commands.ElementAt(0), booking.location.name, booking.socStart, booking.socEnd) + commands.ElementAt(1) + 
                string.Format(commands.ElementAt(5), booking.startTime.ToString("dd.MM.yyyy"), booking.startTime.ToString("HH:mm:ss"), 
                booking.endTime.ToString("dd.MM.yyyy"), booking.endTime.ToString("HH:mm:ss"), site);
            return message;
        }

        /// <summary>
        /// Generiert eine "Abgelehnt"-Nachricht zu der entsprechenden Buchung.
        /// </summary>
        /// <param name="booking">Buhcung zu der die Nachricht erzeugt wird.</param>
        /// <returns></returns>
        private string generateMessageDeclined(Booking booking)
        {
            string message = string.Format(commands.ElementAt(0), booking.location.name, booking.socStart, booking.socEnd) + commands.ElementAt(2) +
                string.Format(commands.ElementAt(6), booking.socStart, booking.socEnd, booking.startTime.ToString("dd.MM.yyyy HH:mm:ss"), 
                booking.endTime.ToString("dd.MM.yyyy HH:mm:ss"), booking.capacity);
            return message;
        }

        /// <summary>
        /// Generiert eine "CheckIn"-Nachricht zu der entsprechenden Buchung.
        /// </summary>
        /// <param name="booking">Buchung zu der die Nachricht erzeugt wird.</param>
        /// <returns></returns>
        private string generateMessageCheckIn(Booking booking)
        {
            string message = string.Format(commands.ElementAt(0), booking.location.name, booking.socStart, booking.socEnd) + commands.ElementAt(3) +
                string.Format(commands.ElementAt(7), booking.endTime.ToString("dd.MM.yyyy"), booking.endTime.ToString("HH:mm:ss"));
            return message;
        }

        /// <summary>
        /// Generiert eine "CheckOut"-Nachricht zu der entsprechenden Buchung.
        /// </summary>
        /// <param name="booking">Buhcung zu der die Nachricht erzeugt wird.</param>
        /// <returns></returns>
        private String generateMessageCheckOut(Booking booking)
        {
            String message = String.Format(commands.ElementAt(0), booking.location.name, booking.socStart, booking.socEnd) + commands.ElementAt(3);
            return message;
        }
    }
}