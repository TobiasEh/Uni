using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using Sopro.ValidationAttributes;
using System.Linq;
using System.Threading.Tasks;

namespace Sopro.ViewModels
{
    public class FileViewModel
    {
        [FileExtensionValidation(".json")]
        public IFormFile importedFile { get; set; }
    }
}
