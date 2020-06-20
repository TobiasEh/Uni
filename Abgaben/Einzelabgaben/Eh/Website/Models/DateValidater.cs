using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Website.Models
{
    public class DateValidater : ValidationAttribute
    {
        private string start;

        public DateValidater()
        {
            start = null;
        }
        public DateValidater(string startTime)
        {
            start = startTime;
        }


        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            DateTime date = DateTime.Parse(value.ToString());
            if(date== null)
            {
                return new ValidationResult("Zeitpunkt war nicht verhanden");
            }

            if (date < DateTime.Now)
            {
                return new ValidationResult("Zeitpunkt zu sehr in der Vergangenheit");
            }

            if (start != null)
            {
                var property = validationContext.ObjectType.GetProperty(start);
                if (property != null)
                {
                    DateTime startDate = (DateTime)property.GetValue(validationContext.ObjectInstance);
                    if(startDate > date)
                    {
                        return new ValidationResult("Ende vor Anfang");
                    }
                }
            }
            return ValidationResult.Success;
        }
    }
}
