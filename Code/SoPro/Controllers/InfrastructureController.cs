using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Sopro.Interfaces;
using Sopro.Interfaces.AdministrationController;
using Sopro.Interfaces.PersistenceController;
using Sopro.Models.Administration;
using Sopro.Models.Infrastructure;
using Sopro.Models.Simulation;
using Sopro.Persistence.PersLocation;
using Sopro.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;

namespace Sopro.Controllers
{
    /// <summary>
    /// Kontroller Klasse für das Verwalten der Infrastruktur auf der Gui.
    /// </summary>
    public class InfrastructureController : Controller
    {
        private IMemoryCache cache;
        private List<ILocation> locations;
        private ILocationService service = new LocationService();

        /// <summary>
        /// Konstruktor des Kontrollers für die Buchungen.
        /// </summary>
        /// <param name="_cache"> Cache der Anwendung.</param>
        public InfrastructureController(IMemoryCache _cache)
        {
            cache = _cache;
        }
        
        /// <summary>
        /// Zeigt dem Benutzer eine Übersicht aller Standorte an.
        /// </summary>
        /// <returns>Eine Seite mit einer Übersicht über aller Standorte.</returns>
        public IActionResult Index()
        {
            // Die Liste der Standorte wird aus dem Cache geladen.
            if (!cache.TryGetValue(CacheKeys.LOCATION, out locations))
            {
                locations = new List<ILocation>();
            }

            return View(new InfrastructureViewModel() { locations = locations });
        }

        /// <summary>
        /// Erstellt einen neuen Standort.
        /// Initialisiert ihn.
        /// </summary>
        /// <returns>Eine Seite mit einer Übersicht über aller Standorte.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult NewLocation()
        {
            // Erstellen eines neuen Standorts und mit startwerten initialisieren.
            ILocation l = new Location();
            l.name = "Neu";
            l.emergency = 0;
            l.zones = new List<Zone>();

            // Die Liste der Standorte wird aus dem Cache geladen.
            if (!cache.TryGetValue(CacheKeys.LOCATION, out locations))
            {
                locations = new List<ILocation>();
            }
            
            // Der neue Standort wird dem Cache hinzugefügt.
            locations.Add(l);
            cache.Set(CacheKeys.LOCATION, locations);

            return RedirectToAction("Index");
        }

        /// <summary>
        /// Entfernen des Standorts mit der übergebenen Id aus dem Cahce
        /// </summary>
        /// <param name="id"> Id des zu entfernenden Standorts.</param>
        /// <returns>Eine Seite mit einer Übersicht über aller Standorte.</returns>
        public IActionResult DeleteLocation(string id)
        {
            // Die Liste der Standorte wird aus dem Cache geladen.
            if (!cache.TryGetValue(CacheKeys.LOCATION, out locations))
            {
                locations = new List<ILocation>();
            }

            // Der Standort wird ermittelt und aus dem Cache geholt.
            if (locations.RemoveAll( x => x.id == id) == 1)
            {
                cache.Set(CacheKeys.LOCATION, locations);
            }
            return RedirectToAction("Index");
        }

        /// <summary>
        /// Übernimmt die vom Benutzer übergebenen Werte für die Bearbeitung.
        /// </summary>
        /// <param name="viewmodel">Enthält die Werte, welche übernommen werden sollen.</param>
        /// <returns>Eine Seite mit einer Übersicht über aller Standorte.</returns>
        public IActionResult EditLocation(InfrastructureViewModel viewmodel) 
        {
            // Die Liste der Standorte wird aus dem Cache geladen.
            if (!cache.TryGetValue(CacheKeys.LOCATION, out locations))
            {
                locations = new List<ILocation>();
            }
            // Der Standort wird ermittelt und die Werte übertragen.
            foreach (ILocation l in locations)
            {
                if(viewmodel.id == l.id)
                {
                    if(viewmodel.emergency != null) { 
                    double emergency = Convert.ToDouble(viewmodel.emergency.Replace(".",","));
                    l.emergency = emergency;
                    }
                    if(viewmodel.name != null)
                    {
                        l.name = viewmodel.name;
                    }
                    if (viewmodel.distributionTime != null)
                    {
                        l.normalizedDistributionTime = viewmodel.distributionTime;
                    }
                    break;
                }
            }
            //cache.Set(CacheKeys.LOCATION, locations);
            return RedirectToAction("Index");
        }

