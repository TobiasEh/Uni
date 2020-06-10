using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Blatt03.ViewModel.CustomValidation
{
    public class DateAttribute : ValidationAttribute
    {
        public DateTime date { get; }
        public DateAttribute(DateTime _date)
        {
            date = _date;
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

            return ValidationResult.Success;
        }
    }
}
