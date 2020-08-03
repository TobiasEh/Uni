
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Sopro.Interfaces.ControllerHistory;
using Sopro.Interfaces.ControllerSimulation;
using Sopro.Interfaces.PersistenceController;
using Sopro.Models.Administration;
using Sopro.Models.History;
using Sopro.Models.Infrastructure;
using Sopro.Models.Simulation;
using Sopro.Persistence.PersEvaluation;
using Sopro.ViewModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sopro.Controllers
{
    public class HistoryController : Controller
    {
        private IMemoryCache cache;
        private IEvaluationService service = new EvaluationService();
        private List<IEvaluation> evaluations;


        public HistoryController(IMemoryCache _cache)
        {
            cache = _cache;
        }

        public IActionResult Evaluation(string id)
        {
            if (!cache.TryGetValue(CacheKeys.EVALUATION, out evaluations))
            {
                evaluations = new List<IEvaluation>();
            }

            foreach(IEvaluation eva in evaluations)
            {
                if (eva.scenario.id.Equals(id))
                {
                    return View("Views/Simulation/Evaluation.cshtml", new EvaluationViewModel((Evaluation)eva));
                }
            }

            return RedirectToAction("Index");
        }

        public IActionResult Index()
        {
            if (!cache.TryGetValue(CacheKeys.EVALUATION, out evaluations))
            {
                evaluations = new List<IEvaluation>();
            }

            return View(evaluations);
        }

        public IActionResult Delete(string id)
        {
            if (!cache.TryGetValue(CacheKeys.EVALUATION, out evaluations))
            {
                evaluations = new List<IEvaluation>();
            }

            foreach (IEvaluation eva in evaluations)
            {
                if (eva.scenario.id.Equals(id))
                {
                    evaluations.Remove(eva);
                    break;
                }
            }

            cache.Set(CacheKeys.EVALUATION, evaluations);

            return RedirectToAction("Index");
        }

        public IActionResult ChangeInfrastrukture(string id)
        {

            if (!cache.TryGetValue(CacheKeys.EVALUATION, out evaluations))
            {
                evaluations = new List<IEvaluation>();
            }
            IEvaluation evaluation = null;
            foreach (IEvaluation eva in evaluations)
            {
                if (eva.scenario.id.Equals(id))
                {
                    evaluation = eva;
                    break;
                }
            }
            Location l = evaluation.scenario.location.deepCopy();
            List<Booking> genBookings = new List<Booking>();
            foreach(Booking b in evaluation.scenario.generatedBookings)
            {
                genBookings.Add(new Booking()
                {
                    id = b.id,
                    capacity = b.capacity,
                    plugs = b.plugs,
                    socStart = b.socStart,
                    socEnd = b.socEnd,
                    user = b.user,
                    startTime = b.startTime,
                    endTime = b.endTime,
                    active = b.active,
                    location = l,
                    priority = b.priority
                });
            }
            ExecutedScenario newScenario = new ExecutedScenario(genBookings)
            {
                duration = evaluation.scenario.duration,
                bookingCountPerDay = evaluation.scenario.bookingCountPerDay,
                vehicles = evaluation.scenario.vehicles,
                rushhours = evaluation.scenario.rushhours,
                start = evaluation.scenario.start,
                location = l,
                locationWorkload = evaluation.scenario.locationWorkload,
                stationWorkload = evaluation.scenario.stationWorkload,
                fulfilledRequests = evaluation.scenario.fulfilledRequests,
            };
            cache.Set("ChangeInfrastrukture", newScenario);
            return View("EditLocationHistory", newScenario.location);
        }

        public IActionResult EditLocationNameAndEmergencyHistory(Location l)
        {
            ExecutedScenario scenario = null;
            if (!cache.TryGetValue("ChangeInfrastrukture", out scenario))
            {
                return RedirectToAction("Index");
            }
            scenario.location.name = l.name;
            scenario.location.emergency = l.emergency;
            return View("EditLocationHistory", scenario.location);
        }

        public IActionResult CreateZoneHistory()
        {
            ExecutedScenario scenario = null;
            if (!cache.TryGetValue("ChangeInfrastrukture", out scenario))
            {
                return RedirectToAction("Index");
            }
            char site = 'A';
            bool unique = false;
            while (!unique)
            {
                unique = true;
                foreach (Zone z in scenario.location.zones)
                {
                    if (site == z.site)
                    {
                        unique = false;
                        site++;
                        break;
                    }
                }
            }
            Zone newZone = new Zone() { site = site };
            scenario.location.zones.Add(newZone);
            return View("EditZoneHistory", new EditZoneViewModel() { zone = newZone, name = scenario.location.name, station = new Station(), site = site });
        }

        public IActionResult EditZoneHistory(char site)
        {
            ExecutedScenario scenario = null;
            if (!cache.TryGetValue("ChangeInfrastrukture", out scenario))
            {
                return RedirectToAction("Index");
            }

            foreach (Zone z in scenario.location.zones)
            {
                if (z.site == site)
                {
                    return View(new EditZoneViewModel() { zone = z, name = scenario.location.name, station = new Station(), site = site });
                }
            }
            return View();
        }

        public IActionResult EditStationHistory(EditZoneViewModel viewmodel)
        {
            ExecutedScenario scenario = null;
            if (!cache.TryGetValue("ChangeInfrastrukture", out scenario))
            {
                return RedirectToAction("Index");
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
                return View("EditZoneHistory", viewmodel);
            }
            return View("EditZoneHistory", viewmodel);
        }

        public IActionResult StartEditStationHistory(char site, int idStation)
        {
            ExecutedScenario scenario = null;
            if (!cache.TryGetValue("ChangeInfrastrukture", out scenario))
            {
                return RedirectToAction("Index");
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
                            return View("EditZoneHistory", viewmodel);
                        }
                    }
                    break;

                }
            }

            // Station nicht gefunden.
            return RedirectToAction("Index");
        }

        public IActionResult DeleteStationHistory(char site, int idStation)
        {
            ExecutedScenario scenario = null;
            if (!cache.TryGetValue("ChangeInfrastrukture", out scenario))
            {
                return RedirectToAction("Index");
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
                            return View("EditZoneHistory", viewmodel);
                        }
                    }
                    break;

                }
            }

            // Station nicht gefunden.
            return RedirectToAction("EditLocationHistory");
        }

        public IActionResult EndEditZoneHistory(EditZoneViewModel viewmodel)
        {

            ExecutedScenario scenario = null;
            if (!cache.TryGetValue("ChangeInfrastrukture", out scenario))
            {
                return RedirectToAction("Index");
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
                        RedirectToAction("EditZoneHistory", viewmodel.id, viewmodel.site);
                    }
                    break;
                }
            }
            return View("EditLocationHistory", scenario.location);
        }

        public IActionResult DeleteZoneHistory(char site)
        {
            ExecutedScenario scenario = null;
            if (!cache.TryGetValue("ChangeInfrastrukture", out scenario))
            {
                return RedirectToAction("Index");
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
            if (toBeDeleted == null)
                return RedirectToAction("EditLocationHistory");
            scenario.location.deleteZone(toBeDeleted);
            return RedirectToAction("EditLocationHistory");
        }

        public async Task<IActionResult> EndEditing()
        {
            Simulator sim = new Simulator();
            
            ExecutedScenario scenario = null;
            if (!cache.TryGetValue("ChangeInfrastrukture", out scenario))
            {
                return RedirectToAction("Index");
            }

            if (scenario.location.zones.Count == 0)
            {
                return View("EditLocationHistory", scenario.location);
            }
            sim.exScenario = scenario;
            await sim.run();
            Evaluation eva = Analyzer.analyze(sim.exScenario);

            List<IEvaluation> evaluations;
            if (!cache.TryGetValue(CacheKeys.EVALUATION, out evaluations))
            {
                evaluations = new List<IEvaluation>();
            }

            evaluations.Add(eva);
            cache.Set(CacheKeys.EVALUATION, evaluations);
            return RedirectToAction("Evaluation", "History", eva.scenario.id);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Import([FromForm]FileViewModel model)
        {
            try
            {
                IFormFile file = model.importedFile;
                List<EvaluationExportImportViewModel> importedEvaluations = service.import(file);

                if (!cache.TryGetValue(CacheKeys.EVALUATION, out evaluations))
                {
                    evaluations = new List<IEvaluation>();
                }

                List<IVehicle> vehicles;
                if (!cache.TryGetValue(CacheKeys.VEHICLE, out vehicles))
                {
                    vehicles = new List<IVehicle>();
                }

                foreach (EvaluationExportImportViewModel e in importedEvaluations)
                {
                

                
                    IEvaluation eva = e.generateEvaluation();
                    bool unique = true;
                    foreach(IEvaluation evaluation in evaluations)
                    {
                        if (eva.scenario.id.Equals(evaluation.scenario.id))
                        {   
                            unique = false;
                            break;
                        }
                    }
                    if (unique)
                    {
                        evaluations.Add(eva);
                    }
                    foreach(Vehicle v in eva.scenario.vehicles)
                    {   
                        unique = true;
                        foreach(Vehicle vehicle in vehicles)
                        {
                            if (vehicle.id.Equals(v.id))
                            {
                                unique = false;
                                break;
                            }
                        }
                        if (unique)
                        {
                            vehicles.Add(v);
                        }
                    }
                }
                cache.Set(CacheKeys.VEHICLE, vehicles);
                cache.Set(CacheKeys.EVALUATION, evaluations);
                return View("Index", evaluations);
            } 
            catch(Exception e)
            {
                return RedirectToAction("Index");
            }
        }


        [HttpGet]
        public IActionResult Export()
        {
            
            if (!cache.TryGetValue(CacheKeys.EVALUATION, out evaluations))
            {
                evaluations = new List<IEvaluation>();
            }

            List<EvaluationExportImportViewModel> exportEvaluations = new List<EvaluationExportImportViewModel>();
            foreach(IEvaluation eva in evaluations)
            {
                exportEvaluations.Add(new EvaluationExportImportViewModel(eva));
            }
            return service.export(exportEvaluations);
        }
               
    }
}
