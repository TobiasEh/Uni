using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Sopro.ValidationAttributes
{
    public class VehicleSocEndValidation : ValidationAttribute
    {
        private string socStart;
        private int socEnd;
        public VehicleSocEndValidation(string _socStart)
        {
            socStart = _socStart;
        }
        /// <summary>
        /// Überprüft ob der Nutzer einen späteren Endadestatus als Startladestatus bei der Eingabe gewählt hat.
        /// </summary>
        /// <param name="value">gewählter Endladestatus.</param>
        /// <param name="validationContext">gewählter Startladestatus.</param>
        /// <returns>
        /// erzeugt eine erfolgreiche Validierung oder eine Fehlermeldung, falls die Abfrage fehlschlägt.
        /// </returns>
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var property = validationContext.ObjectType.GetProperty("socStart");
            socEnd = Convert.ToInt16(value);
            if (socEnd >= Convert.ToInt16(property.GetValue(validationContext.ObjectInstance, null)) && socEnd <= 100 && socEnd >= 0)
                return ValidationResult.Success;
            else
                return new ValidationResult("ErrorSocEnd", new List<string>() { "socEnd" });
        }
    }
}
