using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Sopro.Interfaces.ControllerSimulation;
using Sopro.Interfaces.PersistenceController;
using Sopro.ViewModels;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Sopro.Persistence.PersVehicle
{
    public class VehicleService :  IVehicleService
    {
        private JsonSerializerOptions options = new JsonSerializerOptions() { WriteIndented = true };
        private JsonStringEnumConverter stringEnumConverter = new JsonStringEnumConverter();

        public List<VehicleExportImportViewModel> import(IFormFile file)
        {
            var result = new StringBuilder();
            using (var reader = new StreamReader(file.OpenReadStream()))
            {
                while (reader.Peek() >= 0)
                    result.AppendLine(reader.ReadLine());
            }
            options.Converters.Add(new JsonStringEnumConverter(JsonNamingPolicy.CamelCase));
            List<VehicleExportImportViewModel> importedVehicles = JsonSerializer.Deserialize<List<VehicleExportImportViewModel>>(result.ToString(), options);

            return importedVehicles;
        }

        public FileContentResult export(List<VehicleExportImportViewModel> list)
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
            output.FileDownloadName = "Vehicels.json";
            return output;
        }
    }

}
