using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Validations
{
    public class DateTimeVal : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            DateTime time = Convert.ToDateTime(value);
            if (time > DateTime.Now)
            {
                return true;
            }
            else
                return false;
        }
    }
}
}
