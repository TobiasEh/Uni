using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Sopro.Interfaces.AdministrationController;
using Sopro.Interfaces.PersistenceController;
using Sopro.Models.Administration;
using Sopro.ViewModels;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;


namespace Sopro.Persistence.PersBooking
{
    public class BookingService : IBookingService
    {
        private JsonSerializerOptions options = new JsonSerializerOptions() { WriteIndented = true };
        private JsonStringEnumConverter stringEnumConverter = new JsonStringEnumConverter();

        public List<BookingExportImportViewModel> Import(IFormFile file)
        {
            //options.Converters.Add(new JsonStringEnumConverter(JsonNamingPolicy.CamelCase));
            //byte[] bytes = File.ReadAllBytes(file);
            //var data = Encoding.UTF8.GetString(bytes);
            //var utf8Reader = new Utf8JsonReader(file);
            //List<BookingExportImportViewModel>  importedBookings = JsonSerializer.Deserialize<List<BookingExportImportViewModel>>(ref utf8Reader);
            var result = new StringBuilder();
            using (var reader = new StreamReader(file.OpenReadStream()))
            {
                while (reader.Peek() >= 0)
                    result.AppendLine(reader.ReadLine());
            }
            options.Converters.Add(new JsonStringEnumConverter(JsonNamingPolicy.CamelCase));
            string test = result.ToString();
            List<BookingExportImportViewModel> importedBookings = JsonSerializer.Deserialize<List<BookingExportImportViewModel>>(result.ToString(), options);

            return importedBookings;
        }

        public FileContentResult export(List<BookingExportImportViewModel> list)
        {

            // Write enum content as string
            var stringEnumConverter = new JsonStringEnumConverter();
            JsonSerializerOptions opts = new JsonSerializerOptions() { WriteIndented = true };
            opts.Converters.Add(stringEnumConverter);

            // Serialize
            var data = JsonSerializer.Serialize(list, opts);
            byte[] bytes = Encoding.UTF8.GetBytes(data);

            // Export
            var output = new FileContentResult(bytes, "application/octet-stream");
            output.FileDownloadName = "bookings.json";
            return output;

        }

    }
}
