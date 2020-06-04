using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Blatt3_Aufgabe4.DataValidation
{
    public class DateNonNegativeAttribute : ValidationAttribute
    {
        public DateTime startTime;
        public DateNonNegativeAttribute(DateTime _startTime)
        {
            startTime = _startTime;
        }
        public override bool IsValid(object value)
        {
            var endTime = (DateTime)value;
            return endTime >= startTime;
        }

    }
}
