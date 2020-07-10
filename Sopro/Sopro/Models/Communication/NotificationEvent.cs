﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sopro.Models.Communication
{
    public static class NotificationEvent
    {
        public static string ACCEPTED = "acceptedBooking";
        public static string DECLINED = "declinedBooking";
        public static string CHECKIN = "checkInBooking";
        public static string CHECKOUT = "checkOutBooking";
    }
}
