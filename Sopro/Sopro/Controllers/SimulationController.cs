﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sopro.Interfaces.ControllerSimulation;
using Microsoft.AspNetCore.Http;
using System.IO;
using Sopro.Models.Simulation;
using Sopro.Interfaces;

namespace Sopro.Controllers
{
    public class SimulationController : Controller
    {
        private IMemoryCache cache;
        private List<IScenario> scenarios;
        private IScenarioService service;

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult import([FromForm] FileViewModel model)
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

            cache.Set(locations, CacheKeys.LOCATION);
            cache.Set(vehicles, CacheKeys.VEHICLE);
            cache.Set(scenarios, CacheKeys.SCENARIO);
            return View("Index", scenarios);
        }

        [HttpGet]
        public IActionResult export([FromForm] FileViewModel model)
        {
            cache.TryGetValue(CacheKeys.SCENARIO, out scenarios);
            IFormFile file = model.exportedFile;
            string path = Path.GetFullPath(file.Name);
            service.exportFile(scenarios, path);

            return View("Index", scenarios);
        }
    }
}
