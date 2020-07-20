using NUnit.Framework;
using Sopro.Models.Communication;
using System;
using System.Collections.Generic;
using System.Text;

namespace UnitTests.Communication
{
    [TestFixture]
    class MessengerTest
    {
        static Messenger messenger = new Messenger();

        [Test]
        public void SendMessageTest()
        {
            string s = NotificationEvent.ACCEPTED;
            string user = "saender1324@gmail.com";
            messenger.sendMessage(s, user);
        }
    }
}