        /// <summary>
        /// Fügt einem Standort eine neue Zone hinzu.
        /// </summary>
        /// <param name="id">Id des Standorts, welchem eine neue Zone hinzugefügt werden.</param>
        /// <returns>Eine Seite auf der man die Zone bearbeiten kann.</returns>
        public IActionResult CreateZone(string id)
        {
            Zone zone = new Zone();
            char site = 'A';
            bool siteValid = false;

            // Die Liste der Standorte wird aus dem Cache geladen.
            if (!cache.TryGetValue(CacheKeys.LOCATION, out locations))
            {
                locations = new List<ILocation>();
            }

            EditZoneViewModel viewmodel = new EditZoneViewModel();
            // Ermittelt den Standort richtigen Standort mithilfe der Id.
            ILocation location = null;
            foreach (ILocation l in locations)
            {
                if (l.id.Equals(id.ToString()))
                {
                    location = l;
                    break;
                    
                }
            }

            // Ermittelt einen Validen site char und setzen.
            while (!siteValid)
            {
                siteValid = true;
                foreach (Zone z in location.zones)
                {
                    if (z.site == site)
                    {
                        siteValid = false;
                        site++;
                        break;
                    }
                }
            }
            zone.site = site;
            // Initialisieren der Zone und hinzufügen zum Standort.
            zone.stations= new List<Station>();
            location.addZone(zone);
               
            // Befüllen des Viemodels für die Seite.
            viewmodel.name = location.name;
            viewmodel.id = id;
            viewmodel.site = site;
            viewmodel.zone = zone;
            viewmodel.station = new Station();

            return View("EditZone", viewmodel);
        }

        /// <summary>
        /// Entfernt die Zone aus der Location.
        /// </summary>
        /// <param name="id">Id der Location.</param>
        /// <param name="site">Site der zu Löschenden Zone.</param>
        /// <returns> Eine Seite mit einer Übersicht über aller Standorte</returns>
        public IActionResult DeleteZone(string id, char site)
        {
            // Die Liste der Standorte wird aus dem Cache geladen.
            if (!cache.TryGetValue(CacheKeys.LOCATION, out locations))
            {
                locations = new List<ILocation>();
            }

            // Ermitteln und entfernen der zu Löschenden Zone.
            foreach (ILocation l in locations)
            {
                if (l.id.Equals(id))
                {
                    foreach (Zone z in l.zones)
                    {
                        if (z.site == site)
                        {
                            bool test =l.deleteZone(z);

                            // Weiterleitung an auf die Seite mit einer Übersicht über aller Standorte.
                            return View("Index",new InfrastructureViewModel() { locations = locations });
                        }
                    }
                    break;
                }
            }

            // Weiterleitung an auf die Seite mit einer Übersicht über aller Standorte.
            return View("Index", new InfrastructureViewModel() { locations = locations }); ;
        }

        /// <summary>
        /// Zeigt eine Seite, welche das bearbeiten einer Zone ermöglicht.
        /// Sollte die Zone nicht gefunden werden, wird an die Index Methode weitergeleitet.
        /// </summary>
        /// <param name="id">ID der Infrastruktur, zu welcher die zu bearbeitende Zone gehört.</param>
        /// <param name="site"> Die zu bearbeitende Zone.</param>
        /// <returns>Seite, auf welcher man die Zone bearbeiten kann.</returns>
        [ValidateAntiForgeryToken]
        public IActionResult EditZone(string id, char site)
        {
            // Die Liste der Standorte wird aus dem Cache geladen.
            if (!cache.TryGetValue(CacheKeys.LOCATION, out locations))
            {
                locations = new List<ILocation>();
            }

            // Ermitteln der Location.
            EditZoneViewModel viewmodel;
            foreach(ILocation l in locations)
            {
                if (l.id.Equals(id.ToString()))
                {
                    // Vorbereiten des Viewmodels, für die Seite.
                    viewmodel = new EditZoneViewModel();
                    viewmodel.name = l.name;
                    viewmodel.id = id;
                    viewmodel.station = new Station();

                    // Ermitteln der richtigen Zone, befüllen des VieModels und anzeigen der Seite.
                    foreach(Zone z in l.zones)
                    {
                        if (z.site == site)
                        {
                            viewmodel.zone = z;
                            viewmodel.site = site;
                            return View(viewmodel);
                        }
                    }
                    break;
                }
            }
            // Sollte die Zone nicht gefunden werden wird auf die Index Methode weitergeleitet.
            return RedirectToAction("Index");
        }

