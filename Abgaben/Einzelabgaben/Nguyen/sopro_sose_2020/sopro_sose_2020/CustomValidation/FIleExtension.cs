using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace sopro_sose_2020.CustomValidation
{
    public class FileExtensionAttribute : ValidationAttribute
    {
        public string extension;
        public string GetErrorMessage() =>
       $"File extension not allowed.";
        public FileExtensionAttribute(string _extension) 
        {
            extension = _extension;
        }
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            IFormFile file = value as IFormFile;
            string fileExtension = Path.GetExtension(file.FileName);
            if (file != null)
            {
                if (!extension.Contains(fileExtension.ToLower()))
                {
                    return new ValidationResult(GetErrorMessage());
                }
            }

            return ValidationResult.Success;
        }

    }
}
