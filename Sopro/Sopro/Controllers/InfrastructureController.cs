using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Net.Http.Headers;
using Sopro.Interfaces;
using Sopro.Interfaces.PersistenceController;
using Sopro.Models.Infrastructure;
using Sopro.Persistence.PersLocation;
using Sopro.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Sopro.Controllers
{
    public class InfrastructureController : Controller
    {
        private IMemoryCache cache;
        private List<ILocation> locations;
        private ILocationService service = new LocationService();

        public InfrastructureController(IMemoryCache _cache)
        {
            cache = _cache;
        }

        public IActionResult Index()
        {
            if (!cache.TryGetValue(CacheKeys.LOCATION, out locations))
            {
                locations = new List<ILocation>();
            }

            return View(new InfrastructureViewModel() { locations = locations });
        }

        public IActionResult EndEdit(EditZoneViewModel viewmodel)
        {

            return RedirectToAction("Index");
        }

        public IActionResult EditStation(EditZoneViewModel viewmodel) 
        {
            Station station = viewmodel.station;
            station.plugs = new List<Plug>();
            for(int i = 0; i < viewmodel.ccs;i++)
            {
                station.addPlug(new Plug() { power=viewmodel.ccsPower, type=PlugType.CCS});
            }
            for (int i = 0; i < viewmodel.type2;i++)
            {
                station.addPlug(new Plug() { power = viewmodel.type2Power, type = PlugType.TYPE2 });
            }
            if (!cache.TryGetValue(CacheKeys.LOCATION, out locations))
            {
                locations = new List<ILocation>();
            }
            foreach(ILocation location in locations)
            {
                if (viewmodel.id.ToString().Equals(location.id))
                {
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
                    }
                }
            }
            station.id = id;
            if (TryValidateModel(station)) 
            {
                viewmodel.zone.addStation(station);
                viewmodel.station = new Station();
                viewmodel.ccs = 0;
                viewmodel.type2 = 0;
                viewmodel.ccsPower = 0;
                viewmodel.type2Power = 0;
                return View("EditZone", viewmodel);
            }
            return View("EditZone", viewmodel);
        }

        [ValidateAntiForgeryToken]
        public IActionResult EditZone(int? id, char site)
        {
            if (!cache.TryGetValue(CacheKeys.LOCATION, out locations))
            {
                locations = new List<ILocation>();
            }
            EditZoneViewModel viewmodel;
            foreach(ILocation l in locations)
            {
                bool test = (l.id.Equals(id.ToString()));
                if (l.id.Equals(id.ToString()))
                {
                    viewmodel = new EditZoneViewModel();
                    viewmodel.name = l.name;
                    viewmodel.id = (int) id;
                    viewmodel.station = new Station();
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
            return RedirectToAction("Index");
        }

        public IActionResult StartEditStation(int? id, char site, int idStation)
        {
            if (!cache.TryGetValue(CacheKeys.LOCATION, out locations))
            {
                locations = new List<ILocation>();
            }

            EditZoneViewModel viewmodel = new EditZoneViewModel();
            foreach (ILocation l in locations)
            {
                if (l.id.Equals(id.ToString()))
                {
                    viewmodel.name = l.name;
                    viewmodel.id = (int)id;
                    foreach (Zone z in l.zones)
                    {
                        if (z.site == site)
                        {
                            viewmodel.zone = z;
                            foreach (Station s in z.stations)
                            { 
                                if (s.id == idStation)
                                {
                                    viewmodel.station = s;
                                    return View("EditZone",viewmodel);
                                }
                            }
                        }
                    }
                }
            }
return RedirectToAction("EditZone",new { id = id, site = site });
        }

        public IActionResult DeleteStation(int? _id, char _site, int idStation) 
        {
            if (!cache.TryGetValue(CacheKeys.LOCATION, out locations))
            {
                locations = new List<ILocation>();
            }

            foreach (ILocation l in locations)
            {
                if (l.id.Equals(_id.ToString()))
                {
                    foreach (Zone z in l.zones)
                    {
                        if (z.site == _site)
                        {
                            foreach(Station s in z.stations)
                            {
                                if(s.id == idStation)
                                {
                                    z.deleteStation(s);
                                    return RedirectToAction("EditZone", new{ id = _id, site = _site });
                                }
                            }
                        }
                    }
                }
            }

            return RedirectToAction("EditZone", new{ id = _id, site = _site });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult NewLocation()
        {
            ILocation l = new Location();
            l.name = "Neu";
            l.emergency = 0;
            l.zones = new List<Zone>();
            if (!cache.TryGetValue(CacheKeys.LOCATION, out locations))
            {
                locations = new List<ILocation>();
            }
            int id = -1;
            bool uniqeId = false;
            while (!uniqeId)
            {
                uniqeId = true;
                id++;
                foreach(ILocation location in locations)
                {
                    if(location.id == id.ToString())
                    {
                        uniqeId = false;
                        break;
                    }
                }

            }
            l.id = id.ToString();
            
            locations.Add(l);
            cache.Set(CacheKeys.LOCATION, locations);
            return RedirectToAction("Index");
        }
        public IActionResult DeleteLocation(int ? id)
        {
            if (!cache.TryGetValue(CacheKeys.LOCATION, out locations))
            {
                locations = new List<ILocation>();
            }
            foreach(ILocation l in locations)
            {
                if(l.id.Equals(id.ToString()))
                {
                    locations.Remove(l);
                    cache.Set(CacheKeys.LOCATION, locations);
                    break;
                }
            }
            return RedirectToAction("Index");
        }


        public IActionResult EditLocation(InfrastructureViewModel viewmodel) 
        {
            
            if (!cache.TryGetValue(CacheKeys.LOCATION, out locations))
            {
                locations = new List<ILocation>();
            }
            foreach(ILocation l in locations)
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
            cache.Set(CacheKeys.LOCATION, locations);
            return RedirectToAction("Index");
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult import([FromForm]FileViewModel model)
        {
            IFormFile file = model.importedFile;
            string path = Path.GetFullPath(file.Name);
            List<ILocation> importedLocations = service.import();

            if(!cache.TryGetValue(CacheKeys.LOCATION, out locations))
            {
                locations = importedLocations;
            }
            
            foreach(ILocation loc in importedLocations)
            {
                if (!locations.Contains(loc))
                {
                    locations.Add(loc);
                }
                    
            }
            cache.Set(CacheKeys.LOCATION, locations);
            return View("Index", locations);
        }

        public IActionResult CreateZone(int? id)
        {

            Zone zone = new Zone();
            char site = 'A';
            bool siteValid = false;
            if (!cache.TryGetValue(CacheKeys.LOCATION, out locations))
            {
                locations = new List<ILocation>();
            }
            foreach (ILocation l in locations)
            {
                if (l.id.Equals(id.ToString()))
                {
                    while (!siteValid)
                    {
                        siteValid = true;
                        foreach (Zone z in l.zones)
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
                    zone.stations= new List<Station>();
                    Station station = new Station();
                    station.id = 0;
                    station.maxPower = 0;
                    station.maxParallelUseable = 0;
                    station.plugs = new List<Plug>();
                    zone.addStation(station);
                    Plug plug = new Plug();
                    plug.power = 0;
                    plug.type = PlugType.CCS;
                    station.addPlug(plug);
                    l.addZone(zone);
                    break;
                }
            }
            //cache.Set(CacheKeys.LOCATION, locations);
            return RedirectToAction("EditZone",new { id = id, site = site });
        }

        [HttpGet]
        public IActionResult export([FromForm]FileViewModel model)
        {
            cache.TryGetValue(CacheKeys.LOCATION, out locations);
            IFormFile file = model.exportedFile;
            string path = Path.GetFullPath(file.Name);
            service.export(locations);

            return View("Index", locations);
        }
    }
}
