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
            

            //System.Environment.SetEnvironmentVariable("webdriver.chrome.driver", @"D:\Downloads\chromedriver_win32\chromedriver.exe");
            //webDriver = new ChromeDriver();

            System.Environment.SetEnvironmentVariable("webdriver.edge.driver", @"D:\Downloads\edgedriver_win64\MicrosoftWebDriver.exe");
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

        //[TestCase(20, null, "10102020", "1000", "10102020", "1345", false)]
        //[TestCase(1, 0, "10102020", "1000", "10102020", "1345", false)]
        [TestCase(20, 120, "10102020", "1000", "10102020", "1345", true)]
        public void Test1(int chargeP, int distanceP, string startDateP, string startTimeP, string endDateP, string endTimeP, bool expected)
        {
            opsBrowser.Goto(ulrToTest);
            System.Threading.Thread.Sleep(5000);

            webDriver = opsBrowser.getDriver;
            var exeption = new WebDriverException();

            IWebElement charge = webDriver.FindElement(By.XPath("//input[@name='chargeStatus']"));
            charge.SendKeys(chargeP.ToString());

            IWebElement distance = webDriver.FindElement(By.XPath("//input[@name='distance']"));
            distance.SendKeys(distanceP.ToString());
            
            IWebElement startTime = webDriver.FindElement(By.XPath("//input[@name='startTime']"));
            startTime.SendKeys(startDateP);
            startTime.SendKeys(Keys.ArrowRight);
            startTime.SendKeys(startTimeP);
            startTime.SendKeys(Keys.Tab);

            IWebElement endTime = webDriver.FindElement(By.XPath("//input[@name='endTime']"));
            endTime.SendKeys(endDateP);
            endTime.SendKeys(Keys.ArrowRight);
            endTime.SendKeys(endTimeP);
            endTime.SendKeys(Keys.Tab);

            Random r = new Random();
            IWebElement plugType = webDriver.FindElement(By.XPath("//select[@id='plugType']"));
            SelectElement selectElement = new SelectElement(plugType);
            selectElement.SelectByIndex(r.Next(1, 6));

            IWebElement submit = webDriver.FindElement(By.XPath("//button[@type='submit']"));

            System.Threading.Thread.Sleep(2000);
            

            if (expected == false)
            {
                
                submit.Click();
                System.Threading.Thread.Sleep(2000);
                var b = charge.FindElement(By.CssSelector("input:required:invalid"));
                Assert.IsTrue(b.Text.ToString().Contains("Wert"));
            } else
            {
                submit.Click();
            }

            System.Threading.Thread.Sleep(2000);
        }
        [TearDown]
        public void kill()
        {
            opsBrowser.kill();
        }
    }
}