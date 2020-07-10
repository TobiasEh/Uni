using Sopro.Models.Administration;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Sopro.ValidationAttributes
{
    public class BookingEndTimeValidation : ValidationAttribute
    {
        private Booking booking;
        public override bool IsValid(object value)
        {
            var starttime = booking.startTime;
            DateTime time = Convert.ToDateTime(value);
            if (time > starttime)
            {
                return true;
            }
            else
                return false;
        }
    }
}
