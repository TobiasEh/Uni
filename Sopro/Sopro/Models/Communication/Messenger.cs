using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;

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

        }
    }
}
