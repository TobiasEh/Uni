using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using Sopro.Models.User;

namespace UnitTests.User
{
    [TestFixture]
    class IdentityProviderTest
    {
        private static string email = "planer@sopro.de";
        private static string path = "wwwroot/UserList.csv";

        [Test]
        public void loadCSVTest()
        {
            List<Sopro.Models.User.User> list = IdentityProvider.loadCSV(path);

            Assert.IsNotNull(list);
            Assert.IsTrue(list.Count > 0);
        }

        [Test]
        public void getUserPriorityTest()
        {
            UserType type = IdentityProvider.getUserPriority(email);
            Assert.IsTrue(type.Equals(UserType.PLANER));
        }
    }
}
