using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestProjekt.Validation;

namespace TestProjekt.Models
{
    public class FileSpec
    {
        [FileExtension(".json")]
        [FileSize(1024 * 1024)]
        public IFormFile file { get; set; }
    }
}
