using Sopro.Interfaces.PersistenceController;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Sopro.Persistence.PersVehicle
{
    public class VehicleService : VehicleRepository, IVehicleService
    {
        private JsonSerializerOptions options = new JsonSerializerOptions() { WriteIndented = true };
        private JsonStringEnumConverter stringEnumConverter = new JsonStringEnumConverter();

        public List<IVehicle> import(string path)
        {
            options.Converters.Add(new JsonStringEnumConverter(JsonNamingPolicy.CamelCase));
            byte[] bytes = File.ReadAllBytes(path);
            var data = Encoding.UTF8.GetString(bytes);
            List<IVehicle> importedBookings = JsonSerializer.Deserialize<List<IVehicle>>(data, options);

            return importedBookings;
        }

        public void export(List<IVehicle> list, string path)
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
