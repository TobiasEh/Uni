using Sopro.Interfaces.PersistenceController;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.IO;
using System.Text;
using Sopro.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Sopro.Persistence.PersScenario
{
    public class ScenarioService : IScenarioService
    {
        private JsonSerializerOptions options = new JsonSerializerOptions() { WriteIndented = true };
        private JsonStringEnumConverter stringEnumConverter = new JsonStringEnumConverter();

        public List<ScenarioExportImportViewModel> import(IFormFile file)
        {
            var result = new StringBuilder();
            using (var reader = new StreamReader(file.OpenReadStream()))
            {
                while (reader.Peek() >= 0)
                    result.AppendLine(reader.ReadLine());
            }
            options.Converters.Add(new JsonStringEnumConverter(JsonNamingPolicy.CamelCase));
            string test = result.ToString();
            List<ScenarioExportImportViewModel> importedScenarios = JsonSerializer.Deserialize<List<ScenarioExportImportViewModel>>(result.ToString(), options);

            return importedScenarios;
        }

        public FileContentResult export(List<ScenarioExportImportViewModel> list)
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
            output.FileDownloadName = "Scenario.json";
            return output;

        }
    }
}
