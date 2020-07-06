using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Validations
{
    public class FileExtensionVal : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            var file = value as IFormFile;
            string fileType = Path.GetExtension(file.FileName);
            if (file != null)
            {
                if ((".json".Equals(fileType)))
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
