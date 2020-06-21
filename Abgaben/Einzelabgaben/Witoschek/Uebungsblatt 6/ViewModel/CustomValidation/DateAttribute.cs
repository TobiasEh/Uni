using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Blatt03.ViewModel.CustomValidation
{
    public class DateAttribute : ValidationAttribute, IClientModelValidator
    {
        public DateTime date { get; }
        string comparisonProperty = null;

        public DateAttribute(DateTime _date)
        {
            date = _date;
        }
        public DateAttribute(string comparisonProperty)
        {
            this.comparisonProperty = comparisonProperty;
        }

        public DateAttribute()
        {

        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            string test = value.ToString();

            DateTime d = DateTime.Parse(test);

            if (d == null)
            {
                return new ValidationResult("Could not parse date");
            }

            if (d < DateTime.Now)
            {
                return new ValidationResult("Start date can't be in the past");
            }


            if (comparisonProperty != null)
            {
                var property = validationContext.ObjectType.GetProperty(comparisonProperty);
                var start = (DateTime)property.GetValue(validationContext.ObjectInstance);

                if (d <= start)
                {
                    return new ValidationResult("End date cannot be earlier than start date");
                }
            }

            return ValidationResult.Success;
        }

        public void AddValidation(ClientModelValidationContext context)
        {
            throw new NotImplementedException();
        }
    }
}
