using Microsoft.Extensions.Caching.Memory;
using NUnit.Framework;
using WebApplication1.Controllers;
using WebApplication1.Models;

namespace NUnitTestProject1
{
    [TestFixture]
    public class Tests
    {
        private readonly IMemoryCache _memoryCache;
        [Test]
        public void Test1()
        {
            Assert.Pass();
        }

        [Test]
        public void onlycalc()
        {
            
        BookingController ctrl = new BookingController(_memoryCache);
            Assert.AreEqual(ctrl.onlycalc(1,100),1);
            Assert.AreEqual(ctrl.onlycalc(2, 50), 4);
        }

    }
}