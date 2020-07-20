using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Sopro.ViewModels;
using System.Collections.Generic;


namespace Sopro.Interfaces.PersistenceController
{
    public interface IVehicleService
    {
        public List<VehicleExportImportViewModel> import(IFormFile file);

        public FileContentResult export(List<VehicleExportImportViewModel> list);
    }
}
