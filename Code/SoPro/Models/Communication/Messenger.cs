﻿using MailKit.Net.Smtp;
using MimeKit;

namespace Sopro.Models.Communication
{
    public class Messenger
    {
        private SmtpClient emailClient;
        public Messenger()
        {
            emailClient = new SmtpClient();
        }
        /* Method for sending an Email to spezific user
         */
        public void sendMessage(string message, string user)
        {
            var finalMessage = new MimeMessage();
            finalMessage.From.Add(new MailboxAddress("Ladesäulensystem MHP", "Saender1324@gmail.com"));
            finalMessage.To.Add(MailboxAddress.Parse(user));
            finalMessage.Subject = "Ladesäulenbuchung MHP";
            finalMessage.Body = new TextPart(MimeKit.Text.TextFormat.Plain) { Text = message };
            emailClient.Connect("smtp.gmail.com", 465, true);
            emailClient.Authenticate("saender1324@gmail.com", "Sander1234");
            emailClient.Send(finalMessage);
            emailClient.Disconnect(true);

        }
    }
}