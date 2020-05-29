using System;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Security.Cryptography.X509Certificates;

namespace CustomModelValidation.CustomValidation
{
    public class FileSizeAttribute : ValidationAttribute
    {
        public long size { get; }
        public string GetErrorMessage() =>
        $"Filesize should not be larger than {size}.";
        public FileSizeAttribute(long _size)
        {
            size = _size;
        }
        public override bool IsValid(object value)
        {
            long fsize = Convert.ToInt64(value);
            

            return fsize <= size;
        }
    }
}