using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Sopro.ViewModels;
using System.Collections.Generic;

namespace Sopro.Interfaces.PersistenceController
{
    public interface IBookingService
    {
        public List<BookingExportImportViewModel> Import(IFormFile file);
        public FileContentResult export(List<BookingExportImportViewModel> list);
    }
}
