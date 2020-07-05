using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestProjekt.Models;
using TestProjekt.Validation;

namespace TestProjekt.ViewModel
{
    public class FileViewModel
    {
        [FileSizeAttribute(1 * 1024 * 1024)]
        [FileExtension(".json")]
        public IFormFile importedBookings { set; get; }
    }
}
