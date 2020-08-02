using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Sopro.ValidationAttributes
{
    public class BookingSocEndValidation : ValidationAttribute, IClientModelValidator
    {
        private int socEnd;
       

        public void AddValidation(ClientModelValidationContext context)
        {
            context.Attributes.Add("data-val", "true");
            context.Attributes.Add("data-val-socend", GetErrorMessage());
        }
        public string GetErrorMessage() =>
            $"End SoC kann nicht kleiner als Start SoC sein.";

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
