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
using Sopro.Models.Infrastructure;
using Sopro.Persistence.PersScenario;
using Sopro.ViewModels.TestViewModels;
using Sopro.Models.History;
using Sopro.Interfaces.HistorySimulation;

namespace Sopro.Controllers
{
    public class SimulationController : Controller
    {
        private IMemoryCache cache;
        private IScenarioService service = new ScenarioService();
        private List<IScenario> scenarios;
        private List<IEvaluatable> evaluations;

        public SimulationController(IMemoryCache _cache)
        {
            cache = _cache;
        }

        public IActionResult Index()
        {
            if (!cache.TryGetValue(CacheKeys.SCENARIO, out scenarios))
            {
                scenarios = new List<IScenario>();
            }

            return View(scenarios);
        }

        public IActionResult Edit(string id)
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

            if (!cache.TryGetValue(CacheKeys.SCENARIO, out scenarios))
            {
                scenarios = new List<IScenario>();
            }

            foreach(Scenario s in scenarios)
            {
                if (s.id.Equals(id))
                {
                    locations.Add(s.location);
                    viewmodel.setVehicles(vehicles);
                    for(int i = 0; i < vehicles.Count; i++)
                    {
                        int count = 0;
                        foreach(Vehicle v in s.vehicles)
                        {
                            if(v == vehicles[i])
                            {
                                count++;
                            }
                        }
                        viewmodel.countVehicles[i] = count;
                    }
                    viewmodel.locations = locations;
                    viewmodel.scenario = s;
                    viewmodel.id = viewmodel.scenario.id;

                    cache.Set("ScenarioEdit", viewmodel.scenario);

                    scenarios.Remove(s);
                    cache.Set(CacheKeys.SCENARIO, scenarios);

                    return View("Create",viewmodel);
                }
            }

            return RedirectToAction("Index");
        }

        public IActionResult Delete(string id)
        {
            if (!cache.TryGetValue(CacheKeys.SCENARIO, out scenarios))
            {
                scenarios = new List<IScenario>();
            }

            foreach (Scenario s in scenarios)
            {
                if (s.id.Equals(id))
                {
                    scenarios.Remove(s);
                    break;
                }
            }

            return View("Index", scenarios);
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

            List<IVehicle> vehicles;
            if (!cache.TryGetValue(CacheKeys.VEHICLE, out vehicles))
            {
                vehicles = new List<IVehicle>();
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
                scenario.location.name = "Neu";
            }  
            else
            {
                foreach(ILocation l in locations)
                {
                    if (viewmodel.idLocation.Equals(l.id))
                    {
                        scenario.location = l.deepCopy();
                        break;
                    }
                }
            }
            
            for (int i = 0; i < vehicles.Count; i++)
            {
                for (int j = 0; j < viewmodel.countVehicles[i]; j++)
                {
                    scenario.vehicles.Add((Vehicle)vehicles[i]);
                }
            }

            if(viewmodel.countRushhours == 0)
            {
                return View("EditLocationScenario", scenario.location);
            }

            while (scenario.rushhours.Count > viewmodel.countRushhours)
            {
                scenario.rushhours.RemoveAt(scenario.rushhours.Count - 1);
            }

            for (int i = scenario.rushhours.Count; i < viewmodel.countRushhours; i++)
            {
                scenario.rushhours.Add(new Rushhour());
            }


            return View(scenario);
        }

        public IActionResult EditLocationScenario(Scenario s)
        {
            Scenario scenario = null;
            if (!cache.TryGetValue("ScenarioEdit", out scenario))
            {
                scenario = new Scenario();
            }
            scenario.rushhours = s.rushhours;
            return View(scenario.location);
        }

        public IActionResult EditLocationNameAndEmergencyScenario (Location l)
        {
            Scenario scenario = null;
            if (!cache.TryGetValue("ScenarioEdit", out scenario))
            {
                scenario = new Scenario();
            }
            scenario.location.name = l.name;
            scenario.location.emergency = l.emergency;
            return View("EditLocationScenario", scenario.location);
        }

        public IActionResult CreateZoneScenario()
        {
            Scenario scenario = null;
            if (!cache.TryGetValue("ScenarioEdit", out scenario))
            {
                scenario = new Scenario();
            }
            char site = 'A';
            bool unique = false;
            while (!unique)
            {
                unique = true;
                foreach (Zone z in scenario.location.zones)
                {
                    if(site == z.site)
                    {
                        unique = false;
                        site++;
                        break;
                    }
                }
            }
            Zone newZone = new Zone() { site = site };
            scenario.location.zones.Add(newZone);
            return View("EditZoneScenario", new EditZoneViewModel() { zone = newZone, name= scenario.location.name, station = new Station(), site = site});
        }

