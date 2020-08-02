
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Sopro.ValidationAttributes
{
    public class BookingStartTimeValidation : ValidationAttribute, IClientModelValidator
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
        public void AddValidation(ClientModelValidationContext context)
        {
            context.Attributes.Add("data-val", "true");
            context.Attributes.Add("data-val-starttime", GetErrorMessage());
        }
        public string GetErrorMessage() =>
            $"Beginn kann nicht in der Vergangenheit liegen.";

    }
}
