using Org.BouncyCastle.Math.EC.Rfc7748;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Threading;

namespace Sopro.ValidationAttributes
{
    public class AtleastOnePlug : ValidationAttribute
    {
        private string[] plugs;
        
        public AtleastOnePlug(params string[] _plugs)
        {
            plugs = _plugs;
        }
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            List<bool> propList = new List<bool>();
            foreach (string s in plugs)
            {
                propList.Add(Convert.ToBoolean(validationContext.ObjectType.GetProperty(s).GetValue(validationContext.ObjectInstance, null)));
            }
           
            if (propList.Any(x => x))
                return ValidationResult.Success;
            else
            {
                ErrorMessage = "Mindestens 1 Plug auswählen";
                return new ValidationResult(ErrorMessage);
            }
               
        }
    }
}