        /// <summary>
        /// Beendet den Bearbeitungsvorgang einer Zone.
        /// </summary>
        /// <param name="viewmodel">Enthalt alle Daten</param>
        /// <returns>Eine Seite mit einer Übersicht über aller Standorte</returns>
        public IActionResult EndEdit(EditZoneViewModel viewmodel)
        {
            // Die Liste der Standorte wird aus dem Cache geladen.
            if (!cache.TryGetValue(CacheKeys.LOCATION, out locations))
            {
                locations = new List<ILocation>();
            }

            // Ermitteln des richtigen Standorts.
            foreach (ILocation location in locations)
            {
                if (viewmodel.id.ToString().Equals(location.id))
                {
                    // Ermittelt die richtige Zone.
                    foreach (Zone z in location.zones)
                    {
                        if (viewmodel.site == z.site)
                        {
                            z.maxPower = viewmodel.zone.maxPower;
                            // Validiert das Model
                            if (!TryValidateModel(z))
                            {
                                RedirectToAction("EditZone", viewmodel.id, viewmodel.site);
                            }
                        }
                    }
                    break;
                }
            }

            return RedirectToAction("Index");
        }


        /// <summary>
        /// Erstellt eine neue Ladestation und fügt diese der Zone hinzu.
        /// </summary>
        /// <param name="viewmodel">Enthält die nötigen Daten.</param>
        /// <returns>Seite, auf welcher man die Zone bearbeiten kann.</returns>
        public IActionResult EditStation(EditZoneViewModel viewmodel) 
        {
            Station station = viewmodel.station;
            // Erzeugt die Stecker der Station.
            station.plugs = new List<Plug>();
            for(int i = 0; i < viewmodel.ccs;i++)
            {
                station.addPlug(new Plug() { power=viewmodel.ccsPower, type=PlugType.CCS});
            }
            for (int i = 0; i < viewmodel.type2;i++)
            {
                station.addPlug(new Plug() { power = viewmodel.type2Power, type = PlugType.TYPE2 });
            }

            // Die Liste der Standorte wird aus dem Cache geladen.
            if (!cache.TryGetValue(CacheKeys.LOCATION, out locations))
            {
                locations = new List<ILocation>();
            }

            // Ermitteln des richtigen Standorts.
            foreach (ILocation location in locations)
            {
                if (viewmodel.id.ToString().Equals(location.id))
                {
                    // Ermittelt die richtige Zone.
                    viewmodel.name = location.name;
                    foreach(Zone z in location.zones)
                    {
                        if (viewmodel.site == z.site)
                        {
                            viewmodel.zone = z;
                            break;
                        }
                    }
                    break;
                }
            }
            //Ermittelt eine Valide Id.
            bool idValid = false;
            int id = 0;
            while (!idValid)
            {
                idValid = true;
                foreach(Station s in viewmodel.zone.stations)
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
                station.maxParallelUseable= Math.Min( station.maxParallelUseable, station.plugs.Count);
                return View("EditZone", viewmodel);
            }
            return View("EditZone", viewmodel);
        }

        /// <summary>
        /// Löschen der Station.
        /// </summary>
        /// <param name="_id">Id der Location</param>
        /// <param name="_site">Site der Zone</param>
        /// <param name="idStation">id der Station, welche zu Löschen ist.</param>
        /// <returns>Seite, auf welcher man die Zone bearbeiten kann.</returns>
        public IActionResult DeleteStation(string _id, char _site, int idStation) 
        {
            // Die Liste der Standorte wird aus dem Cache geladen.
            if (!cache.TryGetValue(CacheKeys.LOCATION, out locations))
            {
                locations = new List<ILocation>();
            }

            // Ermitteln des richtigen Standorts.
            EditZoneViewModel viewmodel = new EditZoneViewModel();
            foreach (ILocation l in locations)
            {
                if (l.id.Equals(_id))
                {
                    // Befüllen des ViewModels mit den bereits bekannten Werten.
                    viewmodel.name = l.name;
                    viewmodel.id= _id;

                    // Ermittelt die richtige Zone.
                    foreach (Zone z in l.zones)
                    {
                        if (z.site == _site)
                        {
                            // Befüllen des ViewModels mit den bereits bekannten Werten.
                            viewmodel.site = z.site;
                            viewmodel.zone = z;

                            // Ermitteln der richtigen Station
                            foreach(Station s in z.stations)
                            {
                                if(s.id == idStation)
                                {
                                    // entfernen der Station.
                                    z.deleteStation(s);

                                    viewmodel.station = new Station();
                                    return View("EditZone", viewmodel);
                                }
                            }
                            break;
                        }
                    }
                    break;
                }
            }
            // Station nicht gefunden.
            return RedirectToAction("Index");
        }
        
