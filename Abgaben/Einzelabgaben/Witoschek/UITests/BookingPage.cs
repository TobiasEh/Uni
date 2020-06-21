using Newtonsoft.Json;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Text;

namespace AutomatedUITests
{
    class BookingPage
    {
        private Random r = new Random();
        private readonly IWebDriver _driver;
        private const string URLCREATE = "https://localhost:44365/Booking/create";

        private IWebElement chargeElement => _driver.FindElement(By.Id("in_currentCharge"));
        private IWebElement requiredDistanceElement => _driver.FindElement(By.Id("in_requiredDistance"));
        private IWebElement startDateTimeElement => _driver.FindElement(By.Id("in_start")); 
        private IWebElement endDateTimeElement => _driver.FindElement(By.Id("in_end"));
        private IWebElement plugTypeElement => _driver.FindElement(By.Id("in_connectorType"));
        private IWebElement createElement => _driver.FindElement(By.Id("Create"));

        public string Title => _driver.Title;
        public string Source => _driver.PageSource;

        public BookingPage(IWebDriver driver)
        {
            _driver = driver;
        }

        public void Navigate() => _driver.Navigate().GoToUrl(URLCREATE);
        public void populateCharge(string currentCharge) => chargeElement.SendKeys(currentCharge);
        public void populateRequiredDistance(string requiredDistance) => requiredDistanceElement.SendKeys(requiredDistance);
        public void populateStartDate(string date, string time)
        {
            startDateTimeElement.SendKeys(date);
            startDateTimeElement.SendKeys(Keys.ArrowRight);
            startDateTimeElement.SendKeys(time);
            startDateTimeElement.SendKeys(Keys.Tab);
            
        }
        public void populateEndDate(string date, string time)
        {
            endDateTimeElement.SendKeys(date);
            endDateTimeElement.SendKeys(Keys.ArrowRight);
            endDateTimeElement.SendKeys(time);
            endDateTimeElement.SendKeys(Keys.Tab);
        }
        public void populatePlugType()
        {
            SelectElement selectElement = new SelectElement(plugTypeElement);
            selectElement.SelectByIndex(r.Next(1, 6));
        }

        public void ClickCreate() {
            createElement.Click();
            System.Threading.Thread.Sleep(1000);
        }
        
        // public void populateStartDate();
    }
}
