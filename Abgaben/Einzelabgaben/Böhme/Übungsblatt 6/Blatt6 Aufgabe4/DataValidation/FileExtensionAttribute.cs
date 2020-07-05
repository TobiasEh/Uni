using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Blatt3_Aufgabe4.DataValidation
{
    public class FileExtensionAttribute:ValidationAttribute
    {
        public string extension;
        public FileExtensionAttribute(string _extension)
        {
            extension = _extension;
        }
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            IFormFile file = value as IFormFile;
            if (file == null)
            {
                return new ValidationResult("No File Selected");
            }
            if(file != null)
            {
                string fileExtension = Path.GetExtension(file.FileName);

                if (!extension.Contains(fileExtension.ToLower()))
                {
                    return new ValidationResult("File extension not allowed.");
                }
            }
            return ValidationResult.Success;
        }
    }
}
