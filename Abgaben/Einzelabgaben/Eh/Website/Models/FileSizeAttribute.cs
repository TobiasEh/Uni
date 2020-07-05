using Microsoft.AspNetCore.Http;
using System;
using System.ComponentModel.DataAnnotations;

namespace Website.ViewModel
{
    internal class FileSizeAttribute : ValidationAttribute
    {
        public long size { get; }
        public string GetErrorMessage() =>
        $"Filesize should not be larger than {size}.";
        public FileSizeAttribute(long _size)
        {
            size = _size;
        }
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var file = value as IFormFile;
            if (file != null)
            {
                if (file.Length > size)
                {
                    return new ValidationResult(GetErrorMessage());
                }
            }

            return ValidationResult.Success;
        }
    }
}