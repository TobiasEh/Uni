using MailKit.Net.Smtp;
using MimeKit;

namespace Sopro.Models.Communication
{
    /// <summary>
    /// Klasse ist für das Senden von Nachrichten zuständig.
    /// </summary>
    public class Messenger
    {
        private SmtpClient emailClient;
        public Messenger()
        {
            emailClient = new SmtpClient();
        }

        /// <summary>
        /// Übergebene Nachricht wird an den Entsprechendnen User gesendet.
        /// </summary>
        /// <param name="message">Nachricht die gesendet werden soll.</param>
        /// <param name="user">Email-Addresse des Users, an den die Nachricht gesendet werden soll.</param>
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
