using System;
using System.Collections.Generic;
using System.Linq;
using Blatt03.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Blatt03.ViewModel;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Text.Json.Serialization;

namespace Blatt03.Controllers
{
    public class BookingController : Controller
    {
        private readonly IMemoryCache _memoryCache;
        List<Booking> bookings;

        public BookingController(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        public IActionResult Index()
        {
            var cacheKey = "bookings";
            _memoryCache.TryGetValue(cacheKey, out bookings);
            return View(bookings);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View(new Booking());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Post(Booking booking)
        {
            var cacheKey = "bookings";

            if (!_memoryCache.TryGetValue(cacheKey, out bookings))
            {
                bookings = new List<Booking>();
            }

            bookings.Add(booking);
            _memoryCache.Set(cacheKey, bookings);
            return View("Index", bookings);
        }

        [HttpGet]
        public IActionResult Download()
        {
            // Read cache
            var cacheKey = "bookings";
            _memoryCache.TryGetValue(cacheKey, out bookings);

            // Write enum content as string
            var stringEnumConverter = new JsonStringEnumConverter();
            JsonSerializerOptions opts = new JsonSerializerOptions() { WriteIndented = true};
            opts.Converters.Add(stringEnumConverter);

            // Serialize
            var data = JsonSerializer.Serialize(bookings, opts);
            byte[] bytes = Encoding.UTF8.GetBytes(data);

            // Export
            var output = new FileContentResult(bytes, "application/octet-stream");
            output.FileDownloadName = "bookings.json";
            return output;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upload(IFormFile importedFile)
        {
            var file = importedFile;
            var cacheKey = "bookings";

            _memoryCache.TryGetValue(cacheKey, out bookings);
            if (!_memoryCache.TryGetValue(cacheKey, out bookings))
            {
                bookings = new List<Booking>();
            }

            if (file == null || file.Length == 0)
            {
                return View("Index", bookings);
            }

            var result = new StringBuilder();
            using (var reader = new StreamReader(file.OpenReadStream()))
            {
                while (reader.Peek() >= 0)
                    result.AppendLine(reader.ReadLine());
            }

            var options = new JsonSerializerOptions();
            options.Converters.Add(new JsonStringEnumConverter(JsonNamingPolicy.CamelCase));
            List<Booking> importedBookings = JsonSerializer.Deserialize<List<Booking>>(result.ToString(), options);
            foreach (Booking b in importedBookings) {
                bookings.Add(b);
            }

            _memoryCache.Set(cacheKey, bookings);

            return View("Index", bookings);
        }

        public IActionResult Evaluation()
        {
            var cacheKey = "bookings";
            if (!_memoryCache.TryGetValue(cacheKey, out bookings))
            {
                return View();
            }

            List<ConnectorTypeEvaluationViewModel> evaluationData = new List<ConnectorTypeEvaluationViewModel>() { };
            
            int[] plugAppearances = new int[Enum.GetNames(typeof(ConnectorType)).Length];
            foreach (Booking b in bookings)
            {
                plugAppearances[(int)(b.connectorType)]++;
            }
                
            int totalConnectors = plugAppearances.Sum();

            foreach (ConnectorType c in Enum.GetValues(typeof(ConnectorType)))
            {
                evaluationData.Add(new ConnectorTypeEvaluationViewModel()
                {
                    connectorType = c,
                    percAppearsInBookings = Math.Round((double)plugAppearances[(int)c] / (double)totalConnectors * 100, 2)
                });
            }

            return View(evaluationData);
        }
    }
}
