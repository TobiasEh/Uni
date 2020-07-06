using Microsoft.AspNetCore.Mvc.RazorPages;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Edge;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Reflection.Metadata;
using System.Text;

namespace AutomatedUITests
{
    [TestFixture()]
    class BookingUITest
    {
        private IWebDriver _driver;
        private BookingPage _page;

        [SetUp]
        public void init()
        {
            _driver = new ChromeDriver(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location));
            //_driver = new EdgeDriver(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location));
            _page = new BookingPage(_driver);
            _page.Navigate();
        }

        [Test]
        public void test_successful()
        {
            _page.populateCharge("20");
            _page.populateRequiredDistance("100");

            string startDate = DateTime.Now.Date.ToString("dd'.'MM'.'yyyy");
            string startTime = DateTime.Now.AddHours(1).ToString("HH:mm");
            _page.populateStartDate(startDate, startTime);
            string endDate = DateTime.Now.Date.ToString("dd'.'MM'.'yyyy");
            string endTime = DateTime.Now.AddHours(4).ToString("HH:mm");
            _page.populateEndDate(endDate, endTime);

            _page.populatePlugType();

            _page.ClickCreate();
            Assert.AreEqual("Booking - Blatt03", _page.Title);
        }

        [Test]
        public void test_invalid_charge()
        {
            _page.populateCharge("-20");
            _page.populateRequiredDistance("100");

            string startDate = DateTime.Now.Date.ToString("dd'.'MM'.'yyyy");
            string startTime = DateTime.Now.AddHours(1).ToString("HH:mm");
            _page.populateStartDate(startDate, startTime);
            string endDate = DateTime.Now.Date.ToString("dd'.'MM'.'yyyy");
            string endTime = DateTime.Now.AddHours(4).ToString("HH:mm");
            _page.populateEndDate(endDate, endTime);

            _page.populatePlugType();

            _page.ClickCreate();
            Assert.AreEqual("Create - Blatt03", _page.Title);
        }

        [Test]
        public void test_invalid_distance()
        {
            _page.populateCharge("20");
            _page.populateRequiredDistance("0");

            string startDate = DateTime.Now.Date.ToString("dd'.'MM'.'yyyy");
            string startTime = DateTime.Now.AddHours(1).ToString("HH:mm");
            _page.populateStartDate(startDate, startTime);
            string endDate = DateTime.Now.Date.ToString("dd'.'MM'.'yyyy");
            string endTime = DateTime.Now.AddHours(4).ToString("HH:mm");
            _page.populateEndDate(endDate, endTime);

            _page.populatePlugType();

            _page.ClickCreate();
            Assert.AreEqual("Create - Blatt03", _page.Title);
        }

        [Test]
        public void test_invalid_datetimestart()
        {
            _page.populateCharge("20");
            _page.populateRequiredDistance("100");

            string startDate = "01.01.1999";
            string startTime = "10:00";
            _page.populateStartDate(startDate, startTime);
            string endDate = DateTime.Now.Date.ToString("dd'.'MM'.'yyyy");
            string endTime = DateTime.Now.AddHours(4).ToString("HH:mm");
            _page.populateEndDate(endDate, endTime);

            _page.populatePlugType();

            _page.ClickCreate();
            Assert.AreEqual("Create - Blatt03", _page.Title);
        }

        [Test]
        public void test_invalid_datetimeend()
        {
            _page.populateCharge("20");
            _page.populateRequiredDistance("100");

            string startDate = DateTime.Now.Date.ToString("dd'.'MM'.'yyyy");
            string startTime = DateTime.Now.AddHours(1).ToString("HH:mm");
            _page.populateStartDate(startDate, startTime);
            string endDate = "01.01.1999";
            string endTime = "10:00";
            _page.populateEndDate(endDate, endTime);

            _page.populatePlugType();

            _page.ClickCreate();
            Assert.AreEqual("Create - Blatt03", _page.Title);
        }

        [Test]
        public void test_end_earlier_than_start()
        {
            _page.populateCharge("20");
            _page.populateRequiredDistance("100");

            string startDate = DateTime.Now.Date.ToString("dd'.'MM'.'yyyy");
            string startTime = DateTime.Now.AddHours(4).ToString("HH:mm");
            _page.populateStartDate(startDate, startTime);
            string endDate = DateTime.Now.Date.ToString("dd'.'MM'.'yyyy");
            string endTime = DateTime.Now.AddHours(1).ToString("HH:mm");
            _page.populateEndDate(endDate, endTime);

            _page.populatePlugType();

            _page.ClickCreate();
            Assert.AreEqual("Create - Blatt03", _page.Title);
        }

        [Test]
        public void test_allfail()
        {
            _page.populateCharge("-20");
            _page.populateRequiredDistance("0");

            string startDate = "01.01.1999";
            string startTime = "10:00";
            _page.populateStartDate(startDate, startTime);
            string endDate = "01.01.1999";
            string endTime = "10:00";
            _page.populateEndDate(endDate, endTime);

            _page.populatePlugType();

            _page.ClickCreate();
            Assert.AreEqual("Create - Blatt03", _page.Title);
        }

        [TearDown]
        public void kill()
        {
            _driver.Dispose();
            _driver.Quit();
        }

    }
}