        /// <summary>
        /// Beginnt das Bearbeiten einer Station.
        /// </summary>
        /// <param name="id">Id der Location</param>
        /// <param name="site">Site der Zone</param>
        /// <param name="idStation">Id der zu bearbeitenden Station.</param>
        /// <returns>Lädt die Daten aus der Station in das Create Formular.</returns>
        public IActionResult StartEditStation(string id, char site, int idStation)
        {
            // Die Liste der Standorte wird aus dem Cache geladen.
            if (!cache.TryGetValue(CacheKeys.LOCATION, out locations))
            {
                locations = new List<ILocation>();
            }

            // Ermitteln des richtigen Standorts.
            EditZoneViewModel viewmodel = new EditZoneViewModel();
            foreach (ILocation l in locations)
            {
                if (l.id.Equals(id.ToString()))
                {
                    // Befüllen des ViewModels mit den bereits bekannten Werten.
                    viewmodel.name = l.name;
                    viewmodel.id =  id;

                    // Ermitteln der richtigen Station
                    foreach (Zone z in l.zones)
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
                                    foreach(Plug p in s.plugs)
                                    {
                                        if(p.type == PlugType.CCS)
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
                                    return View("EditZone",viewmodel);
                                }
                            }
                            break;
                        }
                    }
                    break;
                }
            }

            // Station nicht gefunden.
            return RedirectToAction("Index");
        }


        /// <summary>
        /// Importiert die im File vorhandene Infrastruktur.
        /// </summary>
        /// <param name="model">Json File welches die Infrastruktur enthält.</param>
        /// <returns>Eine Seite mit einer Übersicht über aller Standorte.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Import([FromForm]FileViewModel model)
        {
            IFormFile file = model.importedFile;
            // Sollte kein File übergeben worden sein passiert nichts.
            if (file == null)
            {
                return RedirectToAction("Index");
            }

            //Übersetzten des Files in ein Viewmodel.
            List<LocationExportImportViewModel> importedLocations = service.import(file);

            // Die Liste der Standorte wird aus dem Cache geladen.
            if (!cache.TryGetValue(CacheKeys.LOCATION, out locations))
            {
                locations = new List<ILocation>();
            }

            // Hinzufügen der Importierten Locations, sollten sie nicht im Cache sein.
            foreach (LocationExportImportViewModel l in importedLocations)
            {
                ILocation location = l.generateLocation();
                bool notIncluded = true;
                foreach(ILocation loc in locations) { 
                    if (loc.id.Equals(location.id))
                    {
                        notIncluded = false;
                        break;
                    }
                }
                if (notIncluded)
                {
                    locations.Add(location);
                }
            }

            // Setzten des Caches.
            cache.Set(CacheKeys.LOCATION, locations);

            return RedirectToAction("Index");
        }

        /// <summary>
        /// Export der Infrastruktur.
        /// </summary>
        /// <returns>Das Erstellte File mit der Infrastruktur.</returns>
        [HttpGet]
        public IActionResult Export()
        {
            // Die Liste der Standorte wird aus dem Cache geladen.
            if (!cache.TryGetValue(CacheKeys.LOCATION, out locations))
            {
                locations = new List<ILocation>();
            }

            // Übersetzten der Standort Liste in eine ViewModel Liste.
            List<LocationExportImportViewModel> locationsExport = new List<LocationExportImportViewModel>();
            foreach(ILocation l in locations)
            {
                locationsExport.Add(new LocationExportImportViewModel(l));
            }

            // Exportieren.
            return service.export(locationsExport);

        }


        /// <summary>
        /// Methode zum testen und Vorführen der Zuteilung von Buchungen.
        /// </summary>
        /// <param name="id">Id des Standorts auf dem Zugeteilt werden soll.</param>
        /// <returns>Eine Seite mit einer Übersicht über aller Standorte.</returns>
        public IActionResult zuteilen(string id)
        {
            if (!cache.TryGetValue(CacheKeys.LOCATION, out locations))
            {
                locations = new List<ILocation>();
            }
            foreach (ILocation l in locations)
            {
                if (l.id.Equals(id.ToString()))
                {
                    l.distributor.strategy = (IDistributionStrategy) new StandardDistribution();
                    List<IBooking> bookings;
                    if(!cache.TryGetValue(CacheKeys.BOOKING ,out bookings))
                    {
                        bookings = new List<IBooking>();
                    }
                    List<Booking> book = new List<Booking>();
                    foreach (Booking b in bookings)
                    {
                        book.Add(b);
                    }
                    l.distributor.run(DateTime.Now, book);
                    break;
                }
            }
            return RedirectToAction("Index");
        }
    }
}