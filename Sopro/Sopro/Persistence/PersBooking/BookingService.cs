﻿using Sopro.Interfaces.AdministrationController;
using Sopro.Interfaces.PersistenceController;
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

        public List<IBooking> import(string path)
        {
            options.Converters.Add(new JsonStringEnumConverter(JsonNamingPolicy.CamelCase));
            byte[] bytes = File.ReadAllBytes(path);
            var data = Encoding.UTF8.GetString(bytes);
            List<IBooking> importedBookings = JsonSerializer.Deserialize<List<IBooking>>(data, options);

            return importedBookings;
        }

        public void export(List<IBooking> list, string path)
        {
            options.Converters.Add(stringEnumConverter);

            var data = JsonSerializer.Serialize(list, options);
            byte[] bytes = Encoding.UTF8.GetBytes(data);

            string extension = Path.GetExtension(path);
            if(extension == null || extension == string.Empty)
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
