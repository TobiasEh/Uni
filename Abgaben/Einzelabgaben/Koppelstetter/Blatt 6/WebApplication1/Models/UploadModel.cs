using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication1.Validations;

namespace WebApplication1.Models
{
    public class UploadModel
    {
        [FileSizeVal]
        [FileExtensionVal]
        public IFormFile file { get; set; }

    }
}
