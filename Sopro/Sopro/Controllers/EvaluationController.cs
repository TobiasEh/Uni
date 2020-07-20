using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Sopro.Interfaces.ControllerHistory;
using Sopro.Interfaces.HistorySimulation;
using Sopro.Interfaces.PersistenceController;
using Sopro.Models.History;
using Sopro.Persistence.PersEvaluation;
using Sopro.ViewModels;
using System.Collections.Generic;
using System.IO;

namespace Sopro.Controllers
{
    public class EvaluationController : Controller
    {
        private IMemoryCache cache;
        private IEvaluationService service = new EvaluationService();
        private List<IEvaluation> evaluations;

        public IActionResult Evaluation(IEvaluation evaluation)
        {
            return View(evaluation);
        }

        public IActionResult History()
        {
            cache.TryGetValue(CacheKeys.EVALUATION, out evaluations);
            return View(evaluations);
        }

        [HttpPost]
        public IActionResult Post()
        {
            return View();
        }
        public IActionResult Analyze(IEvaluatable scenario)
        {
            if (!cache.TryGetValue(CacheKeys.EVALUATION, out evaluations))
            {
                evaluations = new List<IEvaluation>();
            }

            IEvaluation evaluation = Analyzer.analyze(scenario);
            evaluations.Add(evaluation);
            cache.Set(CacheKeys.EVALUATION, evaluations);
            return RedirectToAction("Evaluation", evaluation);
        } 

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Import([FromForm]FileViewModel model)
        {
            IFormFile file = model.importedFile;
            string path = Path.GetFullPath(file.Name);
            List<IEvaluation> importedEvaluations = service.import(path);

            if (!cache.TryGetValue(CacheKeys.EVALUATION, out evaluations))
            {
                evaluations = importedEvaluations;
                cache.Set(CacheKeys.EVALUATION, evaluations);
                return View("Evaluation", evaluations);
            }

            foreach(IEvaluation eva in importedEvaluations)
            {
                if (!evaluations.Contains(eva))
                {
                    evaluations.Add(eva);
                }
            }
            
            cache.Set(CacheKeys.EVALUATION, evaluations);
            return View("Evaluation", evaluations);
        }
        [HttpGet]
        public IActionResult Export([FromForm] FileViewModel model)
        {
            cache.TryGetValue(CacheKeys.LOCATION, out evaluations);

            return View("Index", evaluations);
        }
    }
}
