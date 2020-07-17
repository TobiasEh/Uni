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

        public IActionResult EditZone(Zone zone)
        {
            locations = (List<ILocation>)cache.Get(CacheKeys.LOCATION);
            locations.ForEach(e => { if (e.zones.Contains(zone)) { e.zones.Remove(zone); }; });
            cache.Set(CacheKeys.LOCATION, locations);

            return View("CreateZone", zone);
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

        [HttpGet]
        public IActionResult CreateZone(Zone zone)
        {
            if(zone != null)
                return View(zone);
            return View(new Zone());
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
