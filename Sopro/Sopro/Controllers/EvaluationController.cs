using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Sopro.Controllers
{
    public class EvaluationController : Controller
    {
        private IMemoryCache cache;
        private IEvaluationService service;
        private List<IEvaluation> evaluations;

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult import([FromForm]FileViewModel model)
        {
            IFormFile file = model.importedFile;
            string path = Path.GetFullPath(file.Name);
            List<IEvaluation> importedEvaluations = service.import(path);

            if (!cache.TryGetValue(CacheKeys.EVALUATION, out evaluations))
            {
                evaluations = importedLocations;
            }
            foreach(IEvaluation eva in importedEvaluations)
            {
                if (evaluations.Contains())
                {
                    evaluations.Add(eva);
                }
            }
            
            cache.Set(CacheKeys.EVALUATION, evaluations);
            return View("Evaluation", evaluations);
        }
        [HttpGet]
        public IActionResult export([FromForm] FileViewModel model)
        {
            cache.TryGetValue(CacheKeys.LOCATION, out evaluations);
            IFormFile file = model.exportedFile;
            string path = Path.GetFullPath(file.Name);
            service.exportFile(evaluations, path);

            return View("Index", evaluations);
        }
    }
}
