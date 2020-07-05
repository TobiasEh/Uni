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
using System.IO;
using System.Text;

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
        [HttpGet]
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
       
        [HttpPost]
        public IActionResult upload(JSONFileSpezifier spezifier)
        {
            cacheKey = "bookings";
            if(!_memoryCache.TryGetValue(cacheKey, out bookings))
            {
                bookings = new List<Booking>();
            }
            if (!ModelState.IsValid)
            {
                ViewBag.ErrorMessage = "Achtung Sie haben eine falsche Datei ausgewählt.";
                return RedirectToAction("Index", "Booking", bookings);
            }

            var _file = spezifier.file;
            if (_file == null || _file.Length == 0)
            {
                ViewBag.ErrorMessage = "File not found";
                return RedirectToAction("Index", "Booking", bookings);
            }

            var res = new StringBuilder();
            using (var rea = new StreamReader(_file.OpenReadStream()))
            {
                while (rea.Peek() >= 0)
                    res.AppendLine(rea.ReadLine());
            }
            
            List<Booking>output = JsonSerializer.Deserialize<List<Booking>>(res.ToString(), options);
            foreach (Booking b in output)
            {
                bookings.Add(b);
            }
            _memoryCache.Set(cacheKey, bookings);
            ViewBag.Message = "File wurde erfolgreich Hochgeladen";

            return RedirectToAction("Index", "Booking");
        }
    }
}