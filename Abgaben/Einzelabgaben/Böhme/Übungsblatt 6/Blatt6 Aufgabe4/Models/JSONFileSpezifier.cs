using Blatt3_Aufgabe4.DataValidation;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Blatt3_Aufgabe4.Models
{
    public class JSONFileSpezifier
    {
        [FileExtension(".json")]
        [FileSize(1024 * 1024*1024)]
        public IFormFile file { get; set; }
    }
}
