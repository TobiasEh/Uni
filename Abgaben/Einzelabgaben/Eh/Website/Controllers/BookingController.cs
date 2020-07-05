using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Website.Models;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.AspNetCore.DataProtection.KeyManagement.Internal;
using System.Collections;
using Website.ViewModel;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Text.Json.Serialization;
using System.Diagnostics;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Website.Controllers
{
    public class BookingController : Controller
    {
        // GET: /<controller>/
       

        private readonly IMemoryCache _memoryCache;
        List<Booking> bookings = new List<Booking>();
 public BookingController(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }
        public IActionResult Index()
        {
            _memoryCache.TryGetValue("bookings", out bookings);
            if(bookings == null)
            {
                bookings = new List<Booking>();
            }
            /*UploadViewModel test = new UploadViewModel();
            test.bookings = bookings;
            
            Booking b1 = new Booking()
            {
                currentCharge = 10,
                requiredDistance = 30,
                start = new DateTime(2020, 5, 7, 12, 0, 0),
                end = new DateTime(2020, 5, 7, 17, 0, 0)
            };

            Booking b2 = new Booking()
            {
                currentCharge = 10,
                requiredDistance = 30,
                start = new DateTime(2020, 5, 10, 12, 30, 0),
                end = new DateTime(2020, 5, 10, 18, 0, 0)
            };

            Booking b3 = new Booking()
            {
                currentCharge = 10,
                requiredDistance = 30,
                start = new DateTime(2020, 5, 7, 22, 0, 0),
                end = new DateTime(2020, 5, 8, 8, 15, 0)
            };
            Booking b4 = new Booking()
            {
                currentCharge = 10,
                requiredDistance = 30,
                start = new DateTime(2020, 5, 8, 22, 0, 0),
                end = new DateTime(2020, 5, 9, 8, 15, 0)
            };

            bookings = new List<Booking>()
            {
                b1,
                b2,
                b3,
                b4
            };
            */
            return View(bookings);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View(new Booking());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult newBooking(Booking booking)
        {
            if (TryValidateModel(booking))
                { 
                string cacheKey = "bookings";
                if (!_memoryCache.TryGetValue(cacheKey, out bookings))
                {
                    bookings = new List<Booking>();
                }
                bookings.Add(booking);
                _memoryCache.Set(cacheKey, bookings);
                return View("Index", bookings);
            }
            else
            {
                return RedirectToAction(nameof(Create));
            }
        }

        public IActionResult Auswertung()
        {
            var cacheKey = "bookings";
            if (!_memoryCache.TryGetValue(cacheKey, out bookings))
            {
                return View();
            }

            List<ConnectorTypeEvaluationViewModel> evaluationData = new List<ConnectorTypeEvaluationViewModel>() { };

            int[] plugAppearances = new int[Enum.GetNames(typeof(Plug)).Length];
            foreach (Booking b in bookings)
            {
                plugAppearances[(int)(b.plugType)]++;
            }

            int totalConnectors = plugAppearances.Sum();

            foreach (Plug c in Enum.GetValues(typeof(Plug)))
            {
                evaluationData.Add(new ConnectorTypeEvaluationViewModel()
                {
                    plugType = c,
                    percentShowsInBookings =  Math.Round((double)plugAppearances[(int)c] / (double)totalConnectors * 100, 2)
                });
            }

            return View(evaluationData);
        }


        [HttpGet]
        public IActionResult Export()
        {
            var cacheKey = "bookings";
            _memoryCache.TryGetValue(cacheKey, out bookings);

            var stringEnumConverter = new System.Text.Json.Serialization.JsonStringEnumConverter();
            JsonSerializerOptions opts = new JsonSerializerOptions() { WriteIndented = true };
            opts.Converters.Add(stringEnumConverter);

            var data = JsonSerializer.Serialize(bookings, opts);
            byte[] bytes = Encoding.UTF8.GetBytes(data);

            var output = new FileContentResult(bytes, "application/octet-stream");
            output.FileDownloadName = "bookings.json";
            return output;
        }


        [HttpPost]
        public IActionResult Index(JsonFileModel jsonModel)
        {
           
            _memoryCache.TryGetValue("bookings", out bookings);
            if (bookings == null)
                {
                    bookings = new List<Booking>();
                }
            if (ModelState.IsValid)
            {
                var file = jsonModel.file;
                if (file == null || file.Length == 0)
                {
                    ViewBag.ErrorMessage = "File not found";
                    return View(bookings);
                }
                var import = new StringBuilder();
                using (var reader = new StreamReader(file.OpenReadStream()))
                {
                    while (reader.Peek() >= 0)
                        import.AppendLine(reader.ReadLine());
                }
                var jsonOption = new JsonSerializerOptions();
                jsonOption.Converters.Add(new JsonStringEnumConverter(JsonNamingPolicy.CamelCase));
                List<Booking> newBookings = JsonSerializer.Deserialize<List<Booking>>(import.ToString(), jsonOption);


                foreach (Booking booking in newBookings)
                {
                    bookings.Add(booking);
                }



                _memoryCache.Set("bookings", bookings);

                ViewBag.Message = "File successfully uploaded";

                return View(newBookings);
            } else
            {
                ViewBag.ErrorMessage = "ERROR: Es wurde eine falsche Datei ausgewählt.";
                return View(bookings);
            }
        }
    }
}
