using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TestProjekt.Validation
{
    public class FileSizeAttribute : ValidationAttribute
    {
        public long size { get; } 
        public FileSizeAttribute(long _size)
        {
            size = _size;
        }
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var file = value as IFormFile;
            if (file != null)
            {
                if(file.Length > size)
                {
                    return new ValidationResult($"Filesize should not be bigger than {size}.");
                }
            }
            return ValidationResult.Success;
        }
    }
}
