using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using Sopro.Interfaces.ControllerSimulation;
using Microsoft.AspNetCore.Http;
using System.IO;
using Sopro.Models.Simulation;
using Sopro.Interfaces;
using Sopro.ViewModels;
using Sopro.Interfaces.PersistenceController;
using Sopro.Persistence.PersScenario;
using System.Threading.Tasks;

namespace Sopro.Controllers
{
    public class SimulationController : Controller
    {
        private IMemoryCache cache;
        private IScenarioService service = new ScenarioService();
        private List<IScenario> scenarios;

        public SimulationController(IMemoryCache _cache)
        {
            cache = _cache;
        }

        public IActionResult Create()
        {
            Console.WriteLine("ay");
            List<ILocation> locations;
            if (!cache.TryGetValue(CacheKeys.LOCATION, out locations))
            {
                locations = new List<ILocation>();
            }
            List<Vehicle> vehicles;
            if (!cache.TryGetValue(CacheKeys.VEHICLE, out vehicles))
            {
                vehicles = new List<Vehicle>();
            }
            var model = new ScenarioCreateViewModel(locations, vehicles);
            model.rushhours.Add(new Rushhour());
            model.scenario = new Scenario();
            return View(model);
        }
        [HttpPost]
        public IActionResult Create(ScenarioCreateViewModel model)
        {
            var scenario = new Scenario();
            scenario.duration = model.scenario.duration;
            scenario.bookingCountPerDay = model.scenario.bookingCountPerDay;
            scenario.start = model.scenario.start;
            scenario.location = model.scenario.location;
            scenario.rushhours = model.rushhours;
            scenario.vehicles = model.vehicles;
            if(!cache.TryGetValue(CacheKeys.SCENARIO,out scenarios))
            {
                scenarios = new List<IScenario>();
            }
            scenarios.Add(scenario);
            cache.Set(CacheKeys.SCENARIO,scenarios);

            return View("Index");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AddRushhour(ScenarioCreateViewModel model)
        {
            Console.WriteLine("Test");
            model.rushhours.Add(new Rushhour());
            return PartialView("_addRushhour", model);
        }
       
       
       
        public IActionResult Index()
        {
            if (cache.TryGetValue(CacheKeys.SCENARIO, out scenarios))
            {
                scenarios = new List<IScenario>();
            }

            return View(scenarios);
        }

        /*
        public async Task<IActionResult> Simulate(ISimulator simulator)
        {
            // Simulation muss asynchron ausgeführt werden, da die Website sonst nicht navigierbar ist
            await Task.Run(() =>
            {
                simulator.init();
                simulator.run();
            });
            
            return RedirectToAction("Analyze", "EvaluationController", simulator.scenario);
        }
        */

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Import([FromForm] FileViewModel model)
        {
            IFormFile file = model.importedFile;
            string path = Path.GetFullPath(file.Name);
            List<IScenario> importedScenarios = service.import(path);

            if (!cache.TryGetValue(CacheKeys.SCENARIO, out scenarios))
            {
                scenarios = importedScenarios;
                cache.Set(scenarios, CacheKeys.SCENARIO);
                return View("Index", scenarios);
            }

            List<ILocation> locations;
            if (!cache.TryGetValue(CacheKeys.LOCATION, out locations))
            {
                locations = new List<ILocation>();
            }

            List<Vehicle> vehicles;
            if (!cache.TryGetValue(CacheKeys.VEHICLE, out vehicles))
            {
                vehicles = new List<Vehicle>();
            }

            foreach (IScenario sce in importedScenarios)
            {
                if (!scenarios.Contains(sce))
                {
                    scenarios.Add(sce);

                    if (!locations.Contains(sce.location))
                    {
                        locations.Add(sce.location);
                    }
                    sce.vehicles.ForEach(e => { if (!vehicles.Contains(e)) { vehicles.Add(e); }; });
                }
            }

            cache.Set(CacheKeys.LOCATION, locations);
            cache.Set(CacheKeys.VEHICLE, vehicles);
            cache.Set(CacheKeys.SCENARIO, scenarios);
            return View("Index", scenarios);
        }

        [HttpGet]
        public IActionResult Export([FromForm] FileViewModel model)
        {
            cache.TryGetValue(CacheKeys.SCENARIO, out scenarios);
            IFormFile file = model.exportedFile;
            string path = Path.GetFullPath(file.Name);
            service.export(scenarios, path);

            return View("Index", scenarios);
        }

        public IActionResult History()
        {
            return View(); 
        }

        public IActionResult Evaluation()
        {
            return View();
        }
    }
}
