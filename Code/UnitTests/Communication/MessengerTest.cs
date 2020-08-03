
using NUnit.Framework;
using Sopro.Models.Administration;
using Sopro.Models.Communication;
using Sopro.Models.Infrastructure;
using System;
using System.Collections.Generic;
using System.Text;

namespace UnitTests.Communication
{
    class MessengerTest
    {
        [Test]
        public void test()
        {
            Booking booking = new Booking()
            {
                capacity = 120,
                user = "example@mail",
                socStart = 30,
                socEnd = 60,
                startTime = DateTime.Now + new TimeSpan(1, 0, 0, 0),
                endTime = DateTime.Now + new TimeSpan(1, 6, 0, 0),
                plugs = new List<Sopro.Models.Infrastructure.PlugType>() { Sopro.Models.Infrastructure.PlugType.CCS },
                location = new Location() { emergency = 0.05, name = "man", zones = new List<Zone>() },
            };
            Messenger.newMessage(booking, NotificationEvent.DECLINED, booking.user);
            string message = Messenger.messages[0].createMessageText();
            Console.WriteLine(message);
        }
    }
}
