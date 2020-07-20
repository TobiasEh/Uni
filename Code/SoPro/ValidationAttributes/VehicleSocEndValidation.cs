using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Sopro.ValidationAttributes
{
    public class VehicleSocEndValidation : ValidationAttribute
    {
        private int socStart;
        private int socEnd;

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var property = validationContext.ObjectType.GetProperty("socStart");
            socStart = Convert.ToInt16(property.GetValue(validationContext.ObjectInstance, null));
            socEnd = Convert.ToInt16(value);
            if (socEnd >= socStart && socEnd <= 100 && socEnd >= 0)
                return ValidationResult.Success;
            else
                return new ValidationResult("ErrorSocEnd", new List<string>() { "socEnd" });
        }
    }
}
