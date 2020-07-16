using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Sopro.Interfaces.ControllerSimulation;
using Sopro.Interfaces.PersistenceController;
using Sopro.Models.Simulation;
using Sopro.ViewModels;
using System.Collections.Generic;
using System.IO;

namespace Sopro.Controllers
{
    public class VehicleController : Controller
    {
        private IMemoryCache cache;
        private List<IVehicle> vehicles;
        private IVehicleService service;


        public IActionResult Cartemplates()
        {
            cache.TryGetValue(CacheKeys.VEHICLE, out vehicles);
            return View(vehicles);
        }

        [HttpPost]
        public IActionResult Post(IVehicle vehilce)
        {
            if(!cache.TryGetValue(CacheKeys.VEHICLE, out vehicles))
            {
                vehicles = new List<IVehicle>();
            }
            if (!ModelState.IsValid)
            {
                throw new Exception("Fahrzeug nicht valide!");
            }
            vehicles.Add(vehilce);
            cache.Set(CacheKeys.VEHICLE, vehicles);
            return View("Cartemplates", vehicles);

        }

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
                cache.Set(CacheKeys.SCENARIO, vehicles);
                return View("Index", vehicles);
            }

            foreach(Vehicle veh in importedVehicles)
            {
                if (!vehicles.Contains(veh))
                {
                    vehicles.Add(veh);
                }
            }

            cache.Set(CacheKeys.VEHICLE, vehicles);
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

