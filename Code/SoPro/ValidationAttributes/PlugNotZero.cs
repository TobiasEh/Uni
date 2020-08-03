using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Org.BouncyCastle.Math.EC.Rfc7748;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Threading;

namespace Sopro.ValidationAttributes
{
    public class PowerNotZero : ValidationAttribute, IClientModelValidator
    {

        private string plug;
        public PowerNotZero(string _plug)
        {
            plug = _plug;
        }

        public void AddValidation(ClientModelValidationContext context)
        {
            context.Attributes.Add("data-val", "true");
            context.Attributes.Add("data-val-notzeropower", GetErrorMessage());
        }
        public string GetErrorMessage() =>
            $"Leistung sollte größer 0 sein.";
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
            if (Convert.ToInt16(validationContext.ObjectType.GetProperty(plug).GetValue(validationContext.ObjectInstance, null)) > 0)
            {
                if (Convert.ToInt16(value) == 0)
                {
                    ErrorMessage = "Power sollte größer 0 sein!";
                    return new ValidationResult(ErrorMessage);
                }
            }
            return ValidationResult.Success;

        }
    }
}
