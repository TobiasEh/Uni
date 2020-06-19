using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using DocumentFormat.OpenXml.Drawing;
using DocumentFormat.OpenXml.EMMA;
using DocumentFormat.OpenXml.Office2013.PowerPoint.Roaming;
using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.AspNetCore.DataProtection.KeyManagement.Internal;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing.Constraints;
using Microsoft.Extensions.Caching.Memory;
using sopro_sose_2020.Models;
using sopro_sose_2020.ViewModel.Booking;

namespace sopro_sose_2020.Controllers
{
    public class DataController : Controller
    {
        List<sopro_sose_2020.Models.Booking> bookingList;
        private readonly IMemoryCache _memoryCache;
        private string cacheKey;
        private JsonSerializerOptions options;
        public DataController(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
            options = new JsonSerializerOptions();
            options.Converters.Add(new JsonStringEnumConverter(JsonNamingPolicy.CamelCase));
            options.WriteIndented = true;
            bookingList = new List<Booking>();

        }
        public IActionResult exportData(string cKey)
        {
            string _filename;
            object obj;
            string json;
            if (cKey == "eva")
            {
                var controller = new EvaluationController();
                List<ConnectorTypeEvaluationViewModel> EvaList = controller.Evaluation(getBookingList());
                _filename = "evaluation";
                obj = EvaList;
            }
            else
            {
                _filename = "bookings";
                obj = getBookingList();

            }
            json = JsonSerializer.Serialize(obj, options);
            string filename = string.Concat(DateTime.Now.ToString("yyyyddMM"),_filename,"-exported", ".json");
            //System.IO.File.WriteAllText($"wwwroot/APP_DATA/{filename}", json);
            return download(filename,json);
        }

        private List<Booking> getBookingList()
        {
            var cacheKey = "bookingList";
            _memoryCache.TryGetValue(cacheKey, out bookingList);
            return bookingList;
        }

        public IActionResult importData(string filename)
        {
            cacheKey = "bookingList";
            var path = $"wwwroot/APP_DATA/{filename}";
            string json = System.IO.File.ReadAllText(path);
            bookingList = JsonSerializer.Deserialize<List<Booking>>(json, options);
            _memoryCache.Set(cacheKey, bookingList);
            System.IO.File.Delete(path);
            return RedirectToAction("Index", "Booking");


        }
        public IActionResult download(string _fileName,string _data)
        {
            byte[] fileBytes = Encoding.UTF8.GetBytes(_data);
            return File(fileBytes, "application/octet-stream", _fileName);
        }
      
        [HttpPost]
        public async Task<IActionResult> upload(jsonFileModel uploaded)
        {
            

            if (ModelState.IsValid)
            {
                var _file = uploaded.file;
                if (_file == null)
                {
                   ModelState.AddModelError("","File is empty");
                   return Content("File not selected");
                }
                else
                {
                    var fileName = _file.FileName;
                    var filePath = $"wwwroot/APP_DATA/{fileName}";

                    using (var stream = System.IO.File.Create(filePath))
                    {
                          await _file.CopyToAsync(stream);
                    }
                        return importData(fileName);
                }
            }
            return Content("File invalid");
        }
    }
}