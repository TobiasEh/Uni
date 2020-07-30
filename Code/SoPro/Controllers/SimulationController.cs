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
using Sopro.Models.Infrastructure;

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

        public IActionResult Index()
        {
            if (cache.TryGetValue(CacheKeys.SCENARIO, out scenarios))
            {
                scenarios = new List<IScenario>();
            }

            return View(scenarios);
        }

        public IActionResult Create()
        {
            ScenarioCreateViewModel viewmodel = new ScenarioCreateViewModel();

            List<ILocation> locations;
            if (!cache.TryGetValue(CacheKeys.LOCATION, out locations))
            {
                locations = new List<ILocation>();
            }

            List<IVehicle> vehicles;
            if (!cache.TryGetValue(CacheKeys.VEHICLE, out vehicles))
            {
                vehicles = new List<IVehicle>();
            }

            if(!cache.TryGetValue(CacheKeys.SCENARIO, out scenarios))
            {
                scenarios = new List<IScenario>();
            }

            viewmodel.setVehicles(vehicles);
            viewmodel.locations = locations;
            viewmodel.scenario = new Scenario();
            viewmodel.id = viewmodel.scenario.id;

            cache.Set("ScenarioEdit", viewmodel.scenario);

            return View(viewmodel);
        }

        public IActionResult EditRushours(ScenarioCreateViewModel viewmodel)
        {
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
            Scenario scenario = null;
            if (!cache.TryGetValue("ScenarioEdit", out scenario))
            {
                scenario = new Scenario();
            }
            
            scenario.start = viewmodel.scenario.start;
            scenario.duration = viewmodel.scenario.duration;
            scenario.bookingCountPerDay = viewmodel.scenario.bookingCountPerDay;

            if (viewmodel.idLocation.Equals("new"))
            {
                scenario.location = new Location();
            }  
            else
            {
                foreach(ILocation l in locations)
                {
                    if (viewmodel.idLocation.Equals(l.id))
                    {
                        scenario.location = l.deepCopy();
                    }
                }
            }
            
            for (int i = 0; i < vehicles.Count; i++)
            {
                for (int j = 0; j < viewmodel.countVehicles[i]; j++)
                {
                    scenario.vehicles.Add(vehicles[i]);
                }
            }

            while (scenario.rushhours.Count > viewmodel.countRushhours)
            {
                scenario.rushhours.RemoveAt(scenario.rushhours.Count - 1);
            }

            for (int i = scenario.rushhours.Count; i < viewmodel.countRushhours; i++)
            {
                scenario.rushhours.Add(new Rushhour());
            }

            viewmodel.scenario = scenario;

            return RedirectToAction("EditRushours");
        }

        /*
        public IActionResult Create()
        {
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

            model = new ScenarioCreateViewModel();
            model.vehicles = vehicles;
            model.locations = locations;
            model.scenario = new Scenario();

            return View(model);
        }

        public IActionResult CreateVehicles(ScenarioCreateViewModel _model)
        {
            List<Vehicle> newVehicleList = new List<Vehicle>();
            int j = 0;
            foreach(Vehicle v in model.vehicles)
            {
                for(int i = 0; i < _model.countVehicles[j]; i++)
                {
                    newVehicleList.Add(v);
                }
                j++;
            }

            for(int i = 0; i < _model.countRushhours; i++)
            {
                model.rushhours.Add(new Rushhour());
            }
            model.idLocation = _model.idLocation;
            model.scenario.bookingCountPerDay = _model.scenario.bookingCountPerDay;
            model.scenario.duration = _model.scenario.duration;
            model.scenario.start = _model.scenario.start;

            return View("Rushour",model);
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
