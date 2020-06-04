using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using TestProjekt.Models;
using TestProjekt.ViewModel;

namespace TestProjekt.Controllers
{
    public class DataModelController : Controller
    {
        private JsonSerializerOptions options = new JsonSerializerOptions();
        private List<Booking> bookings;
        private readonly IMemoryCache _memoryCache;
        private string cacheKey;
        string json;

        public DataModelController(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
            options.Converters.Add(new JsonStringEnumConverter(JsonNamingPolicy.CamelCase));
            options.WriteIndented = true;
        }

        public IActionResult expoBookingData(string cacheKey)
        {
            string name = "bookings_exp";
            List<Booking> output = new List<Booking>();

            _memoryCache.TryGetValue(cacheKey, out output);

            json = JsonSerializer.Serialize(output, options);

            name = string.Concat(name, DateTime.Now.ToString(("yyyy-MM-dd")), ".json");
            System.IO.File.WriteAllText($"{name}", json);

            return RedirectToAction("Index", "Booking", output);
        }

        public IActionResult expoEvaluationData(string cacheKey)
        {
            string name = "evaluation_exp";
            List<ConnectorTypeEvaluationViewModel> evaluation = new List<ConnectorTypeEvaluationViewModel>();
            List<Booking> output = new List<Booking>();

            _memoryCache.TryGetValue(cacheKey, out output);

            int[] plugs = new int[6];
            foreach (Booking e in output)
            {
                plugs[(int)(e.connectorType)]++;
            }

            int totalCon = plugs.Sum();

            foreach (ConnectorType ct in Enum.GetValues(typeof(ConnectorType)))
            {
                evaluation.Add(new ConnectorTypeEvaluationViewModel()
                {
                    connectorType = ct,
                    percBooking = plugs[(int)ct] / totalCon * 100
                });
            }

            json = JsonSerializer.Serialize(evaluation, options);

            name = string.Concat(name, DateTime.Now.ToString(("yyyy-MM-dd")), ".json");
            System.IO.File.WriteAllText($"{name}", json);

            return RedirectToAction("Evaluation", "Booking", output);
        }

        [HttpPost]
        public async Task<IActionResult> upload(FileSpec spezifier)
        {
            if (ModelState.IsValid)
            {
                var _file = spezifier.file;
                if (_file == null)
                {
                    ModelState.AddModelError("", "File is empty");
                    return Content("File is not selected");
                }

                else
                {
                    var filename = _file.FileName;
                    var filepath = $"{filename}";

                    using (var stream = System.IO.File.Create(filepath))
                    {
                        await _file.CopyToAsync(stream);
                    }
                    return impoBookingData(filename);
                }
            }
            return Content("File is invalid");
        }

        public IActionResult impoBookingData(string filename)
        {
            cacheKey = "bookings";
            json = System.IO.File.ReadAllText($"{filename}");
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
    }
}