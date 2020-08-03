using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace UI_Tests.Tests
{
    [TestFixture]
    class SzenarioTests
    {

        private IWebDriver _driver;
        private string email;
        private BrowserSettings settings;
        private string js = "arguments[0].scrollIntoView(true)";
        private VehicleUiTest _vehicle;
        private UITests _location;

        [SetUp]
        public void setUp()
        {
            _driver = new ChromeDriver(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location));
            settings = new BrowserSettings();

            _vehicle = new VehicleUiTest() { settings = settings, _driver = _driver };
            _location = new UITests() { settings = settings, _driver = _driver };
        }

        public void planerSetUp()
        {
            email = "planer@sopro.de";
            settings.initBrowser(_driver, email);
            System.Threading.Thread.Sleep(2000);
        }

        public void deleteVehicle()
        {
            IWebElement bin = _driver.FindElement(By.XPath("//form/div/div[2]/div/table/tbody/tr[last()]/td[6]/div/a[2]/img"));

            ((IJavaScriptExecutor)_driver).ExecuteScript(js, bin);
            System.Threading.Thread.Sleep(1000);

            bin.Click();
            System.Threading.Thread.Sleep(1000);
        }

        public void javascript(IWebElement element)
        {
            ((IJavaScriptExecutor)_driver).ExecuteScript(js, element);
            System.Threading.Thread.Sleep(1000);
        }


        [Test]
        // invlaid duration
        [TestCase("03092020", -4, 100, 2, 1, false)]
        // invlaid bookingCount
        [TestCase("03092020", 4, -100, 2, 1, false)]
        // invlaid count Rushhour
        [TestCase("03092020", 4, 100, -2, 1, false)]
        // invlaid vehilce count
        // [TestCase("03092020", 4, 100, 2, -1, false)]
        // valid
        [TestCase("03092020", 4, 100, 2, 1, true)]
        public void createSimulation(string _date, int _duration, int _bookingCount, int _countRush, int _vehicleCount, bool expected)
        {
            

            _vehicle.createVehicle("UITestModel", true, 100, 20, 80, true, false);


            
            _location.createZone(2, 200, 3, 150, 3, 2000, 20000, true);

            _driver.Navigate().GoToUrl("https://sopro-ss2020-team17.azurewebsites.net/Simulation");

            IWebElement createButton = _driver.FindElement(By.XPath("//div[2]/form/button"));
            javascript(createButton);
            createButton.Click();
            System.Threading.Thread.Sleep(2000);

            IWebElement date = _driver.FindElement(By.Id("DatePickerId"));
            date.Clear();
            date.SendKeys(_date);

            IWebElement duration = _driver.FindElement(By.Id("scenario_duration"));
            duration.Clear();
            duration.SendKeys(_duration.ToString());

            IWebElement bookingCount = _driver.FindElement(By.Id("scenario_bookingCountPerDay"));
            bookingCount.Clear();
            bookingCount.SendKeys(_bookingCount.ToString());

            IWebElement location = _driver.FindElement(By.Id("idLocation"));
            SelectElement selectElement = new SelectElement(location);
            selectElement.SelectByText("UITestLocation");

            IWebElement countRushhour = _driver.FindElement(By.Id("countRushhours"));
            countRushhour.Clear();
            countRushhour.SendKeys(_countRush.ToString());

            IWebElement vehicle = _driver.FindElement(By.XPath("//form/div[5]/div/table/tbody/tr[last()]/td[last()]/input"));
            vehicle.Clear();
            vehicle.SendKeys(_vehicleCount.ToString());

            IWebElement goOnButton = _driver.FindElement(By.XPath("//button[@type='submit']"));
            javascript(goOnButton);
            goOnButton.Click();

            if(expected)
            {
                Assert.IsTrue(_driver.Url.Contains("https://sopro-ss2020-team17.azurewebsites.net/Simulation/editRushours"));
            } 
            else if (!expected)
            {
                bool create = _driver.Url.Contains("https://sopro-ss2020-team17.azurewebsites.net/Simulation/Create");
                //bool edit = _driver.Url.Contains("https://sopro-ss2020-team17.azurewebsites.net/Simulation/Edit");
                Assert.IsTrue(create);
            }
        }

        [Test]
        // invalid endtime
        [TestCase("04082020", "0800", "0700", 0.01, false)]
        // valid 
        [TestCase("04082020", "0800", "0900", 1, true)]
        public void rushhourTest(string _startdate, string _starttime, string _endtime, double _spread, bool expected)
        {
            createSimulation("04082020", 4, 100, 1, 1, true);

            IWebElement starttime1 = _driver.FindElement(By.XPath("//div/form/div/div[2]/input"));
            starttime1.Clear();
            starttime1.SendKeys(_startdate);
            starttime1.SendKeys(Keys.ArrowRight);
            starttime1.SendKeys(_starttime);

            System.Threading.Thread.Sleep(2000);

            IWebElement endtime1 = _driver.FindElement(By.XPath("//div/form/div/div[3]/input"));
            endtime1.Clear();
            endtime1.SendKeys(_endtime);

            System.Threading.Thread.Sleep(2000);

            IWebElement spread = _driver.FindElement(By.XPath("//div/form/div/div[4]/input"));
            spread.Clear();
            spread.SendKeys(_spread.ToString());
            System.Threading.Thread.Sleep(2000);

            IWebElement submit = _driver.FindElement(By.XPath("//div/form/button"));
            submit.Click();

            System.Threading.Thread.Sleep(2000);

            if (expected)
            {
                Assert.IsTrue(_driver.Url.ToString().Contains("https://sopro-ss2020-team17.azurewebsites.net/Simulation/EditLocationScenario"));
            } else
            {
                Assert.IsTrue(_driver.Url.ToString().Contains("https://sopro-ss2020-team17.azurewebsites.net/Simulation/editRushours"));
            }
        }

        [Test]
        public void startScenario()
        {
            rushhourTest("04082020", "0800", "0900", 1, true);

            IWebElement readyButton = _driver.FindElement(By.XPath("//div[@class='d-flex mt-4']/form/button"));
            readyButton.Click();

            System.Threading.Thread.Sleep(4000);

            IWebElement startSimulation = _driver.FindElement(By.XPath("//div/div[last()-1]/a/img"));
            startSimulation.Click();

            System.Threading.Thread.Sleep(3000);

            Assert.IsTrue(_driver.Url.ToString().Contains("https://sopro-ss2020-team17.azurewebsites.net/Simulation/Evaluation"));

            _driver.Navigate().GoToUrl("https://sopro-ss2020-team17.azurewebsites.net/Simulation");

            System.Threading.Thread.Sleep(2000);

            IWebElement deleteSimulation = _driver.FindElement(By.XPath("//div/div[last()-1]/a[3]/img"));
            deleteSimulation.Click();
        }

        [TearDown]
        public void tearDown()
        {
            _driver.Navigate().GoToUrl("https://sopro-ss2020-team17.azurewebsites.net/Infrastructure");
            System.Threading.Thread.Sleep(2000);
            _location.deleteLocation();

            _driver.Navigate().GoToUrl("https://sopro-ss2020-team17.azurewebsites.net/Vehicle/Cartemplates");
            _vehicle.deleteVehicle();


            settings.kill();
            _driver.Quit();
        }
    }
}
