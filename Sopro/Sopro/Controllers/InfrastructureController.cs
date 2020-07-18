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

        public IActionResult EditZone(int? id, char site)
        {
            if (!cache.TryGetValue(CacheKeys.LOCATION, out locations))
            {
                locations = new List<ILocation>();
            }
            EditZoneViewModel viewmodel;
            foreach(ILocation l in locations)
            {
                if (l.id.Equals(id.ToString()))
                {
                    viewmodel = new EditZoneViewModel();
                    viewmodel.name = l.name;
                    viewmodel.id = (int) id;
                    foreach(Zone z in l.zones)
                    {
                        if (z.site == site)
                        {
                            viewmodel.zone = z;
                            return View(viewmodel);
                        }
                    }
                    break;
                }
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Post(ILocation location)
        {
            if(!cache.TryGetValue(CacheKeys.LOCATION, out locations))
            {
                locations = new List<ILocation>();
            }
            if (!ModelState.IsValid)
            {
                throw new Exception("Standort ist nicht valide");
            }
            locations.Add(location);
            cache.Set(CacheKeys.LOCATION, locations);
            return View("Index", locations);
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
                    l.addZone(zone);
                    break;
                }
            }
            //cache.Set(CacheKeys.LOCATION, locations);
            return RedirectToAction("EditZone",( id, site));
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
