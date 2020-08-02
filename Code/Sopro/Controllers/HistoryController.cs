
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Sopro.Interfaces.ControllerHistory;
using Sopro.Interfaces.PersistenceController;
using Sopro.Models.History;
using Sopro.Persistence.PersEvaluation;
using Sopro.ViewModels;
using System.Collections.Generic;

namespace Sopro.Controllers
{
    public class HistoryController : Controller
    {
        private IMemoryCache cache;
        private IEvaluationService service = new EvaluationService();
        private List<IEvaluation> evaluations;


        public HistoryController(IMemoryCache _cache)
        {
            cache = _cache;
        }

        public IActionResult Evaluation(string id)
        {
            if (!cache.TryGetValue(CacheKeys.EVALUATION, out evaluations))
            {
                evaluations = new List<IEvaluation>();
            }

            foreach(IEvaluation eva in evaluations)
            {
                if (eva.scenario.id.Equals(id))
                {
                    return View("Views/Simulation/Evaluation.cshtml", new EvaluationViewModel((Evaluation)eva));
                }
            }

            return RedirectToAction("Index");
        }

        public IActionResult Index()
        {
            if (!cache.TryGetValue(CacheKeys.EVALUATION, out evaluations))
            {
                evaluations = new List<IEvaluation>();
            }

            return View(evaluations);
        }

        public IActionResult Delete(string id)
        {
            if (!cache.TryGetValue(CacheKeys.EVALUATION, out evaluations))
            {
                evaluations = new List<IEvaluation>();
            }

            foreach (IEvaluation eva in evaluations)
            {
                if (eva.scenario.id.Equals(id))
                {
                    evaluations.Remove(eva);
                    break;
                }
            }

            cache.Set(CacheKeys.EVALUATION, evaluations);

            return RedirectToAction("Index");
        }
            
            /*
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
               */
    }
}
