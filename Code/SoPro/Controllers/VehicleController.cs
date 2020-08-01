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
    /// <summary>
    /// Kontroller Klasse für das Verwalten der Autos auf der Gui.
    /// </summary>
    public class VehicleController : Controller
    {
        private IMemoryCache cache;
        private List<IVehicle> vehicles;
        private VehicleViewModel model = new VehicleViewModel();
        private IVehicleService service = new VehicleService();
        /// <summary>
        ///  Konstruktor des Kontrollers für die Autos.
        /// </summary>
        /// <param name="_memoryCache"> Cache der Anwendung.</param>
        public VehicleController(IMemoryCache _memoryCache)
        {
            cache = _memoryCache;
        }

        /// <summary>
        /// Zeigt dem Benutzer eine Übersicht über alle Autos an.
        /// </summary>
        /// <returns>Eine Seite mit der Übersicht über alle Autos sowie ein Formular zum erstellen und bearbeiten.</returns>
        public IActionResult Cartemplates()
        {
            // Die Liste der Autos wird aus dem Cache geladen.
            if (!cache.TryGetValue(CacheKeys.VEHICLE, out vehicles))
            {
                vehicles = new List<IVehicle>();
            }

            model.vehicles = vehicles;
            return View(model);
        }

        /// <summary>
        /// Erstellen eines neuen Autos.
        /// </summary>
        /// <param name="model">Enthält die Daten des erstellten Autos.</param>
        /// <returns>Eine Seite mit der Übersicht über alle Autos sowie ein Formular zum erstellen und bearbeiten.</returns>
        public IActionResult NewVehicle(VehicleViewModel model)
        {
            // Die Liste der Autos wird aus dem Cache geladen.
            if (!cache.TryGetValue(CacheKeys.VEHICLE, out vehicles))
            {
                vehicles = new List<IVehicle>();
            }

            // Daten des Autos werden aus dem ViewModel geholt.
            var vehicle = model.vehicle;
            vehicle.plugs = new List<PlugType>();
            if (model.CCS)
            {
                vehicle.plugs.Add(PlugType.CCS);
            }
            if (model.TYPE2)
            {
                vehicle.plugs.Add(PlugType.TYPE2);
            }


            // Validierung des erstellten Autos.
            if (!TryValidateModel(vehicle))
            {
                // Validierung schlägt fehl.
                model.vehicles = vehicles;
                model.vehicle = vehicle;
                return View("Cartemplates", model);
            }
            // Auto wird dem cache und dem ViewModel hinzugefügt.
            vehicles.Add(vehicle);
            

            cache.Set(CacheKeys.VEHICLE, vehicles);

            model.vehicles = vehicles;

            model.vehicle = new Vehicle();

            return View("Cartemplates", model);
        }


        /// <summary>
        ///  Importiert die im File vorhandenen Autos.
        /// </summary>
        /// <param name="model">Json File welches die Autos enthält.</param>
        /// <returns>Eine Seite mit der Übersicht über alle Autos sowie ein Formular zum erstellen und bearbeiten.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Import([FromForm] FileViewModel model)
        {
            IFormFile file = model.importedFile;
            
            // Sollte kein File übergeben worden sein passiert nichts.
            if (file == null)
            {
                return RedirectToAction("Cartemplates");
            }

            //Übersetzten des Files in ein Viewmodel.
            List<VehicleExportImportViewModel> importedVehicles = service.import(file);

            // Die Liste der Autos wird aus dem Cache geladen.
            if (!cache.TryGetValue(CacheKeys.VEHICLE, out vehicles))
            {
                vehicles = new List<IVehicle>();
            }

            // Hinzufügen der Importierten Autos, sollten diese nicht im Cache sein.
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

        /// <summary>
        /// Export der Autos.
        /// </summary>
        /// <returns>Das Erstellte File mit einer Liste aller Autos.</returns>
        [HttpGet]
        public IActionResult Export()
        {
            // Die Liste der Autos wird aus dem Cache geladen.
            if (!cache.TryGetValue(CacheKeys.VEHICLE, out vehicles))
            {
                vehicles = new List<IVehicle>();
            }

            // Übersetzten der Auto Liste in eine ViewModel Liste.
            List<VehicleExportImportViewModel> vehiclelist = new List<VehicleExportImportViewModel>();
            foreach (IVehicle v in vehicles)
            {
                vehiclelist.Add(new VehicleExportImportViewModel(v));
            }

            // Export Vorgang.
            return service.export(vehiclelist);
        }
        
        /// <summary>
        /// Zeigt dem Benutzer ein Formular zum Editieren eines Autos an.
        /// </summary>
        /// <param name="id">Id des zu bearbeiten Auos.</param>
        /// <returns>Eine Seite für das Bearbeiten des Autos.</returns>
        public IActionResult Edit(string id)
        {
            // Die Liste der Autos wird aus dem Cache geladen.
            if (!cache.TryGetValue(CacheKeys.VEHICLE, out vehicles))
            {
                vehicles = new List<IVehicle>();
            }

            // Ermitteln des Autos.
            IVehicle modelVehicle = new Vehicle();
            foreach(IVehicle v in vehicles)
            {
                if (v.id.Equals(id))
                {
                    modelVehicle = v;
                    break;
                }
            }

            EditVehicleViewModel model = new EditVehicleViewModel() { vehicle = (Vehicle) modelVehicle };
            return View(model);
        }

        /// <summary>
        /// Beendet das Editeiren des Autos übernimmt die Änderungen.
        /// </summary>
        /// <param name="id">Id des Veränderten Autos.</param>
        /// <param name="model">Enthält die neuen Daten.</param>
        /// <returns>Eine Seite mit der Übersicht über alle Autos sowie ein Formular zum erstellen und bearbeiten.</returns>
        [HttpPost]
        public IActionResult Edit(string id, EditVehicleViewModel model)
        {
            // Die Liste der Autos wird aus dem Cache geladen.
            if (!cache.TryGetValue(CacheKeys.VEHICLE, out vehicles))
            {
                vehicles = new List<IVehicle>();
            }

            // Ermitteln des Autos.
            IVehicle vehicleCache = null;
            foreach(IVehicle v in vehicles)
            {
                if (v.id.Equals(id))
                {
                    vehicleCache = v;
                    break;
                }
            }

            // Befüllen des Autos mit den neuen Daten.
            
            var vehicle = model.vehicle;
            
            // Stecker des Autos ermitteln.
            if (vehicle.plugs == null) 
            {
                vehicle.plugs = new List<PlugType>();
            }

            if (model.CCS)
            {
                vehicle.plugs.Add(PlugType.CCS);
            }

            if (model.TYPE2)
            {
                vehicle.plugs.Add(PlugType.TYPE2);
            }
            vehicle.id = id;

            
            // Übernehmen der Daten.
            vehicleCache.model = vehicle.model;
            vehicleCache.plugs = vehicle.plugs;
            vehicleCache.socEnd = vehicle.socEnd;
            vehicleCache.socStart = vehicle.socStart;
            vehicleCache.capacity = vehicle.capacity;

            // Validiert das Auto.
            if (!TryValidateModel(vehicle))
                return View("Edit", model);

            this.model.vehicles = vehicles;
            this.model.vehicle = new Vehicle();
            cache.Set(CacheKeys.VEHICLE,vehicles);
            return RedirectToAction("Cartemplates",this.model);
        }

        /// <summary>
        /// Entfernt ein Auto aus dem Cahche.
        /// </summary>
        /// <param name="id">Id des zu löschenten Autos.</param>
        /// <returns>Eine Seite mit der Übersicht über alle Autos sowie ein Formular zum erstellen und bearbeiten.</returns>
        public IActionResult Delete(string id)
        {
            // Die Liste der Autos wird aus dem Cache geladen.
            if (!cache.TryGetValue(CacheKeys.VEHICLE, out vehicles))
            {
                vehicles = new List<IVehicle>();
            }

            // Ermitteln des Autos und entfernen aus dem Cache.
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

