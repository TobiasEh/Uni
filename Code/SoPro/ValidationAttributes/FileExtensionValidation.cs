﻿using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using System.IO;

namespace Sopro.ValidationAttributes
{
    public class FileExtensionValidation : ValidationAttribute
    {
        private readonly string extension;
        public FileExtensionValidation(string _extension)
        {
            extension = _extension;
        }
        public override bool IsValid(object value)
        {
            IFormFile file = value as IFormFile;
            if (file != null)
            {
                string _extension = Path.GetExtension(file.FileName);
                if (_extension.Equals(extension.ToLower()))
                {
                    return true;
                }
            }
            return false;
        }
    }
}