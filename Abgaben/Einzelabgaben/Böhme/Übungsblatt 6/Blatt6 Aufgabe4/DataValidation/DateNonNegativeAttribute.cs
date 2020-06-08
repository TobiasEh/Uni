using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Blatt3_Aufgabe4.DataValidation
{
    public class DateNonNegativeAttribute : ValidationAttribute
    {
        private DateTime startTime;
        public DateNonNegativeAttribute(DateTime _startTime)
        {
            this.startTime = _startTime;
        }

        /*public override bool IsValid(object value)
        {
            DateTime endTime = (DateTime)value;
            return endTime >= startTime;
        }*/

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            DateTime endTime = (DateTime)value;
            if (endTime >= startTime)
            {
                return ValidationResult.Success;
            }
            return new ValidationResult("endTime must be greater than starTime");
        }

    }
}
