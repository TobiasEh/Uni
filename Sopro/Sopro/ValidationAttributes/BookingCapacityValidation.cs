using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Threading.Tasks;

namespace Sopro.ValidationAttributes
{
    public class BookingCapacityValidation : ValidationAttribute
    {

        public override bool IsValid(object value)
        {
            int temp = Convert.ToInt16(value);
            if (temp > 0)
                return true;
            else
                return false;
        }

    }

}
