using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Sopro.ValidationAttributes
{
    public class BookingEndTimeValidation : ValidationAttribute, IClientValidatable
    {
        private string startTime;
        private DateTime endTime;
        public BookingEndTimeValidation(string _startTime)
        {
            startTime = _startTime;
        }

        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            var rule = new ModelClientValidationRule
            {
                ErrorMessage = FormatErrorMessage(metadata.DisplayName),
                ValidationType = "endtime"
            };
            rule.ValidationParameters.Add("otherpropertyname", startTime);
            yield return rule;
        }

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
            var property = validationContext.ObjectType.GetProperty(startTime);
            endTime = Convert.ToDateTime(value);
            if (endTime > Convert.ToDateTime(property.GetValue(validationContext.ObjectInstance, null)) && endTime.Date == Convert.ToDateTime(property.GetValue(validationContext.ObjectInstance, null)).Date)
                return ValidationResult.Success;
            else
                return new ValidationResult("ErrorEndTime", new List<string>() { "endTime" });
        }
    }
}
