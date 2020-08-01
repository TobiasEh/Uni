using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Sopro.ViewModels;
using System.Collections.Generic;

namespace Sopro.Interfaces.PersistenceController
{
    public interface IScenarioService
    {
        public List<ScenarioExportImportViewModel> import(IFormFile file);
        public FileContentResult export(List<ScenarioExportImportViewModel> list);
    }
}
