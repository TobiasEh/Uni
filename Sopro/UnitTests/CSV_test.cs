using NUnit.Framework;
using Sopro.Models.User;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace UnitTests
{
    [TestFixture]
    public class CSV_test
    {
        
        

        [Test]
        public void loadTest()
        {
            Console.WriteLine("starting Test");

            string fpath = $"wwwroot/APP_DATA/test.csv";
            Console.WriteLine("set fpath : " + fpath);
            IdentityProvider provider = new IdentityProvider() { path = fpath };
            Console.WriteLine(" new Identity Provider with fpath ");
            List<User> userList = new List<User>();
            Console.WriteLine(" new User List ");


            userList = provider.loadCSV(fpath);
            if (userList.Count > 0)
            {
                Console.WriteLine(" email | role \n");
                foreach (User user in userList)
                {
                    Console.WriteLine(user.email + " | " + user.usertype + user.usertype.GetType().IsEnum);
                    Assert.IsTrue(user.usertype.GetType().IsEnum);
                }
            }
            else
            {
                Console.WriteLine("UserList is empty");
            }
            
        }
    }
}
