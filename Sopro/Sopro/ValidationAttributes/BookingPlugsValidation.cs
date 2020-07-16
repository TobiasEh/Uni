﻿using Sopro.Models.Infrastructure;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;


namespace Sopro.ValidationAttributes
{
    public class BookingPlugsValidation : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            
            List<PlugType> plugarray = value as List<PlugType>;
            if (plugarray.Count >= 1 && plugarray.Count() == plugarray.Distinct().Count())
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
