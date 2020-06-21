using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Validations
{
    public class FileSizeVal : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            var file = value as IFormFile;
            if (file != null)
            {
                if (file.Length < 1024*1024)
                {
                    return true;
                }
                else
                return false;
            }
            else
            return false;
        }
    }
}
