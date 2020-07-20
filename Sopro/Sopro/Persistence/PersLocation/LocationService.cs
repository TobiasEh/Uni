using Sopro.Interfaces.PersistenceController;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.IO;
using System.Text;
using Sopro.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Sopro.Persistence.PersLocation
{
    public class LocationService : ILocationService
    {
        private JsonSerializerOptions options = new JsonSerializerOptions() { WriteIndented = true };
        private JsonStringEnumConverter stringEnumConverter = new JsonStringEnumConverter();

        public List<LocationExportImportViewModel> import(IFormFile file)
        {
            var result = new StringBuilder();
            using (var reader = new StreamReader(file.OpenReadStream()))
            {
                while (reader.Peek() >= 0)
                    result.AppendLine(reader.ReadLine());
            }
            options.Converters.Add(new JsonStringEnumConverter(JsonNamingPolicy.CamelCase));
            string test = result.ToString();
            List<LocationExportImportViewModel> importedLocations = JsonSerializer.Deserialize<List<LocationExportImportViewModel>>(result.ToString(), options);

            return importedLocations;
        }

        public FileContentResult export(List<LocationExportImportViewModel> list)
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
            output.FileDownloadName = "Infrastructure.json";
            return output;

        }
    }
}