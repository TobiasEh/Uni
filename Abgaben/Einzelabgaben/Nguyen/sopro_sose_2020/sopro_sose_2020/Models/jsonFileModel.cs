using CustomModelValidation.CustomValidation;
using FoolProof.Core;
using Microsoft.AspNetCore.Http;
using sopro_sose_2020.CustomValidation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace sopro_sose_2020.Models
{
    public class jsonFileModel
    {
        [FileSize(1 * 1024 * 1024)]
        [FileExtension(".json")]
        public IFormFile file { get; set; }
    }
}
