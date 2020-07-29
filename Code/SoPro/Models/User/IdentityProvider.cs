using CsvHelper;
using CsvHelper.Configuration;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;


namespace Sopro.Models.User
{
    /// <summary>
    /// Statische Klasse die zu einer E-Mail Adresse eine Benutzerpriorität zurückgibt.
    /// </summary>
    public static class IdentityProvider
    {
        public static string path { get; set; } = "Models/User/UserList.csv";

        public static UserType getUserPriority(string email)
        {
            List <User> userList = loadCSV(path);

            if (userList.Exists(x => x.email.Equals(email)))
            {
                return userList.Find(x => x.email.Contains(email)).usertype;
            }

            return UserType.GUEST;
        }

        /// <summary>
        /// Läd die Benutzerdaten aus der Datei, in der sie gespeichert werden.
        /// </summary>
        /// <param name="path">Der Pfad zu der Datei mit den Benutzerdaten</param>
        /// <returns>Eine Liste der Benutzer.</returns>
        public static List<User> loadCSV(string path)
        {
            List<User> userList = new List<User>();

            using (var reader = new StreamReader(path))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                csv.Configuration.Delimiter = ";";
                csv.Configuration.RegisterClassMap<UserMap>();
                userList = csv.GetRecords<User>().ToList<User>();
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
