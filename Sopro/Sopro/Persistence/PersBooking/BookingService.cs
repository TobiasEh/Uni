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
    public class BookingService : BookingRepository, IBookingService
    {
        private JsonSerializerOptions options = new JsonSerializerOptions() { WriteIndented = true };
        private JsonStringEnumConverter stringEnumConverter = new JsonStringEnumConverter();

        public List<BookingExportInportViewModel> Import(IFormFile file)
        {
            var result = new StringBuilder();
            using (var reader = new StreamReader(file.OpenReadStream()))
            {
                while (reader.Peek() >= 0)
                    result.AppendLine(reader.ReadLine());
            }
            options.Converters.Add(new JsonStringEnumConverter(JsonNamingPolicy.CamelCase));
            List<BookingExportInportViewModel> importedBookings = JsonSerializer.Deserialize<List<BookingExportInportViewModel>>(result.ToString(), options);

            return importedBookings;
        }

        public FileContentResult export(List<BookingExportInportViewModel> list)
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
