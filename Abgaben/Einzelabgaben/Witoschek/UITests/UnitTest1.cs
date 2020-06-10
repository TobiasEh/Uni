using Blatt03.Models;
using Blatt03.ViewModel.CustomValidation;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;

namespace UITests
{
    public class operationsBrowser
    {
        IWebDriver webDriver;
        public void initBrowser()
        {
            System.Environment.SetEnvironmentVariable("webdriver.chrome.driver", @"C:\Users\Dominik\Desktop\chromedriver\chromedriver.exe");
            webDriver = new ChromeDriver(@"C:\Users\Dominik\Desktop\chromedriver\");
            webDriver.Manage().Window.Maximize();

            //System.Environment.SetEnvironmentVariable("webdriver.edge.driver", @"C:\Users\Dominik\Desktop\edgedriver_win64\msedgedriver.exe");
            //webDriver = new EdgeDriver(@"C:\Users\Dominik\Desktop\edgedriver_win64\");
            //webDriver.Manage().Window.Maximize();
        }

        public void Goto(string url)
        {
            webDriver.Url = url;
        }

        public IWebDriver getDriver
        {
            get { return webDriver; }
        }

        public void kill()
        {
            webDriver.Quit();
        }
    }

    public class Tests
    {
        operationsBrowser opsBrowser = new operationsBrowser();
        string ulr = "https://localhost:44365/Booking/create";
        IWebDriver webDriver;

        [SetUp]
        public void Setup()
        {
            opsBrowser.initBrowser();
        }

        [TestCase(22, 100, "10.10.2020", "10:00", "10.10.2020", "13:45")]
        public void Test1(int chargeP, int distanceP, string startDateP, string startTimeP, string endDateP, string endTimeP)
        {
            opsBrowser.Goto(ulr);
            System.Threading.Thread.Sleep(5000);

            webDriver = opsBrowser.getDriver;

            IWebElement charge = webDriver.FindElement(By.XPath("//input[@name='currentCharge']"));
            charge.SendKeys(chargeP.ToString());

            IWebElement distance = webDriver.FindElement(By.XPath("//input[@name='requiredDistance']"));
            distance.SendKeys(distanceP.ToString());

            IWebElement startTime = webDriver.FindElement(By.XPath("//input[@name='start']"));
            startTime.SendKeys(startDateP);
            startTime.SendKeys(Keys.ArrowRight);
            startTime.SendKeys(startTimeP);
            startTime.SendKeys(Keys.Tab);

            IWebElement endTime = webDriver.FindElement(By.XPath("//input[@name='end']"));
            endTime.SendKeys(endDateP);
            endTime.SendKeys(Keys.ArrowRight);
            endTime.SendKeys(endTimeP);
            endTime.SendKeys(Keys.Tab);

            Random r = new Random();
            IWebElement connectorType = webDriver.FindElement(By.XPath("//select[@name='connectorType']"));
            SelectElement selectElement = new SelectElement(connectorType);
            selectElement.SelectByIndex(r.Next(1, 6));

            IWebElement submit = webDriver.FindElement(By.XPath("//button[@type='submit']"));

            System.Threading.Thread.Sleep(2000);

            submit.Click();
            System.Threading.Thread.Sleep(1000);
            Assert.IsTrue(webDriver.Url.ToString().Equals("https://localhost:44365/Booking/Post"));
        }
        [TearDown]
        public void kill()
        {
            opsBrowser.kill();
        }
    }
}