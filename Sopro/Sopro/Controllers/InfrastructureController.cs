using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Sopro.Interfaces;
using Sopro.Models.Infrastructure;
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
        private ILocationService service;

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult import([FromForm]FileViewModel model)
        {
            IFormFile file = model.importedFile;
            string path = Path.GetFullPath(file.Name);
            List<ILocation> importedLocations = service.import(path);

            if(!cache.TryGetValue(CacheKeys.LOCATION, out locations))
            {
                locations = importedLocations;
            }
            
            foreach(ILocation loc in importedLocations)
            {
                if (locations.All(e => e.guid != loc.guid))
                {
                    locations.Add(loc);
                }
                    
            }
            cache.Set(locations, CacheKeys.LOCATION);
            return View("Index", locations);
        }

        [HttpGet]
        public IActionResult export([FromForm]FileViewModel model)
        {
            cache.TryGetValue(CacheKeys.LOCATION, out locations);
            IFormFile file = model.exportedFile;
            string path = Path.GetFullPath(file.Name);
            service.exportFile(locations, path);

            return View("Index", locations);
        }
    }
}
