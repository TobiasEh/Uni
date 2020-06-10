using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Support.UI;
using System;

namespace Ent_To_Ent_Tests
{
    

    public class Tests
    {
        
        IWebDriver webDriver;

        [SetUp]
        public void Setup()
        {
            System.Environment.SetEnvironmentVariable("webdriver.chrome.driver", @"E:\Sopro\tutorium-d-team-17\Abgaben\Einzelabgaben\Eh\Ent-To-Ent-Tests\chromedriver.exe");
            webDriver = new ChromeDriver(@"E:\Sopro\tutorium-d-team-17\Abgaben\Einzelabgaben\Eh\Ent-To-Ent-Tests\");
            webDriver.Manage().Window.Maximize();
            
        }

        [Test]
        public void Test1()
        {
            string url = "https://localhost:44325/Booking/Create";
            string url2 = "https://localhost:44325/Booking/";
            webDriver.Url = url;
            System.Threading.Thread.Sleep(5000);


            IWebElement charge = webDriver.FindElement(By.XPath("//input[@name='currentCharge']"));
            charge.SendKeys("10");

            IWebElement distance = webDriver.FindElement(By.XPath("//input[@name='requiredDistance']"));
            distance.SendKeys("100");

            IWebElement start = webDriver.FindElement(By.XPath("//input[@name='start']"));
            start.SendKeys("10092020");
            start.SendKeys(Keys.ArrowRight);
            start.SendKeys("1300");
            start.SendKeys(Keys.Tab);

            IWebElement end = webDriver.FindElement(By.XPath("//input[@name='end']"));
            end.SendKeys("10092020");
            end.SendKeys(Keys.ArrowRight);
            end.SendKeys("1700");
            end.SendKeys(Keys.Tab);

            IWebElement plug = webDriver.FindElement(By.XPath("//select[@id='plugType']"));
            SelectElement selectElement = new SelectElement(plug);
            selectElement.SelectByIndex(1);

            IWebElement submit = webDriver.FindElement(By.XPath("//button[@type='submit']"));

            System.Threading.Thread.Sleep(4000);

            submit.Click();

            System.Threading.Thread.Sleep(2000);
            string url3 = webDriver.Url;
            Assert.AreEqual(url2, url3);
        }

        [TearDown]
        public void kill()
        {
            webDriver.Quit();
        }
    }
}