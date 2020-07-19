using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Sopro.Interfaces.ControllerSimulation;
using Sopro.Interfaces.PersistenceController;
using Sopro.Models.Infrastructure;
using Sopro.Models.Simulation;
using Sopro.Persistence.PersVehicle;
using Sopro.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;

namespace Sopro.Controllers
{
    public class VehicleController : Controller
    {
        private IMemoryCache cache;
        private List<IDVehicle> vehicles;
        private VehicleViewModel model = new VehicleViewModel();
        private IVehicleService service = new VehicleService();

        public VehicleController(IMemoryCache _memorycache)
        {
            cache = _memorycache;
        }
        public IActionResult Cartemplates()
        {

            if(!cache.TryGetValue(CacheKeys.VEHICLE, out vehicles))
            {
                vehicles = new List<IDVehicle>();
            }
            model.vehicles = vehicles;
            return View(model);
        }

        [HttpPost]
        public IActionResult Post(IDVehicle vehicle)
        {
            if(!cache.TryGetValue(CacheKeys.VEHICLE, out vehicles))
            {
                vehicles = new List<IDVehicle>();
            }
            if (!ModelState.IsValid)
            {
                throw new Exception("Fahrzeug nicht valide!");
            }
            vehicle.id = vehicles.Count;
            vehicle.plugs = new List<PlugType>() { PlugType.CCS };
            vehicles.Add(vehicle);
            cache.Set(CacheKeys.VEHICLE, vehicles);
            model.vehicles = vehicles;
            return View("Cartemplates", model);
        }
        /*
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Import([FromForm] FileViewModel model)
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
        public IActionResult Export([FromForm] FileViewModel model)
        {
            cache.TryGetValue(CacheKeys.VEHICLE, out vehicles);
            IFormFile file = model.exportedFile;
            string path = Path.GetFullPath(file.Name);
            service.export(vehicles, path);

            return View("Index", vehicles);
        }
        */
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                throw new Exception("index not found bljad");
            }
            else
            {
                cache.TryGetValue(CacheKeys.VEHICLE, out vehicles);
                model.vehicles = vehicles;
                model.vehicle = vehicles[vehicles.IndexOf(vehicles.Find(x => x.id == (int)id))];
                return View(model);
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Editted(IDVehicle vehicle)
        {
            cache.TryGetValue(CacheKeys.VEHICLE, out vehicles);
            model.vehicles = vehicles;
            vehicles[vehicle.id] = vehicle;
            model.vehicles = vehicles;
            return View("Cartemplates");
        }
        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                throw new Exception("index not found");
            }
            else 
            {
                cache.TryGetValue(CacheKeys.VEHICLE,out vehicles);
                vehicles.RemoveAt((int)id);
                model.vehicles = vehicles;
                return View("Cartemplates",model);
            }
        }
    }
}

