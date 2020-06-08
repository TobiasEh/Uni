using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Support.UI;
using System;
using System.ComponentModel.DataAnnotations;

namespace NUnitTestProject1
{
    public class operationsBrowser
    {
        IWebDriver webDriver;
        public void initBrowser()
        {
            System.Environment.SetEnvironmentVariable("webdriver.edge.driver", @"D:\Downloads\edgedriver_win64\MicrosoftWebDriver.exe");
            
            //webDriver = new ChromeDriver();
            
            webDriver = new EdgeDriver(@"D:\Downloads\edgedriver_win64\");
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
        operationsBrowser opsBrowser = new operationsBrowser();
        string ulrToTest = "https://localhost:44336/Booking/Create";
        IWebDriver webDriver;

        [SetUp]
        public void Setup()
        {
            opsBrowser.initBrowser();
        }

        [Test]
        public void Test1()
        {
            opsBrowser.Goto(ulrToTest);
            System.Threading.Thread.Sleep(5000);

            webDriver = opsBrowser.getDriver;

            Random r = new Random();

            IWebElement charge = webDriver.FindElement(By.XPath("//input[@name='chargeStatus']"));
            charge.SendKeys(r.Next(0, 100).ToString());

            IWebElement distance = webDriver.FindElement(By.XPath("//input[@name='distance']"));
            distance.SendKeys(r.Next(1, 1000).ToString());
            
            IWebElement startTime = webDriver.FindElement(By.XPath("//input[@name='startTime']"));
            startTime.SendKeys("10102020");
            startTime.SendKeys(Keys.ArrowRight);
            startTime.SendKeys("1000");
            startTime.SendKeys(Keys.Tab);

            IWebElement endTime = webDriver.FindElement(By.XPath("//input[@name='endTime']"));
            endTime.SendKeys("11102020");
            endTime.SendKeys(Keys.ArrowRight);
            endTime.SendKeys("0830");
            endTime.SendKeys(Keys.Tab);

            IWebElement plugType = webDriver.FindElement(By.XPath("//select[@id='plugType']"));
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
            opsBrowser.kill();
        }
    }
}