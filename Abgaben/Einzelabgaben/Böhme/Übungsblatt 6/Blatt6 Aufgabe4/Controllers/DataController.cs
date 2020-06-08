using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Blatt3_Aufgabe4.Models;
using Blatt3_Aufgabe4.ViewModel;
using System.Text.Json;
using Blatt3_Aufgabe4.Controllers;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Http;

namespace Blatt3_Aufgabe4.Controllers
{
    public class DataController : Controller
    {
        private JsonSerializerOptions options = new JsonSerializerOptions();
        private List<Booking> bookings;
        private readonly IMemoryCache _memoryCache;
        private string cacheKey;
        string json;
        

        public DataController(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
            bookings = new List<Booking>();
            options.Converters.Add(new JsonStringEnumConverter(JsonNamingPolicy.CamelCase));
            options.WriteIndented = true;
            
        }
        public IActionResult exportData(string cacheKey)
        {
            
            string filename;
            this.cacheKey = cacheKey;
            object objec;
            
            if(cacheKey == "evaluation")
            {
                List<ConnectorTypeEvaluationViewModel> evaluList = new List<ConnectorTypeEvaluationViewModel>();
                filename = "evaluation";
                objec = bookings;
            } 
            else
            {
                filename = "bookings";
                objec = bookings;
            }

            _memoryCache.TryGetValue(cacheKey, out objec);
            json = JsonSerializer.Serialize(objec, options);
            string filename2 = string.Concat(filename, DateTime.Now.ToString("ddMMyyyyhhmm"), ".json");
            System.IO.File.WriteAllText($"{filename2}", json);
            if(cacheKey == "evaluation")
            {
                return RedirectToAction("Auswertung", "Booking", objec);
            }
            else
            {
                return RedirectToAction("Index", "Booking", objec);
            }
            
        }
        
        public IActionResult importData(string filename2)
        {
            cacheKey = "bookings";
            json = System.IO.File.ReadAllText($"{filename2}");
            bookings = JsonSerializer.Deserialize<List<Booking>>(json, options);
            List<Booking> output = new List<Booking>();
            _memoryCache.TryGetValue(cacheKey, out output);
            foreach (Booking b in output)
            {
                bookings.Add(b);
            }
            _memoryCache.Set(cacheKey, bookings);

            return RedirectToAction("Index", "Booking");
        }
        [HttpPost]
        public async Task<IActionResult> upload(JSONFileSpezifier spezifier)
        {
            Console.WriteLine("upload_beginn");
            if (ModelState.IsValid)
            {
                var _file = spezifier.file;
                if(_file == null)
                {
                    ModelState.AddModelError("", "File is empty");
                    return Content("File is not selected");
                }

                else
                {
                    var filename2 = _file.FileName;
                    var filepath = $"{filename2}";
                
                using (var stream = System.IO.File.Create(filepath))
                    {
                        await _file.CopyToAsync(stream);
                    }
                    return importData(filename2);
                }
            }
            return Content("File is invalid");
        }
    }
}