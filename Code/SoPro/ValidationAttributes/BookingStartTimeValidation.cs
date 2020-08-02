using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Sopro.ValidationAttributes
{
    public class BookingStartTimeValidation : ValidationAttribute
    {
        /// <summary>
        /// Überprüft ob der Nutzer einen sinnvollen Startzeitpunkt gewählt hat.
        /// </summary>
        /// <param name="value">gewählter Startzeitpunkt.</param>
        /// <returns>
        /// validiert oder falsifiziert den Variableninhalt.
        /// </returns>
        public override bool IsValid(object value)
        {
            DateTime time = Convert.ToDateTime(value);
            if (time > DateTime.Now)
            {
                return true;
            }
            else
            {
                return false;
            }     
        }
        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            ModelClientValidationRule rule = new ModelClientValidationRule();
            rule.ErrorMessage = FormatErrorMessage(metadata.GetDisplayName());
            rule.ValidationType = "BookingStartTimeValidation";
            yield return rule;
        }
    }
}
