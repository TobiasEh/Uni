using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace Sopro.ValidationAttributes
{
    public class BookingEndTimeValidation : ValidationAttribute, IClientModelValidator
    {
        private DateTime endTime;
        
        public void AddValidation(ClientModelValidationContext context)
        {
            context.Attributes.Add("data-val", "true");
            context.Attributes.Add("data-val-endtime", GetErrorMessage());
        }
        public string GetErrorMessage() =>
            $"Buchungsende kann nicht vor Start sein.";


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
            endTime = Convert.ToDateTime(value);
            if (endTime > Convert.ToDateTime(property.GetValue(validationContext.ObjectInstance, null)) && endTime.Date == Convert.ToDateTime(property.GetValue(validationContext.ObjectInstance, null)).Date)
                return ValidationResult.Success;
            else
                return new ValidationResult("ErrorEndTime", new List<string>() { "endTime" });
        }
    }
}
