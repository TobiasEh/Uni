using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Sopro.Interfaces.ControllerHistory;
using Sopro.ViewModels;

namespace Sopro.Interfaces.PersistenceController
{
    public interface IEvaluationService
    {
        public List<EvaluationExportImportViewModel> import(IFormFile file);
        public FileContentResult export(List<EvaluationExportImportViewModel> list);
    }
}
