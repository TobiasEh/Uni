
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System.Threading;
using OpenQA.Selenium.Support.UI;
using Microsoft.Edge.SeleniumTools;
using WebApplication1.Models;
using System.Collections.Generic;
using System.Linq;
using System;

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
                Thread.Sleep(250);
            }

            [OneTimeSetUp]
            public void OneTimeSetUp()
            {
                EdgeOptions edgeOptions = new EdgeOptions();
                edgeOptions.UseChromium= true;
                driverME = new EdgeDriver(edgeOptions);
                driverGC = new ChromeDriver();
            }

            [SetUp]
            public void Setup()
            {
                driverGC.Navigate().GoToUrl("https://localhost:44391/Booking/Create");
                //driverGC.Navigate().GoToUrl("https://www.google.de");
                driverME.Navigate().GoToUrl("https://localhost:44391/Booking/Create");
                Waittime();
            }

            [Test]
            public void TestChromeDriver()
            {
                //Assert.AreEqual("https://www.google.de/", driverGC.Url);
                Assert.AreEqual("https://localhost:44391/Booking/Create", driverGC.Url);
            }
            [Test]
            public void TestButtonBookingurl()
            {
                IWebElement temp = driverGC.FindElement(By.Name("buchen"));
                IWebElement temp2 = driverME.FindElement(By.Name("buchen"));
                temp.Click();
                temp2.Click();
                Waittime();
                Assert.AreEqual("https://localhost:44391/Booking/Create", driverGC.Url);
                Assert.AreEqual("https://localhost:44391/Booking/Create", driverME.Url);
                
            }

            [Test]
            public void TestAttributes()
            {

                IWebElement temp = driverGC.FindElement(By.Name("charge"));
                IWebElement temp2 = driverME.FindElement(By.Name("charge"));

                IWebElement temp3 = driverGC.FindElement(By.Name("buchen"));
                IWebElement temp4 = driverME.FindElement(By.Name("buchen"));

                temp.Clear();
                temp2.Clear();
                temp.SendKeys("50");
                temp2.SendKeys("50");

                temp3.Click();
                temp4.Click();
                Waittime();

                IReadOnlyCollection<IWebElement> elements = driverGC.FindElements(By.CssSelector("td"));

                IWebElement element = elements.ElementAt(0);
                IReadOnlyCollection<IWebElement> elements2 = driverME.FindElements(By.CssSelector("td"));
                IWebElement element2 = elements2.ElementAt(0);


                Assert.AreEqual(50, element);
                Assert.AreEqual(50, element2);
            }

            [Test]
            public void TestEdgeDriver()
            {
               //driverME.Navigate().GoToUrl("http://www.google.com/");
            }

            [OneTimeTearDown]
            public void OneTimeTearDown()
            {
                driverME.Quit();
                driverME.Dispose();
                driverGC.Quit();
                driverGC.Dispose();
            }

        }
    }
}
