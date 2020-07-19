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
        private List<Vehicle> vehicles;
        private VehicleViewModel model = new VehicleViewModel();
        private IVehicleService service = new VehicleService();

        public VehicleController(IMemoryCache _memoryCache)
        {
            cache = _memoryCache;
        }
        public IActionResult Cartemplates()
        {
            if(!cache.TryGetValue(CacheKeys.VEHICLE, out vehicles))
            {
                vehicles = new List<Vehicle>();
            }
            model.vehicles = vehicles;
            return View(model);
        }

        [HttpPost]
        public IActionResult Post(VehicleViewModel model)
        {
            var vehicle = model.vehicle;
            ModelState.Clear();
            if (!cache.TryGetValue(CacheKeys.VEHICLE, out vehicles))
            {
                vehicles = new List<Vehicle>();
            }

            vehicle.plugs = new List<PlugType>();
            if (model.CCS)
            {
                vehicle.plugs.Add(PlugType.CCS);
            }
            if (model.TYPE2)
            {
                vehicle.plugs.Add(PlugType.TYPE2);
            }

            if (!TryValidateModel(vehicle))
            {
                model.vehicles = vehicles;
                model.vehicle = vehicle;
                return View("Cartemplates", model);
            }
                
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

            foreach (Vehicle veh in importedVehicles)
            {
                if (!vehicles.Contains(veh))
                {
                    vehicles.Add(veh);
                }
            }
            cache.Set(CacheKeys.VEHICLE, vehicles);
            this.model.vehicles = vehicles;
            return View("Cartemplates", this.model);
        }

        [HttpGet]
        public IActionResult Export([FromForm] FileViewModel model)
        {
            cache.TryGetValue(CacheKeys.VEHICLE, out vehicles);
            IFormFile file = model.exportedFile;
            string path = Path.GetFullPath(file.Name);
            service.export(vehicles, path);
            this.model.vehicles = vehicles;
            return View("Cartemplates", this.model);
        }
        */
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                throw new Exception("id null");
            }
            cache.TryGetValue(CacheKeys.VEHICLE, out vehicles);
            EditViewModel model = new EditViewModel() { vehicle = vehicles[(int)id] };
            return View(model);
        }
        [HttpPost]
        public IActionResult Edit(int? id, EditViewModel model)
        {
            ModelState.Clear();
            var vehicle = model.vehicle;
            cache.TryGetValue(CacheKeys.VEHICLE, out vehicles);
            if (vehicle.plugs == null) 
            {
                vehicle.plugs = new List<PlugType>();
            }
            if (model.CCS)
            {
                
                vehicle.plugs.Add(PlugType.CCS);
            }
            else
            {
                if (vehicles[(int)id].plugs.Contains(PlugType.CCS))
                {
                    vehicle.plugs.Remove(PlugType.CCS);
                }
            }
            if (model.TYPE2)
            {
               
                vehicle.plugs.Add(PlugType.TYPE2);
            }
            else
            {
                if (vehicles[(int)id].plugs.Contains(PlugType.TYPE2))
                {
                    vehicle.plugs.Remove(PlugType.TYPE2);
                }
            }
            if (!TryValidateModel(vehicle))
                return View(new EditViewModel() { vehicle = vehicles[(int)id] });
            vehicles[(int)id] = vehicle;
            this.model.vehicles = vehicles;
            return RedirectToAction("Cartemplates",model);
        }
        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                throw new Exception("id null");
            }
            cache.TryGetValue(CacheKeys.VEHICLE, out vehicles);
            vehicles.RemoveAt((int)id);
            cache.Set(CacheKeys.VEHICLE, vehicles);
            model.vehicles = vehicles;
            return View("Cartemplates", model);
        }
    }
}

