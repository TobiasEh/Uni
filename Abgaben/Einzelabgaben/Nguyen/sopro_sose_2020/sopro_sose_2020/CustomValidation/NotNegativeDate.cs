using sopro_sose_2020.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace sopro_sose_2020.CustomValidation
{
    public class NotNegativeDate : ValidationAttribute
    {
        public NotNegativeDate(DateTime _startTime)
        {
            startTime = _startTime;
        }
        public DateTime startTime;
        public string GetErrorMessage() =>
       $"End time has to be AFTER start time.";

        public override bool IsValid(object value)
        {
            var endTime = (DateTime)value;
            return endTime >= startTime;
        }
    }
}
