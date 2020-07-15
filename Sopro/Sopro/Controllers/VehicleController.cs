using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Sopro.Interfaces.ControllerSimulation;
using Sopro.Models.Simulation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Sopro.Controllers
{
    public class VehicleController : Controller
    {
        private IMemoryCache cache;
        private List<IVehicle> vehicles;
        private IVehicleService service;

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult import([FromForm] FileViewModel model)
        {
            IFormFile file = model.importedFile;
            string path = Path.GetFullPath(file.Name);
            List<IVehicle> importedVehicles = service.import(path);

            if (!cache.TryGetValue(CacheKeys.VEHICLE, out vehicles))
            {
                vehicles = importedVehicles;
                cache.Set(vehicles, CacheKeys.SCENARIO);
                return View("Index", vehicles);
            }

            foreach(Vehicle veh in importedVehicles)
            {
                if (!vehicles.Contains(veh))
                {
                    vehicles.Add(veh);
                }
            }

            cache.Set(vehicles, CacheKeys.VEHICLE);
            return View("Index", vehicles);
        }

        [HttpGet]
        public IActionResult export([FromForm] FileViewModel model)
        {
            cache.TryGetValue(CacheKeys.VEHICLE, out vehicles);
            IFormFile file = model.exportedFile;
            string path = Path.GetFullPath(file.Name);
            service.exportFile(vehicles, path);

            return View("Index", vehicles);
        }
    }
}

