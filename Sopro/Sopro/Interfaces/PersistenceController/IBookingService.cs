using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Sopro.Interfaces.AdministrationController;
using Sopro.Interfaces.Persistence;
using Sopro.Models.Administration;
using Sopro.ViewModels;
using System.Collections.Generic;

namespace Sopro.Interfaces.PersistenceController
{
    public interface IBookingService : IBookingRepository
    {
        public List<BookingExportInportViewModel> Import(IFormFile file);
        public FileContentResult export(List<BookingExportInportViewModel> list);
    }
}
