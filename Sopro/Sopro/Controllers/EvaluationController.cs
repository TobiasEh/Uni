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

        public IActionResult Evaluation(IEvaluation evaluation)
        {
            return View(evaluation);
        }

        public IActionResult History()
        {
            cache.TryGetValue(CacheKeys.HISTORY, out evaluations);
            return View(evaluations);
        }

        [HttpPost]
        public IActionResult Post()
        {
            return View();
        }
        public IActionResult Analyze(IEvaluatable scenario)
        {
            if (!cache.TryGetValue(CacheKeys.HISTORY))
            {
                evaluations = new List<IEvaluation>();
            }
            Analyzer analyzer = new Analyzer();
            IEvaluation evaluation = analyzer.analyze(scenario);
            evaluations.Add(evaluation);
            cache.Set(CacheKeys.HISTORY, evaluations);
            return RedirectToAction("Evaluation", evaluation);
        } 

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Import([FromForm]FileViewModel model)
        {
            IFormFile file = model.importedFile;
            string path = Path.GetFullPath(file.Name);
            List<IEvaluation> importedEvaluations = service.import(path);

            if (!cache.TryGetValue(CacheKeys.HISTORY, out evaluations))
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
            
            cache.Set(CacheKeys.HISTORY, evaluations);
            return View("Evaluation", evaluations);
        }
        [HttpGet]
        public IActionResult Export([FromForm] FileViewModel model)
        {
            cache.TryGetValue(CacheKeys.LOCATION, out evaluations);
            IFormFile file = model.exportedFile;
            string path = Path.GetFullPath(file.Name);
            service.export(evaluations, path);

            return View("Index", evaluations);
        }
    }
}
