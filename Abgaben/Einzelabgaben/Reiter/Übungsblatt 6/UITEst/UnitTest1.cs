using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Support.UI;
using System;

namespace UITEst
{
    public class operationsBrowser
    {
        IWebDriver webDriver;
        public void initBrowser()
        {
            System.Environment.SetEnvironmentVariable("webdriver.edge.driver", @"G:\Uni Git\tutorium-d-team-17\Abgaben\Einzelabgaben\Reiter\Übungsblatt 6\UITEst\MicrosoftWebDriver.exe");

            webDriver = new ChromeDriver(@"G:\Uni Git\tutorium-d-team-17\Abgaben\Einzelabgaben\Reiter\Übungsblatt 6\UITEst\");

            webDriver = new EdgeDriver(@"G:\Uni Git\tutorium-d-team-17\Abgaben\Einzelabgaben\Reiter\Übungsblatt 6\UITEst\");
            webDriver.Manage().Window.Maximize();
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
        operationsBrowser browser = new operationsBrowser();
        string ulr = "https://localhost:44336/Booking/Create";
        IWebDriver webDriver;

        [SetUp]
        public void Setup()
        {
            browser.initBrowser();
        }

        [Test]
        public void Test1()
        {
            browser.Goto(ulr);
            System.Threading.Thread.Sleep(5000);

            webDriver = browser.getDriver;

            Random r = new Random();

            IWebElement charge = webDriver.FindElement(By.XPath("//input[@name='currentCharge']"));
            charge.SendKeys(r.Next(0, 100).ToString());

            IWebElement distance = webDriver.FindElement(By.XPath("//input[@name='requiredDistance']"));
            distance.SendKeys(r.Next(1, 1000).ToString());

            IWebElement startTime = webDriver.FindElement(By.XPath("//input[@name='start']"));
            startTime.SendKeys("10102020");
            startTime.SendKeys(Keys.ArrowRight);
            startTime.SendKeys("1000");
            startTime.SendKeys(Keys.Tab);

            IWebElement endTime = webDriver.FindElement(By.XPath("//input[@name='end']"));
            endTime.SendKeys("11102020");
            endTime.SendKeys(Keys.ArrowRight);
            endTime.SendKeys("0830");
            endTime.SendKeys(Keys.Tab);

            IWebElement plugType = webDriver.FindElement(By.XPath("//select[@id='connectorType']"));
            SelectElement selectElement = new SelectElement(plugType);
            selectElement.SelectByIndex(r.Next(1, 6));

            IWebElement submit = webDriver.FindElement(By.XPath("//button[@type='submit']"));

            System.Threading.Thread.Sleep(4000);

            submit.Click();

            System.Threading.Thread.Sleep(2000);
        }

        [TearDown]
        public void kill()
        {
            browser.kill();
        }
    }
}