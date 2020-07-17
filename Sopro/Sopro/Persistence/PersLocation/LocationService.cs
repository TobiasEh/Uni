using Sopro.Interfaces.PersistenceController;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.IO;
using System.Text;
using Sopro.Interfaces;

namespace Sopro.Persistence.PersLocation
{
    public class LocationService : LocationRepository, ILocationService
    {
        private JsonSerializerOptions options = new JsonSerializerOptions() { WriteIndented = true };
        private JsonStringEnumConverter stringEnumConverter = new JsonStringEnumConverter();

        public List<ILocation> import(string path)
        {
            options.Converters.Add(new JsonStringEnumConverter(JsonNamingPolicy.CamelCase));
            byte[] bytes = File.ReadAllBytes(path);
            var data = Encoding.UTF8.GetString(bytes);
            List<ILocation> importedBookings = JsonSerializer.Deserialize<List<ILocation>>(data, options);

            return importedBookings;
        }

        public void export(List<ILocation> list, string path)
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