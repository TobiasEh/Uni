using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TestProjekt.Validation
{
    public class DateValidation: ValidationAttribute
    {
        public DateTime date { get; }
        public DateValidation(DateTime _date)
        {
            date = _date;
        }
        public DateValidation()
        {
            
        }
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            string test = value.ToString();
            DateTime d = DateTime.Parse(test);
            if(d == null)
            {
                return new ValidationResult("wrong date");
            }
            return ValidationResult.Success;
        }
    }
}