        public IActionResult EditZoneScenario(char site)
        {
            Scenario scenario = null;
            if (!cache.TryGetValue("ScenarioEdit", out scenario))
            {
                scenario = new Scenario();
            }

            foreach(Zone z in scenario.location.zones)
            {
                if (z.site == site)
                {
                    return View(new EditZoneViewModel() { zone = z , name = scenario.location.name, station = new Station(), site = site}) ;
                }
            }
            return View();
        }

        public IActionResult EditStationScenario(EditZoneViewModel viewmodel)
        {
            Scenario scenario = null;
            if (!cache.TryGetValue("ScenarioEdit", out scenario))
            {
                scenario = new Scenario();
            }

            Station station = viewmodel.station;
            // Erzeugt die Stecker der Station.
            station.plugs = new List<Plug>();
            for (int i = 0; i < viewmodel.ccs; i++)
            {
                station.addPlug(new Plug() { power = viewmodel.ccsPower, type = PlugType.CCS });
            }
            for (int i = 0; i < viewmodel.type2; i++)
            {
                station.addPlug(new Plug() { power = viewmodel.type2Power, type = PlugType.TYPE2 });
            }

            viewmodel.name = scenario.location.name;
            foreach (Zone z in scenario.location.zones)
            {
                if (viewmodel.site == z.site)
                {
                    viewmodel.zone = z;
                    break;
                }
            }

            bool idValid = false;
            int id = 0;
            while (!idValid)
            {
                idValid = true;
                foreach (Station s in viewmodel.zone.stations)
                {
                    if (id == s.id)
                    {
                        idValid = false;
                        id++;
                        break;
                    }
                }
            }
            station.id = id;

            //Validiert die Station.
            if (TryValidateModel(station))
            {
                // Zurücksetzten des viewmodels und hinzufügen der Station.
                viewmodel.zone.addStation(station);
                viewmodel.station = new Station();
                viewmodel.ccs = 0;
                viewmodel.type2 = 0;
                viewmodel.ccsPower = 0;
                viewmodel.type2Power = 0;
                station.maxParallelUseable = Math.Min(station.maxParallelUseable, station.plugs.Count);
                return View("EditZoneScenario", viewmodel);
            }
            return View("EditZoneScenario", viewmodel);
    }

        public IActionResult StartEditStationScenario(char site, int idStation)
        {
            Scenario scenario = null;
            if (!cache.TryGetValue("ScenarioEdit", out scenario))
            {
                scenario = new Scenario();
            }

            EditZoneViewModel viewmodel = new EditZoneViewModel();
            
                    viewmodel.name = scenario.location.name;

                    // Ermitteln der richtigen Station
                    foreach (Zone z in scenario.location.zones)
                    {
                        if (z.site == site)
                        {
                            // Befüllen des ViewModels mit den bereits bekannten Werten.
                            viewmodel.zone = z;
                            viewmodel.site = site;

                            // Ermitteln der richtigen Station
                            foreach (Station s in z.stations)
                            {
                                if (s.id == idStation)
                                {
                                    // Befüllen des ViewModels mit den bereits bekannten Werten.
                                    viewmodel.station = s;

                                    viewmodel.ccs = 0;
                                    viewmodel.type2 = 0;
                                    foreach (Plug p in s.plugs)
                                    {
                                        if (p.type == PlugType.CCS)
                                        {
                                            viewmodel.ccsPower = p.power;
                                            viewmodel.ccs++;
                                        }
                                        else
                                        {
                                            viewmodel.type2++;
                                            viewmodel.type2Power = p.power;
                                        }

                                    }
                                    // Entfernen der Station aus der Zone um doppelungen zu vermeiden.
                                    viewmodel.zone.deleteStation(s);
                                    return View("EditZoneScenario", viewmodel);
                                }
                            }
                            break;
                        
                }
            }

            // Station nicht gefunden.
            return RedirectToAction("Index");
        }

        public IActionResult DeleteStationScenario(char site, int idStation)
        {
            Scenario scenario = null;
            if (!cache.TryGetValue("ScenarioEdit", out scenario))
            {
                scenario = new Scenario();
            }

            EditZoneViewModel viewmodel = new EditZoneViewModel();

            viewmodel.name = scenario.location.name;

            // Ermitteln der richtigen Station
            foreach (Zone z in scenario.location.zones)
            {
                if (z.site == site)
                {
                    // Befüllen des ViewModels mit den bereits bekannten Werten.
                    viewmodel.zone = z;
                    viewmodel.site = site;

                    // Ermitteln der richtigen Station
                    foreach (Station s in z.stations)
                    {
                        if (s.id == idStation)
                        {
                            viewmodel.station = new Station();
                            // Entfernen der Station aus der Zone 
                            viewmodel.zone.deleteStation(s);
                            return View("EditZoneScenario", viewmodel);
                        }
                    }
                    break;

                }
            }

            // Station nicht gefunden.
            return RedirectToAction("EditLocationScenario");
        }

