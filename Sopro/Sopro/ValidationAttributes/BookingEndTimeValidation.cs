using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace Sopro.ValidationAttributes
{
    public class BookingEndTimeValidation : ValidationAttribute
    {
        private DateTime startTime;
        private DateTime endTime;

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var property = validationContext.ObjectType.GetProperty("startTime");
            startTime = Convert.ToDateTime(property.GetValue(validationContext.ObjectInstance, null));
            endTime = Convert.ToDateTime(value);
            if (endTime > startTime)
                return ValidationResult.Success;
            else
                return new ValidationResult("ErrorEndTime", new List<string>() { "endTime" });
        }
    }
}
