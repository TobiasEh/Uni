using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.IO;
using System.Reflection;
using System.Text;
using UI_Tests.Tests;

namespace UI_Tests.Tests
{
    [TestFixture]
    class VehicleUiTest
    {
        private IWebDriver _driver;
        private string email;
        private BrowserSettings settings;

        [SetUp]
        public void setUp()
        {
            _driver = new ChromeDriver(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location));
            settings = new BrowserSettings();

        }

        public void planerSetUp()
        {
            email = "planer@sopro.de";
            settings.initBrowser(_driver, email);
            System.Threading.Thread.Sleep(2000);
        }

        public void deleteVehicle()
        {
            _driver.FindElement(By.XPath("//form/div/div[2]/div/table/tbody/tr[last()]/td[6]/div/a[2]/img")).Click();
            System.Threading.Thread.Sleep(1000);
        }

        [Test]
        // invalid model
        [TestCase("", true, 100, 40, 90, false, false)]
        // invalid no-plugs
        [TestCase("UITestModel", false, 100, 40, 90, false, false)]
        // invalid capacity
        [TestCase("UITestModel", true, -100, 40, 90, false, false)]
        // invalid socStart
        [TestCase("UITestModel", true, 100, -40, 90, false, false)]
        // invalid socEnd
        [TestCase("UITestModel", true, 100, 40, -90, false, false)]
        // valid
        [TestCase("UITestModel", true, 100, 40, 90, true, true)]
        public void createVehicle(string _model, bool _plug, int _capacity, int _socStart, int _socEnd, bool expected, bool delete)
        {
            planerSetUp();

            _driver.Navigate().GoToUrl("https://sopro-ss2020-team17.azurewebsites.net/Vehicle/Cartemplates");
            System.Threading.Thread.Sleep(3000);

            IWebElement model = _driver.FindElement(By.Id("vehicle_model"));
            model.Clear();
            model.SendKeys(_model);

            IWebElement plug = _driver.FindElement(By.Id("CCS"));
            if (!_plug && plug.Selected)
            {
                plug.Click();
            }
            else if (_plug && !plug.Selected)
            {
                plug.Click();
            }

            IWebElement capacity = _driver.FindElement(By.Id("vehicle_capacity"));
            capacity.Clear();
            capacity.SendKeys(_capacity.ToString());

            IWebElement socStart = _driver.FindElement(By.Id("vehicle_socStart"));
            socStart.Clear();
            socStart.SendKeys(_socStart.ToString());

            IWebElement socEnd = _driver.FindElement(By.Id("vehicle_socEnd"));
            socEnd.Clear();
            socEnd.SendKeys(_socEnd.ToString());

            _driver.FindElement(By.XPath("//form/div/div/div/div[6]/button[2]")).Click();
            
            System.Threading.Thread.Sleep(3000);

            if(expected && !delete)
            {
                Assert.IsTrue(_driver.FindElement(By.XPath("//form/div/div[2]/div/table/tbody/tr[last()]/td")).Text.Equals("UITestModel"));
            }
            else if (expected && delete)
            {
                Assert.IsTrue(_driver.FindElement(By.XPath("//form/div/div[2]/div/table/tbody/tr[last()]/td")).Text.Equals("UITestModel"));
                deleteVehicle();
            }
            else
            {
                try
                {
                    Assert.IsTrue(!_driver.FindElement(By.XPath("//form/div/div[2]/div/table/tbody/tr[last()]/td")).Text.Equals("UITestModel"));
                }
                catch
                {
                    Assert.Pass();
                }
            }

        }

        [Test]
        // invalid model
        [TestCase("", true, 100, 40, 90, false)]
        // invalid no-plugs
        [TestCase("UIChangedModel", false, 100, 40, 90, false)]
        // invalid capacity
        [TestCase("UIChangedModel", true, -100, 40, 90, false)]
        // invalid socStart
        [TestCase("UIChangedModel", true, 100, -40, 90, false)]
        // invalid socEnd
        [TestCase("UIChangedModel", true, 100, 40, -90, false)]
        // valid
        [TestCase("UIChangedModel", true, 100, 40, 90, true)]
        public void changeVehicle(string _model, bool _plug, int _capacity, int _socStart, int _socEnd, bool expected)
        {
            createVehicle("UITestModel", true, 50, 20, 70, true, false);

            _driver.FindElement(By.XPath("//form/div/div[2]/div/table/tbody/tr[last()]/td[6]/div/a/img")).Click();
            System.Threading.Thread.Sleep(1000);

            IWebElement model = _driver.FindElement(By.Id("vehicle_model"));
            model.Clear();
            model.SendKeys(_model);

            IWebElement plug = _driver.FindElement(By.Id("CCS"));
            if (!_plug && plug.Selected)
            {
                plug.Click();
            }
            else if (_plug && !plug.Selected)
            {
                plug.Click();
            }

            IWebElement capacity = _driver.FindElement(By.Id("vehicle_capacity"));
            capacity.Clear();
            capacity.SendKeys(_capacity.ToString());

            IWebElement socStart = _driver.FindElement(By.Id("vehicle_socStart"));
            socStart.Clear();
            socStart.SendKeys(_socStart.ToString());

            IWebElement socEnd = _driver.FindElement(By.Id("vehicle_socEnd"));
            socEnd.Clear();
            socEnd.SendKeys(_socEnd.ToString());

            _driver.FindElement(By.XPath("//div/div/div/form/div[6]/button[2]")).Click();

            System.Threading.Thread.Sleep(3000);

            if (expected)
            {
                Assert.IsTrue(_driver.FindElement(By.XPath("//form/div/div[2]/div/table/tbody/tr[last()]/td")).Text.Equals("UIChangedModel"));
                System.Threading.Thread.Sleep(3000);
                deleteVehicle();
            }
            else
            {
                Assert.IsTrue(_driver.Url.ToString().Contains("https://sopro-ss2020-team17.azurewebsites.net/Vehicle/EndEdit"));

                _driver.Navigate().GoToUrl("https://sopro-ss2020-team17.azurewebsites.net/Vehicle/Cartemplates");
                System.Threading.Thread.Sleep(3000);

                deleteVehicle();
                
            }

        }


        [TearDown]
        public void tearDown()
        {
            settings.kill();
            _driver.Quit();
        }
    }
}
