using Microsoft.AspNetCore.Http;
using System;
using System.ComponentModel.DataAnnotations;
using System.IO;

namespace Website.ViewModel
{
    internal class FileExtensionAttribute : ValidationAttribute
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
            
            if (file != null)
            {
                string fileExtension = Path.GetExtension(file.FileName);
                if (!extension.Contains(fileExtension.ToLower()))
                {
                    return new ValidationResult(GetErrorMessage());
                }
            }

            return ValidationResult.Success;
        }
    }
}