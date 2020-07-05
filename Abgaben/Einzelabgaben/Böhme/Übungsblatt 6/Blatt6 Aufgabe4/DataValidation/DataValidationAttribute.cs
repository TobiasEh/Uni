using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Blatt3_Aufgabe4.DataValidation
{
    public class DataValidationAttribute : ValidationAttribute
    {
        /*public override bool IsValid(object value)
        {
            DateTime dateTime = Convert.ToDateTime(value);
            return dateTime >= DateTime.Now;
        }*/

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            DateTime dateTime = Convert.ToDateTime(value);
            if (dateTime > DateTime.Now)
            {
                return ValidationResult.Success;
            }
            return new ValidationResult("Time must be greater than now");
        }
    }
}