        public IActionResult EndEditZoneScenario(EditZoneViewModel viewmodel)
        {

            Scenario scenario = null;
            if (!cache.TryGetValue("ScenarioEdit", out scenario))
            {
                scenario = new Scenario();
            }

               
                    // Ermittelt die richtige Zone.
                    foreach (Zone z in scenario.location.zones)
                    {
                        if (viewmodel.site == z.site)
                        {
                            z.maxPower = viewmodel.zone.maxPower;
                            // Validiert das Model
                            if (!TryValidateModel(z))
                            {
                                RedirectToAction("EditZoneScenario", viewmodel.id, viewmodel.site);
                            }
                            break;
                        }
                    }
            return RedirectToAction("EditLocationScenario");
        }

        public IActionResult DeleteZoneSzenario(char site)
        {
            Scenario scenario = null;
            if (!cache.TryGetValue("ScenarioEdit", out scenario))
            {
                scenario = new Scenario();
            }
            Zone toBeDeleted = null;
            // Ermittelt die richtige Zone.
            foreach (Zone z in scenario.location.zones)
            {
                if (site == z.site)
                {
                    toBeDeleted = z;
                    break;
                    
                }
            }
            if (toBeDeleted == null )
                return RedirectToAction("EditLocationScenario");
            scenario.location.deleteZone(toBeDeleted);
            return RedirectToAction("EditLocationScenario");
        }

        public IActionResult EndEditing()
        {
            Scenario scenario = null;
            if (!cache.TryGetValue("ScenarioEdit", out scenario))
            {
                scenario = new Scenario();
            }

            
            if (!cache.TryGetValue(CacheKeys.SCENARIO, out scenarios))
            {
                scenarios = new List<IScenario>();
            }

            scenarios.Add(scenario);

            cache.Set(CacheKeys.SCENARIO, scenarios);
            if (!TryValidateModel(scenario))
            {
                return RedirectToAction("Edit", scenario.id);
            }

            return View("Index", scenarios);

        }
       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Import([FromForm] FileViewModel model)
        {
            
            IFormFile file = model.importedFile;
            if (file == null)
            {
                return RedirectToAction("Index");
            }
            List<ScenarioExportImportViewModel> importedScenarios = service.import(file);

            if (!cache.TryGetValue(CacheKeys.SCENARIO, out scenarios))
            {
                scenarios = new List<IScenario>();
            }


            List<IVehicle> vehicles;
            if (!cache.TryGetValue(CacheKeys.VEHICLE, out vehicles))
            {
                vehicles = new List<IVehicle>();
            }

            foreach (ScenarioExportImportViewModel s in importedScenarios)
            {
                IScenario sce = s.generateScenario();
                if (!TryValidateModel(sce))
                {
                    continue;
                }
                bool contains = false;
                foreach (IScenario scenario in scenarios)
                {
                    if (scenario.id.Equals(sce.id))
                    {
                        contains = true;
                        break;
                    }
                }
                if (!contains)
                {
                    scenarios.Add(sce);
                    foreach(Vehicle  v in sce.vehicles)
                    {
                        contains = false;
                        foreach(IVehicle vehicle in vehicles)
                        {
                            if (v.id.Equals(vehicle.id))
                            {
                                contains = true;
                                break;
                            }
                        }
                        if (!contains)
                        {
                            vehicles.Add(v);
                        }
                    }
                }
            }

            cache.Set(CacheKeys.VEHICLE, vehicles);
            cache.Set(CacheKeys.SCENARIO, scenarios);
            return View("Index", scenarios);
        }
        
        [HttpGet]
        public IActionResult Export([FromForm] FileViewModel model)
        {
            cache.TryGetValue(CacheKeys.SCENARIO, out scenarios);

            List<ScenarioExportImportViewModel> scenariolist = new List<ScenarioExportImportViewModel>();
            foreach (IScenario s in scenarios)
            {
                scenariolist.Add(new ScenarioExportImportViewModel(s));
            }

            return service.export(scenariolist);
        }

        public IActionResult History()
        {
            return View(); 
        }

        public IActionResult Evaluation(string id)
        {
            Evaluation eva;

            if (!cache.TryGetValue(CacheKeys.EVALUATION, out evaluations))
            {
                evaluations = new List<IEvaluatable>();
            }

            foreach (Evaluation e in evaluations)
            {
                if (e.sce)
            }

            return View("Evaluation", eva);
            /*
            Random rnd = new Random();

            var lstModel = new List<DataViewModel>();
            lstModel.Add(new DataViewModel
            {
                DimensionOne = "Type-2",
                Quantity = rnd.Next(10)
            });
            lstModel.Add(new DataViewModel
            {
                DimensionOne = "CCS",
                Quantity = rnd.Next(10)
            });

            return View("Evaluation", lstModel);
            */
            // return View();
        }
    }
}
