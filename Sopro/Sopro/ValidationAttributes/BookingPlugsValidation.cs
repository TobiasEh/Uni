using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Sopro.ValidationAttributes
{
    public class BookingPlugsValidation : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            object[] plugarray = value as object[];

            if (plugarray.Length == plugarray.Distinct().Count())
            {
                return true;
            }
            else
                return false;
        }
    }
}
