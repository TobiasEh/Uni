
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace NUnitTestProject1
{
    public class Browser_ops
    {
        IWebDriver webDriver;
        public void Init_Browser()
        {
            webDriver = new ChromeDriver();
            webDriver.Manage().Window.Maximize();
        }
        public string Title
        {
            get { return webDriver.Title; }
        }
        public void Goto(string url)
        {
            webDriver.Url = url;
        }
        public void Close()
        {
            webDriver.Quit();
        }
        public IWebDriver getDriver
        {
            get { return webDriver; }
        }
    }
    class NUnit_UI_test
    {
        Browser_ops brow = new Browser_ops();
        string test_url = "https://localhost:44327/Booking/create";
        IWebDriver driver;

        [SetUp]
        public void start_Browser()
        {
            brow.Init_Browser();
        }

        [Test]
        public void test_Browserops()
        {
            brow.Goto(test_url);
            System.Threading.Thread.Sleep(4000);

            driver = brow.getDriver;

            IWebElement cur_charge = driver.FindElement(By.XPath("//*[@id='cur_charge']"));
            IWebElement needed_dist = driver.FindElement(By.XPath("//*[@id='needed_distance']"));
            IWebElement startTime = driver.FindElement(By.XPath("//*[@id='startTime']"));
            IWebElement endTIme = driver.FindElement(By.XPath("//*[@id='endTime']"));
            IWebElement plug_type = driver.FindElement(By.XPath("//*[@id='connectorType']"));
            IWebElement submit = driver.FindElement(By.XPath("//*[@id='submit_btn']"));

            var selectElement = new SelectElement(plug_type);

            cur_charge.Clear();
            cur_charge.SendKeys("69");
            needed_dist.Clear();
            needed_dist.SendKeys("123");
            startTime.SendKeys("20042069");
            startTime.SendKeys(Keys.Tab);
            startTime.SendKeys("0420");
            endTIme.SendKeys("20042069");
            endTIme.SendKeys(Keys.Tab);
            endTIme.SendKeys("1620");
            selectElement.SelectByValue("type_a");

          
            submit.Click();

            /* Perform wait to check the output */
            System.Threading.Thread.Sleep(2000);
        }

        [TearDown]
        public void close_Browser()
        {
            brow.Close();
        }
    }
}