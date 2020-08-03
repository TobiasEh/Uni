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
    public class AtleastOnePlugStation : ValidationAttribute, IClientModelValidator
    {
       
        private string[] plugs;
        public AtleastOnePlugStation(params string[] _plugs)
        {
            plugs = _plugs;
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
            int c = 0;
            foreach (string s in plugs)
            {
                c += Convert.ToInt16(validationContext.ObjectType.GetProperty(s).GetValue(validationContext.ObjectInstance, null));
            }

            if (c > 0)
                return ValidationResult.Success;
            else
            {
                ErrorMessage = "Mindestens 1 Plug auswählen";
                return new ValidationResult(ErrorMessage);
            }
        }
    }
}
