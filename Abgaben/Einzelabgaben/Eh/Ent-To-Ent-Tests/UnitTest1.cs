using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace Ent_To_Ent_Tests
{
    

    public class Tests
    {
        
        private IWebDriver webDriver;

        [OneTimeSetUp]
        public void Setup()
        {
            webDriver = new ChromeDriver();
        }

        [Test]
        public void NoInput()
        {
            webDriver.Navigate().GoToUrl("https://localhost:44325/Booking/Create");

            webDriver.FindElement(By.XPath("//button[@type='submit']")).Click();

            Assert.AreEqual("Create - Website", webDriver.Title);
        }

        [Test]
        public void NoCurrentCharge()
        {

            webDriver.Navigate().GoToUrl("https://localhost:44325/Booking/Create");

            webDriver.FindElement(By.Name("requiredDistance")).SendKeys("12");

            webDriver.FindElement(By.Name("start")).SendKeys("12122012");
            webDriver.FindElement(By.Name("start")).SendKeys(Keys.Tab);
            webDriver.FindElement(By.Name("start")).SendKeys("1212");

            webDriver.FindElement(By.Name("end")).SendKeys("12122012");
            webDriver.FindElement(By.Name("end")).SendKeys(Keys.Tab);
            webDriver.FindElement(By.Name("end")).SendKeys("1312");

            webDriver.FindElement(By.XPath("//button[@type='submit']")).Click();

            Assert.AreEqual("Create - Website", webDriver.Title);
        }


        [Test]
        public void NoRequiredDistance()
        {

            webDriver.Navigate().GoToUrl("https://localhost:44325/Booking/Create");

            webDriver.FindElement(By.Name("currentCharge")).SendKeys("10"); 


            webDriver.FindElement(By.Name("start")).SendKeys("12122012");
            webDriver.FindElement(By.Name("start")).SendKeys(Keys.Tab);
            webDriver.FindElement(By.Name("start")).SendKeys("1212");

            webDriver.FindElement(By.Name("end")).SendKeys("12122012");
            webDriver.FindElement(By.Name("end")).SendKeys(Keys.Tab);
            webDriver.FindElement(By.Name("end")).SendKeys("1312");

            webDriver.FindElement(By.XPath("//button[@type='submit']")).Click();

            Assert.AreEqual("Create - Website", webDriver.Title);
        }


        [Test]
        public void NoStartTime()
        {

            webDriver.Navigate().GoToUrl("https://localhost:44325/Booking/Create");

            webDriver.FindElement(By.Name("requiredDistance")).SendKeys("12");

            webDriver.FindElement(By.Name("currentCharge")).SendKeys("10");

            webDriver.FindElement(By.Name("end")).SendKeys("12122020");
            webDriver.FindElement(By.Name("end")).SendKeys(Keys.Tab);
            webDriver.FindElement(By.Name("end")).SendKeys("1312");

            webDriver.FindElement(By.XPath("//button[@type='submit']")).Click();

            Assert.AreEqual("Create - Website", webDriver.Title);
        }

        [Test]
        public void NoEndTime()
        {

            webDriver.Navigate().GoToUrl("https://localhost:44325/Booking/Create");

            webDriver.FindElement(By.Name("requiredDistance")).SendKeys("12");

            webDriver.FindElement(By.Name("currentCharge")).SendKeys("10");

            webDriver.FindElement(By.Name("start")).SendKeys("12122020");
            webDriver.FindElement(By.Name("start")).SendKeys(Keys.Tab);
            webDriver.FindElement(By.Name("start")).SendKeys("1312");

            webDriver.FindElement(By.XPath("//button[@type='submit']")).Click();

            Assert.AreEqual("Create - Website", webDriver.Title);
        }

        [Test]
        public void WrongCurrentCharge()
        {
            webDriver.Navigate().GoToUrl("https://localhost:44325/Booking/Create");

            webDriver.FindElement(By.Name("currentCharge")).SendKeys("1200");

            webDriver.FindElement(By.Name("requiredDistance")).SendKeys("12");

            webDriver.FindElement(By.Name("start")).SendKeys("12122020");
            webDriver.FindElement(By.Name("start")).SendKeys(Keys.Tab);
            webDriver.FindElement(By.Name("start")).SendKeys("1212");

            webDriver.FindElement(By.Name("end")).SendKeys("12122020");
            webDriver.FindElement(By.Name("end")).SendKeys(Keys.Tab);
            webDriver.FindElement(By.Name("end")).SendKeys("1312");

            webDriver.FindElement(By.XPath("//button[@type='submit']")).Click();

            Assert.AreEqual("Create - Website",webDriver.Title);
        }

        [Test]
        public void WrongRequiredDistance()
        {
            webDriver.Navigate().GoToUrl("https://localhost:44325/Booking/Create");

            webDriver.FindElement(By.Name("currentCharge")).SendKeys("10");

            webDriver.FindElement(By.Name("requiredDistance")).SendKeys("1276");

            webDriver.FindElement(By.Name("start")).SendKeys("12122020");
            webDriver.FindElement(By.Name("start")).SendKeys(Keys.Tab);
            webDriver.FindElement(By.Name("start")).SendKeys("1212");

            webDriver.FindElement(By.Name("end")).SendKeys("12122020");
            webDriver.FindElement(By.Name("end")).SendKeys(Keys.Tab);
            webDriver.FindElement(By.Name("end")).SendKeys("1312");

            webDriver.FindElement(By.XPath("//button[@type='submit']")).Click();

            Assert.AreEqual("Create - Website", webDriver.Title);
        }

        public void WrongStartTime()
        {
            webDriver.Navigate().GoToUrl("https://localhost:44325/Booking/Create");

            webDriver.FindElement(By.Name("currentCharge")).SendKeys("10");

            webDriver.FindElement(By.Name("requiredDistance")).SendKeys("12");

            webDriver.FindElement(By.Name("start")).SendKeys("12012020");
            webDriver.FindElement(By.Name("start")).SendKeys(Keys.Tab);
            webDriver.FindElement(By.Name("start")).SendKeys("1212");

            webDriver.FindElement(By.Name("end")).SendKeys("12122020");
            webDriver.FindElement(By.Name("end")).SendKeys(Keys.Tab);
            webDriver.FindElement(By.Name("end")).SendKeys("1312");

            webDriver.FindElement(By.XPath("//button[@type='submit']")).Click();

            Assert.AreEqual("Create - Website", webDriver.Title);
        }

        public void WrongEndTimePast()
        {
            webDriver.Navigate().GoToUrl("https://localhost:44325/Booking/Create");

            webDriver.FindElement(By.Name("currentCharge")).SendKeys("10");

            webDriver.FindElement(By.Name("requiredDistance")).SendKeys("12");

            webDriver.FindElement(By.Name("start")).SendKeys("12122020");
            webDriver.FindElement(By.Name("start")).SendKeys(Keys.Tab);
            webDriver.FindElement(By.Name("start")).SendKeys("1212");

            webDriver.FindElement(By.Name("end")).SendKeys("01122020");
            webDriver.FindElement(By.Name("end")).SendKeys(Keys.Tab);
            webDriver.FindElement(By.Name("end")).SendKeys("1312");

            webDriver.FindElement(By.XPath("//button[@type='submit']")).Click();

            Assert.AreEqual("Create - Website", webDriver.Title);
        }

        public void WrongEndTimeBeforeStart()
        {
            webDriver.Navigate().GoToUrl("https://localhost:44325/Booking/Create");

            webDriver.FindElement(By.Name("currentCharge")).SendKeys("10");

            webDriver.FindElement(By.Name("requiredDistance")).SendKeys("12");

            webDriver.FindElement(By.Name("start")).SendKeys("12122020");
            webDriver.FindElement(By.Name("start")).SendKeys(Keys.Tab);
            webDriver.FindElement(By.Name("start")).SendKeys("1212");

            webDriver.FindElement(By.Name("end")).SendKeys("12122020");
            webDriver.FindElement(By.Name("end")).SendKeys(Keys.Tab);
            webDriver.FindElement(By.Name("end")).SendKeys("1112");

            webDriver.FindElement(By.XPath("//button[@type='submit']")).Click();

            Assert.AreEqual("Create - Website", webDriver.Title);
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            webDriver.Quit();
            webDriver.Dispose();
        }
        
    }
}