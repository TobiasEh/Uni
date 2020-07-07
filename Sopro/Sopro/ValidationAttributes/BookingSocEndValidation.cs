using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Sopro.ValidationAttributes
{
    public class BookingSocEndValidation : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            int temp = Convert.ToInt16(value);
            if (temp >= 1 && temp <= 100)
                return true;
            else
                return false;
        }
    }
}
