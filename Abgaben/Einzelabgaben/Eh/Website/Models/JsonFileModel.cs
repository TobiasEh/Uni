using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Reflection;
using System.ComponentModel.DataAnnotations;

namespace Website.ViewModel
{
    public class JsonFileModel
    {

        [FileSize(1 * 1024 * 1024)]
        [FileExtension(".json")]
        [Required]
        public IFormFile file { get; set; }
    }
}
