using CsvHelper;
using CsvHelper.Configuration;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;


namespace Sopro.Models.User
{
    public static class IdentityProvider
    {
        public static string path { get; set; }


        public static UserType getUserPriority(string email)
        {
            List <User> userList = loadCSV(path);
            if (userList.Exists(x => x.email.Equals(email)))
            {
                return userList.Find(x => x.email.Contains(email)).usertype;
            }
            return UserType.GUEST;
        }
        public static List<User> loadCSV(String path)
        {
            List<User> userList = new List<User>();
            Console.WriteLine("loadCSV called! \n path : " + path );
            using (var reader = new StreamReader(path))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                Console.WriteLine("hello");
                csv.Configuration.Delimiter = ";";
                csv.Configuration.RegisterClassMap<UserMap>();
                userList = csv.GetRecords<User>().ToList<User>();
                Console.WriteLine("User List Count : " + userList.Count);
            }
            return userList;
        }

    }
    
    public class UserMap : ClassMap<User>
    {
        public UserMap()
        {
            Map(m => m.email).Name("User email");
            Map(m => m.usertype).Name("Role").ConvertUsing(row => Enum.Parse<UserType>(row.GetField("Role")));
        }
    }

}
