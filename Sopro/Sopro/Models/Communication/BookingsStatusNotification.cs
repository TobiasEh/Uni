﻿using Sopro.Interfaces.CommunicationAdministration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Threading.Tasks;

namespace Sopro.Models.Communication
{
    public class BookingsStatusNotification : INotificationListener
    {
        private List<String> commands { get; set; }
        private Messenger messenger;
        public BookingsStatusNotification()
        {
            messenger = new Messenger();
            commands = new List<String>();
            //Indize:
            //0
            commands.Add("Ihre Buchung im Büro {0} mit den Daten:" +
                "\n\tStart-SoC: {1}," +
                "\n\tEnd-Soc: {2}," +
                "\nist hiermit ");
            //1
            commands.Add("bestätigt.\n");
            //2
            commands.Add("abgelehnt.\n");
            //3
            commands.Add("eingecheckt.\n");
            //4
            commands.Add("ausgecheckt.\n");
            //5
            commands.Add("\nIhre Ladezeit beginnt am {0} um {1} Uhr und endet am {2} um {3} Uhr.\n" +
                "Bitte begeben Sie sich, kruz bevor Ihre Ladezeit  beginnt, mit Ihrem Auto zur Ladestation mit der Nummer {4}.\n" +
                "Vergessen Sie nicht sich einzuchecken, wenn Sie Ihr Auto angesteckt haben, da ansonsten Ihr Ladeplatz womöglich weitervergeben wird.");
            //6
            commands.Add("\nBitte versuchen Sie erneu eine Buchung zu erstellen.\n" +
                "Ihre gesamten Daten waren:\n" +
                "\tStart-Soc: {0},\n" +
                "\tEnd-Soc: {1},\n" +
                "\tStartzeitpunkt: {2},\n" +
                "\tEndzeitpunkt: {3},\n" +
                "\tKapazität: {4}.\n" +
                "Wir bitten entstandene Unanehmlichkeiten zu entschuldigen.");
            //7
            commands.Add("\nSie könne jetzt Ihr Auto verlasse.\n" +
                "Zur Erinnerung: Ihre Ladezeit endet am {0} um {1} Uhr.");
            //8
            commands.Add("Bitte verlassen Sie nun die Ladestation.\n\n" +
                "Wir wünschen noch einen schönen Tag.");
        }

        /* Method sends message to user if booking was acceped, declined, checkedIn or checkedOut.
         */
        public void update(Booking booking, String eventName)
        {
            String message;
            if (eventName.Equals(NotificationEvent.ACCEPTED))
            {
                message = generateMessageAccepted(booking);
                messenger.sendMessage(message, booking.user);
            }
            else if (eventName.Equals(NotificationEvent.DECLINDED))
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

        /* Methode gererates String message for accepting a booking.
         */
        private String generateMessageAccepted(Booking booking)
        {
            String message = String.Format(commands.ElementAt(0), booking.location.name, booking.socStart, booking.socEnd) + commands.ElementAt(1) + 
                String.Format(commands.ElementAt(5), booking.startTime.ToString("dd.MM.yyyy"), booking.startTime.ToString("HH:mm:ss"), 
                booking.endTime.ToString("dd.MM.yyyy"), booking.endTime.ToString("HH:mm:ss"), booking.station.id);
            return message;

        }

        /* Methode gererates String message for declining a booking.
         */
        private String generateMessageDeclined(Booking booking)
        {
            String message = String.Format(commands.ElementAt(0), booking.location.name, booking.socStart, booking.socEnd) + commands.ElementAt(2) +
                String.Format(commands.ElementAt(6), booking.socStart, booking.socEnd, booking.startTime.ToString("dd.MM.yyyy HH:mm:ss"), 
                booking.endTime.ToString("dd.MM.yyyy HH:mm:ss"), booking.capacity);
            return message;
        }

        /* Methode gererates String message for checking in a booking.
         */
        private String generateMessageCheckIn(Booking booking)
        {
            String message = String.Format(commands.ElementAt(0), booking.location.name, booking.socStart, booking.socEnd) + commands.ElementAt(3) +
                String.Format(commands.ElementAt(7), booking.endTime.ToString("dd.MM.yyyy"), booking.endTime.ToString("HH:mm:ss"));
            return message;
        }

        /* Methode gererates String message for checking out a booking.
         */
        private String generateMessageCheckOut(Booking booking)
        {
            String message = String.Format(commands.ElementAt(0), booking.location.name, booking.socStart, booking.socEnd) + commands.ElementAt(3);
            return message;
        }
    }
}