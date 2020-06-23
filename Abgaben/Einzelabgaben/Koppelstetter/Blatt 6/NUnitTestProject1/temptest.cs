
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Edge;
using System.Threading;
using OpenQA.Selenium.Support.UI;

namespace NUnitTestProject1
{
    class temptest
    {
        [TestFixture]
        public class TestSelenium
        {
            static IWebDriver driverGC;
            static IWebDriver driverME;
            private void Waittime()
            {
                Thread.Sleep(300);
            }

            [OneTimeSetUp]
            public void OneTimeSetUp()
            {
                //driverME = new EdgeDriver(new EdgeOptions().useChromium = true);
                driverGC = new ChromeDriver();
                

            }

            [Test]
            public void TestChromeDriver()
            {
                driverGC.Navigate().GoToUrl("https://localhost:44391/Booking/Create");
            }
            [Test]
            public void TestBookingurl()
            {
                IWebElement temp = driverGC.FindElement(By.XPath("//*[@id='buchen']"));
                temp.Click();
                Waittime();
                Assert.AreEqual("https://localhost:44391/Booking/Post", driverGC.Url);
            }

            [Test]
            public void TestEdgeDriver()
            {
               driverME.Navigate().GoToUrl("http://www.google.com/");
            }

            [OneTimeTearDown]
            public void OneTimeTearDown()
            {
                //driverME.Quit();
                //DriverME.Dispose();
                driverGC.Quit();
                driverGC.Dispose();
            }

        }
    }
}
