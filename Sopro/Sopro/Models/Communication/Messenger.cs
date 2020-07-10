using System;
using System.Collections.Generic;
using System.Linq;
using MailKit.Net.Smtp;
using System.Threading.Tasks;
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
        public void sendMessage(String message, String user)
        {
            var finalMessage = new MimeMessage();
            ;
            finalMessage.To.Add(MailboxAddress.Parse(user));
            finalMessage.Subject = "Ladesäulenbuchung MHP";
            finalMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html) { Text = message };
            emailClient.Connect()

        }
    }
}
