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
        private List<IVehicle> vehicles;
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
                vehicles = new List<IVehicle>();
            }
            model.vehicles = vehicles;
            return View(model);
        }

        public IActionResult NewVehicle(VehicleViewModel model)
        {
            var vehicle = model.vehicle;
            ModelState.Clear();
            if (!cache.TryGetValue(CacheKeys.VEHICLE, out vehicles))
            {
                vehicles = new List<IVehicle>();
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
            model.vehicle = new Vehicle();
            return View("Cartemplates", model);
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Import([FromForm] FileViewModel model)
        {
            IFormFile file = model.importedFile;
           
            List<VehicleExportImportViewModel> importedVehicles = service.import(file);

            if (!cache.TryGetValue(CacheKeys.VEHICLE, out vehicles))
            {
                vehicles = new List<IVehicle>();
                
            }

            foreach (VehicleExportImportViewModel v in importedVehicles)
            {
                Vehicle vehicle = (Vehicle) v.generateVehicle();

                bool unique = true;
                foreach (IVehicle ve in vehicles)
                {
                    if (ve.id.Equals(vehicle.id))
                    {
                        unique = false;
                        break;
                    }

                }

                if (unique)
                {
                    vehicles.Add(vehicle);
                }
            }

            cache.Set(CacheKeys.VEHICLE, vehicles);
            this.model.vehicles = vehicles;
            return View("Cartemplates", this.model);
        }

        [HttpGet]
        public IActionResult Export()
        {
            if (!cache.TryGetValue(CacheKeys.VEHICLE, out vehicles))
            {
                vehicles = new List<IVehicle>();
            }

            List<VehicleExportImportViewModel> vehiclelist = new List<VehicleExportImportViewModel>();
            foreach (IVehicle v in vehicles)
            {
                vehiclelist.Add(new VehicleExportImportViewModel(v));
            }

            return service.export(vehiclelist);
        }
        
        public IActionResult Edit(string id)
        {
            if (id == null)
            {
                throw new Exception("id null");
            }
            if(!cache.TryGetValue(CacheKeys.VEHICLE, out vehicles))
            {
                vehicles = new List<IVehicle>();
            }
            IVehicle modelVehicle = new Vehicle();
            foreach(IVehicle v in vehicles)
            {
                if (v.id.Equals(id))
                {
                    modelVehicle = v;
                }
            }
            EditVehicleViewModel model = new EditVehicleViewModel() { vehicle = (Vehicle) modelVehicle };
            return View(model);
        }
        
        public IActionResult EndEdit(string id, EditVehicleViewModel model)
        {
            ModelState.Clear();
            var vehicle = model.vehicle;
            if (!cache.TryGetValue(CacheKeys.VEHICLE, out vehicles))
            {
                vehicles = new List<IVehicle>();
            }
            bool contains = true;
            IVehicle vehicleCache = null;

            foreach(IVehicle v in vehicles)
            {
                if (v.id.Equals(id))
                {
                    vehicleCache = v;
                }
            }

            if(vehicleCache==null)
            {
                contains = false;
                vehicleCache = vehicle;
            }
            
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
                if (vehicleCache.plugs.Contains(PlugType.CCS))
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
                if (vehicleCache.plugs.Contains(PlugType.TYPE2))
                {
                    vehicle.plugs.Remove(PlugType.TYPE2);
                }
            }
            vehicle.id = id;
            if (!TryValidateModel(vehicle))
                return View("Edit",new EditVehicleViewModel() { vehicle = (Vehicle) vehicleCache });
            if (!contains)
            {
                vehicles.Add(vehicle);
            }
            this.model.vehicles = vehicles;
            this.model.vehicle = new Vehicle();
            return RedirectToAction("Cartemplates",this.model);
        }

        public IActionResult Delete(string id)
        {
            if (id == null)
            {
                throw new Exception("id null");
            }

            if (!cache.TryGetValue(CacheKeys.VEHICLE, out vehicles))
            {
                vehicles = new List<IVehicle>();
            }

            foreach (IVehicle v in vehicles)
            {
                if (v.id.Equals(id))
                {
                    vehicles.Remove(v);
                    break;
                }
            }
            cache.Set(CacheKeys.VEHICLE, vehicles);

            model.vehicles = vehicles;

            return View("Cartemplates", model);
        }
    }
}

