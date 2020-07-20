using Microsoft.AspNetCore.Http;
using Sopro.ValidationAttributes;

namespace Sopro.ViewModels
{
    public class FileViewModel
    {
        [FileExtensionValidation(".json")]
        public IFormFile importedFile { get; set; }
    }
}
