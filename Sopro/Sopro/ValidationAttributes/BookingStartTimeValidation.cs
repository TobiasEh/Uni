using System;
using System.ComponentModel.DataAnnotations;


namespace Sopro.ValidationAttributes
{
    public class BookingStartTimeValidation : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            DateTime time = Convert.ToDateTime(value);
            if (time > DateTime.Now)
            {
                return true;
            }
            else
            {
                return false;
            }     
        }
    }
}
