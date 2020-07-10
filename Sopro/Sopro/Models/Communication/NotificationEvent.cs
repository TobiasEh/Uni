using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sopro.Models.Communication
{
    public static class NotificationEvent
    {
        public static String ACCEPTED = "acceptedBooking";
        public static String DECLINDED = "declinedBooking";
        public static String CHECKIN = "checkInBooking";
        public static String CHECKOUT = "checkOutBooking";
    }
}
