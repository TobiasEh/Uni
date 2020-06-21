using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Microsoft.AspNetCore.DataProtection.KeyManagement.Internal;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing.Constraints;
using Microsoft.Extensions.Caching.Memory;
using WebApplication1.Models;
using WebApplication1.ViewModel;


namespace WebApplication1.Controllers
{
    public class DataController : Controller
    {
        private readonly IMemoryCache memoryCache;
        List<Booking> bookings = new List<Booking>();
        JsonSerializerOptions opt = new JsonSerializerOptions() { WriteIndented = true };

        public DataController(IMemoryCache memoryCache)
        {
            this.memoryCache = memoryCache;
            var Enumtranslation = new JsonStringEnumConverter(JsonNamingPolicy.CamelCase);
            opt.Converters.Add(Enumtranslation);
            
        }


        public IActionResult export()
        {
            var cacheKey = "bookings";
            memoryCache.TryGetValue(cacheKey, out bookings);
            var data = JsonSerializer.Serialize(bookings, opt);
            var contentType = "APPLICATION/octet-stream";
            System.Text.ASCIIEncoding enc = new System.Text.ASCIIEncoding();
            byte[] databyte = enc.GetBytes(data);
            var response = File(databyte, contentType, "bookings.json");
            return response;
        }

        [HttpPost]
        public IActionResult import(UploadModel jsondata)
        {
            Console.WriteLine("Hello World!");

            if (!ModelState.IsValid)
            {
                ViewBag.ErrorMessage = "wrong file";
                return RedirectToAction("Index", "Booking", bookings);
            }
            var file = jsondata.file;
            string inputContent;
            StreamReader inputStreamReader = new StreamReader(file.OpenReadStream());
            {
                inputContent = inputStreamReader.ReadToEnd();
                List<Booking> templist = JsonSerializer.Deserialize<List<Booking>>(inputContent, opt);
                memoryCache.Set("bookings", templist);
            }

            return RedirectToAction("Index", "Booking");
        }
    }
}
