using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace Sopro.ValidationAttributes
{
    public class BookingEndTimeValidation : ValidationAttribute
    {
        private DateTime startTime;
        private DateTime endTime;
        /// <summary>
        /// Überprüft ob der Nutzer einen späteren Endzeitpunkt als Startzeitpunkt bei der Eingabe gewählt hat.
        /// </summary>
        /// <param name="value">gewählter Endzeitpunkt.</param>
        /// <param name="validationContext">gewählter Startzeitpunkt.</param>
        /// <returns>
        /// erzeugt eine erfolgreiche Validierung oder eine Fehlermeldung, falls die Abfrage fehlschlägt.
        /// </returns>
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
