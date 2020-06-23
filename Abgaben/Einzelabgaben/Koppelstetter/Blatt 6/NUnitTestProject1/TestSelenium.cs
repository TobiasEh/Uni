using NUnit.Framework;
using WebApplication1.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Support.UI;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestContext = NUnit.Framework.TestContext;

namespace UITests
{
    [TestClass]
    public class TestSelenium
    {
        static IWebDriver driverGC;


        [AssemblyInitialize]
        public static void SetUp(TestContext context)
        {
            driverGC = new ChromeDriver();
        }

        [TestMethod]
        public static void TestChromeDriver()
        {
            driverGC.Navigate().GoToUrl("http://www.google.com/");
            driverGC.FindElement(By.Id("lst-ib"))
               .SendKeys("Tapas Pal Codeguru");
            driverGC.FindElement(By.Id("lst-ib"))
               .SendKeys(Keys.Enter);
        }
    }
}