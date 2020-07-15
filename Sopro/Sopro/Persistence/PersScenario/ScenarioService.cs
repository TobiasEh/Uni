using Sopro.Interfaces.PersistenceController;
using Sopro.Persistence.PersLocation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Sopro.Persistence.PersScenario
{
    public class ScenarioService : ScenarioRepository, IScenarioService
    {
        private JsonSerializerOptions options = new JsonSerializerOptions() { WriteIndented = true };
        private JsonStringEnumConverter stringEnumConverter = new JsonStringEnumConverter();

        public List<IScenario> import(string path)
        {
            options.Converters.Add(new JsonStringEnumConverter(JsonNamingPolicy.CamelCase));
            byte[] bytes = File.ReadAllBytes(path);
            var data = Encoding.UTF8.GetString(bytes);
            List<IScenario> importedBookings = JsonSerializer.Deserialize<List<IScenario>>(data, options);

            return importedBookings;
        }

        public void export(List<IScenario> list, string path)
        {
            options.Converters.Add(stringEnumConverter);

            var data = JsonSerializer.Serialize(list, options);
            byte[] bytes = Encoding.UTF8.GetBytes(data);

            string extension = Path.GetExtension(path);
            if (extension == null || extension == string.Empty)
            {
                path = string.Concat(path, ".json");
            }

            if (!extension.Equals(".json"))
            {
                Path.ChangeExtension(path, ".json");
            }

            File.WriteAllBytes(path, bytes);

        }
    }
}
}
