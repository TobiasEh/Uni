using Sopro.Interfaces;
using Sopro.Models.User;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Sopro.Models.User
{
    public static class IdentityProvider
    {
        public static string path { get; set; }


        public static UserType getUserPriority(string email)
        {
            /*List <User> userList = new List<User>();
            JsonSerializerOptions options;
            options = new JsonSerializerOptions();
            options.Converters.Add(new JsonStringEnumConverter(JsonNamingPolicy.CamelCase));
            options.WriteIndented = true;
            string json = File.ReadAllText(path);
            if (json.Contains(email))
            {
                userList = JsonSerializer.Deserialize<List<User>>(json,options);
                return userList.Find(x => x.email.Contains(email)).usertype;
            }*/
            return UserType.GUEST;
        }

       
    }
}
