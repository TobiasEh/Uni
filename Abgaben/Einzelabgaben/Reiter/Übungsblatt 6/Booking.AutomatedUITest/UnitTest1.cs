using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Edge;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using Xunit;

namespace Booking.AutomatedUITest
{
    public class Tests
    {
        private IWebDriver _driverC;
        //private IWebDriver _driverE;

        [SetUp]
        public void Setup()
        {
            _driverC = new ChromeDriver();
            //_driverE = new EdgeDriver();
        }

        [Test]
        public void BasicTestChrome()
        {
            _driverC.Navigate()
                 .GoToUrl("https://localhost:44301/Booking/create");

            Assert.AreEqual("Create - TestProjekt", _driverC.Title);

            _driverC.Quit();
            _driverC.Dispose();
        }

        [Test]
        public void EvaluationTestChrome()
        {
            //Create Booking and look if Evaluation works
            _driverC.Navigate()
                 .GoToUrl("https://localhost:44301/Booking/create");
            _driverC.FindElement(By.Id("currentCharge")).SendKeys("12");
            _driverC.FindElement(By.Id("requiredDistance")).SendKeys("120");
            _driverC.FindElement(By.Id("start")).SendKeys("17.06.2020\t12:00");
            _driverC.FindElement(By.Id("end")).SendKeys("17.06.2020\t14:00");
            _driverC.FindElement(By.Id("connectorType")).SendKeys("Type1Plug");

            _driverC.FindElement(By.Id("createB")).Click();

            _driverC.Navigate()
                 .GoToUrl("https://localhost:44301/Booking");

            _driverC.Navigate()
                 .GoToUrl("https://localhost:44301/Booking/Evaluation");

            IReadOnlyCollection<IWebElement> types = _driverC.FindElements(By.CssSelector("td"));
            IWebElement last = types.ElementAt(0);

            foreach (IWebElement type in types)
            {
                if(last.Text.Equals("Type1Plug"))
                {
                    Console.WriteLine(type.ToString());
                    Assert.IsTrue(type.Text.Equals("100"));
                }
                last = type;
            }

            _driverC.Quit();
            _driverC.Dispose();
        }

        /*
        [Test]
        public void BasicTestEdge()
        {
            _driverE.Navigate()
                 .GoToUrl("https://localhost:44301/Booking/create");

            Assert.AreEqual("Create - TestProjekt", _driverE.Title);

            _driverE.Quit();
            _driverE.Dispose();
        }

        [Test]
        public void EvaluationTestEdge()
        {
            //Create Booking and look if Evaluation works
            _driverE.Navigate()
                 .GoToUrl("https://localhost:44301/Booking/create");
            _driverE.FindElement(By.Id("currentCharge")).SendKeys("12");
            _driverE.FindElement(By.Id("requiredDistance")).SendKeys("120");
            _driverE.FindElement(By.Id("start")).SendKeys("17.06.2020\t12:00");
            _driverE.FindElement(By.Id("end")).SendKeys("17.06.2020\t14:00");
            _driverE.FindElement(By.Id("connectorType")).SendKeys("Type1Plug");

            _driverE.FindElement(By.Id("createB")).Click();

            _driverE.Navigate()
                 .GoToUrl("https://localhost:44301/Booking");

            _driverE.Navigate()
                 .GoToUrl("https://localhost:44301/Booking/Evaluation");

            IReadOnlyCollection<IWebElement> types = _driverE.FindElements(By.CssSelector("td"));
            IWebElement last = types.ElementAt(0);

            foreach (IWebElement type in types)
            {
                if (last.Text.Equals("Type1Plug"))
                {
                    Console.WriteLine(type.ToString());
                    Assert.IsTrue(type.Text.Equals("100"));
                }
                last = type;
            }

            _driverE.Quit();
            _driverE.Dispose();
        }*/
    }
}