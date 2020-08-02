using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Html5;
using OpenQA.Selenium.Support.UI;

namespace UI_Tests.Tests
{
    [TestFixture]
    class UITests
    {
        // HINWEIS!!
        // Bei jedem Test außer createLocatio wird zubeginn ein Standort erstellt.
        // Danach wird bei jedem Test, außer denen die den Standort betreffen und createZone, eine Zone erstellt. 
        // Dies geschieht im Test und ist auch zu sehen,
        // da die Tests so zum einen unabhängig von einander ausgeführt werden können,
        // sicherer getestet werden kann und die Tests auch auf der Azure Website laufen können.
        // Nach jedem Test wird die Location (ggf. mit der Zone) wieder gelöscht.

        private IWebDriver _driver;
        private BrowserSettings settings;
        private string email;
        private string js = "arguments[0].scrollIntoView(true)";

        [SetUp]
        public void setUp()
        {
            _driver = new ChromeDriver(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location));
            settings = new BrowserSettings();
            
        }
        
        public void locationSetUp()
        {
            email = "planer@sopro.de";
            settings.initBrowser(_driver, email);
             _driver.Navigate().GoToUrl("https://sopro-ss2020-team17.azurewebsites.net/Infrastructure");
            System.Threading.Thread.Sleep(2000);
        }
        public void deleteLocation()
        {
            IWebElement bin = _driver.FindElement(By.XPath("//div[@class='flex-column']/div[last()]/div/a/img"));
            ((IJavaScriptExecutor)_driver).ExecuteScript(js, bin);
            System.Threading.Thread.Sleep(1000);
            bin.Click();
            System.Threading.Thread.Sleep(1000);

        }

        public void deleteZone()
        {
            _driver.Navigate().GoToUrl("https://sopro-ss2020-team17.azurewebsites.net/Infrastructure");
            System.Threading.Thread.Sleep(2000);

            IWebElement arrow = _driver.FindElement(By.XPath("//div[@class='flex-column']/div[last()]/div/img[2]"));
            ((IJavaScriptExecutor)_driver).ExecuteScript(js, arrow);
            System.Threading.Thread.Sleep(1000);

            arrow.Click();

            System.Threading.Thread.Sleep(2000);

            IWebElement bin = _driver.FindElement(By.XPath("//div[@class='flex-column']/div[last()]/div[3]/div/div[last()]/div[1]/div/div[2]/form/button/img"));
            ((IJavaScriptExecutor)_driver).ExecuteScript(js, bin);
            System.Threading.Thread.Sleep(1000);

            bin.Click();
            System.Threading.Thread.Sleep(2000);
        }

        public void deleteBooking()
        {
            IWebElement bin = _driver.FindElement(By.XPath("//div[@class='table-responsive rounded']/table/tbody/tr[last()]/td[6]/div/a[2]/img"));

            ((IJavaScriptExecutor)_driver).ExecuteScript(js, bin);
            System.Threading.Thread.Sleep(1000);

            bin.Click();
            System.Threading.Thread.Sleep(1000);
        }

        public void bookingSetUp()
        {
            email = "employee@sopro.de";
            settings.initBrowser(_driver, email);
            _driver.Navigate().GoToUrl("https://sopro-ss2020-team17.azurewebsites.net/Booking/Create?");
            System.Threading.Thread.Sleep(2000);
        }

        [Test]
        // invalid emergency
        [TestCase(-0.05, false)]
        // invalid emergency
        [TestCase(2, false)]
        // valid
        [TestCase(0.05, true)]
        public void createLocationTest(double _emergency, bool expected)
        {
            locationSetUp();

            IWebElement newLocationButton = _driver.FindElement(By.XPath("//div[@class='d-flex mt-4']/*[3]"));
            
            ((IJavaScriptExecutor)_driver).ExecuteScript(js, newLocationButton);
            System.Threading.Thread.Sleep(1000);

            newLocationButton.Click();
            System.Threading.Thread.Sleep(1000);

            IWebElement collapse = _driver.FindElement(By.XPath("//div[@class='flex-column']/div[last()]/div/img"));
            ((IJavaScriptExecutor)_driver).ExecuteScript(js, collapse);
            System.Threading.Thread.Sleep(1000);

            collapse.Click();
            System.Threading.Thread.Sleep(1000);

            IWebElement name = _driver.FindElement(By.XPath("//div[@class='flex-column']/div[last()]/div[2]//input[@id='name']"));
            name.SendKeys("UITestLocation");

            IWebElement emergency = _driver.FindElement(By.XPath("//div[@class='flex-column']/div[last()]/div[2]//input[@id='emergency']"));
            emergency.Clear();
            emergency.SendKeys(_emergency.ToString());

            IWebElement distributionTime = _driver.FindElement(By.XPath("//div[@class='flex-column']/div[last()]/div[2]//input[@id='distributionTime']"));
            distributionTime.Clear();
            distributionTime.SendKeys("0300");

            IWebElement submitButton = _driver.FindElement(By.XPath("//div[@class='flex-column']/div[last()]/div[2]//button"));
            ((IJavaScriptExecutor)_driver).ExecuteScript(js, submitButton);
            System.Threading.Thread.Sleep(1000);
            submitButton.Click();

            System.Threading.Thread.Sleep(2000);

            if (expected)
            {
                Assert.IsTrue(_driver.FindElement(By.XPath("//div[@class='flex-column']/div[last()]/div/h5")).Text.Equals("UITestLocation"));
                
            } else if (!expected)
            {
                Assert.IsTrue(_driver.FindElement(By.XPath("//div[@class='flex-column']/div[last()]/div/h5")).Text.Equals("Neu"));
                
            }
        }

        [Test]
        // invalid
        [TestCase(-0.05, false)]
        // valid
        [TestCase(0.05, true)]
        public void changeLoation(double _emergency, bool expected)
        {
            createLocationTest(0.05, true);

            IWebElement collapse = _driver.FindElement(By.XPath("//div[@class='flex-column']/div[last()]/div/img"));
            
            ((IJavaScriptExecutor)_driver).ExecuteScript(js, collapse);
            System.Threading.Thread.Sleep(1000);

            collapse.Click();
            System.Threading.Thread.Sleep(1000);

            IWebElement name = _driver.FindElement(By.XPath("//div[@class='flex-column']/div[last()]/div[2]//input[@id='name']"));
            name.SendKeys("ChangedNameLocation");

            IWebElement emergency = _driver.FindElement(By.XPath("//div[@class='flex-column']/div[last()]/div[2]//input[@id='emergency']"));
            emergency.Clear();
            emergency.SendKeys(_emergency.ToString());

            IWebElement distributionTime = _driver.FindElement(By.XPath("//div[@class='flex-column']/div[last()]/div[2]//input[@id='distributionTime']"));
            distributionTime.Clear();
            distributionTime.SendKeys("0400");

            IWebElement submitButton = _driver.FindElement(By.XPath("//div[@class='flex-column']/div[last()]/div[2]//button"));

            ((IJavaScriptExecutor)_driver).ExecuteScript(js, submitButton);
            System.Threading.Thread.Sleep(1000);

            submitButton.Click();

            System.Threading.Thread.Sleep(2000);

            if (expected)
            {
                Assert.IsTrue(_driver.FindElement(By.XPath("//div[@class='flex-column']/div[last()]/div/h5")).Text.Equals("ChangedNameLocation"));
            }
            else if (!expected)
            {
                Assert.IsTrue(_driver.FindElement(By.XPath("//div[@class='flex-column']/div[last()]/div/h5")).Text.Equals("UITestLocation"));

                ((IJavaScriptExecutor)_driver).ExecuteScript(js, collapse);
                System.Threading.Thread.Sleep(1000);

                collapse.Click();
            }
        }

        [Test]
        // invalid css
        [TestCase(-2, 10, 0, 0, 2, 100, 2000, false)]
        // invalid type 2
        [TestCase(2, 10, -2, 0, 2, 100, 2000, false)]
        // invalid cssPower
        [TestCase(2, -10, 0, 0, 2, 100, 2000, false)]
        // invalid type2Power
        [TestCase(2, 10, 3, -5, 2, 100, 2000, false)]
        // invalid maxParallel
        [TestCase(2, 10, 0, 0, -2, 100, 2000, false)]
        // invalid maxPower
        [TestCase(2, 10, 0, 0, 2, -100, 2000, false)]
        // valid
        [TestCase(2, 20, 3, 200, 4, 300, 2000, true)]
        public void createZone(int _ccsPlug, int _ccsPlugPower, int _type2Plug, int _type2PlugPower, int _maxParallelUsabel, int _maxPower, int _zonePower, bool expected)
        {
            createLocationTest(0.05, true);

            IWebElement arrow = _driver.FindElement(By.XPath("//div[@class='flex-column']/div[last()]/div/img[2]"));

            ((IJavaScriptExecutor)_driver).ExecuteScript(js, arrow);
            System.Threading.Thread.Sleep(1000);

            arrow.Click();
            System.Threading.Thread.Sleep(1000);
            
            IWebElement newZButton = _driver.FindElement(By.XPath("//div[@class='flex-column']/div[last()]/div[3]/div/form/button"));

            ((IJavaScriptExecutor)_driver).ExecuteScript(js, newZButton);
            System.Threading.Thread.Sleep(1000);

            newZButton.Click();
            System.Threading.Thread.Sleep(1000);
            
            Assert.IsTrue(_driver.Url.ToString().Contains("https://sopro-ss2020-team17.azurewebsites.net/Infrastructure/CreateZone"));

            IWebElement manufacturer = _driver.FindElement(By.Id("station_manufacturer"));
            manufacturer.Clear();
            manufacturer.SendKeys("TestManu");

            IWebElement ccsPlug = _driver.FindElement(By.Id("ccs"));
            ccsPlug.Clear();
            ccsPlug.SendKeys(_ccsPlug.ToString());

            IWebElement ccsPlugPower = _driver.FindElement(By.Id("ccsPower"));
            ccsPlugPower.Clear();
            ccsPlugPower.SendKeys(_ccsPlugPower.ToString());
            

            IWebElement type2Plug = _driver.FindElement(By.Id("type2"));
            type2Plug.Clear();
            type2Plug.SendKeys(_type2Plug.ToString());

            IWebElement type2PlugPower = _driver.FindElement(By.Id("type2Power"));
            type2PlugPower.Clear();
            type2PlugPower.SendKeys(_type2PlugPower.ToString());

            IWebElement maxUsable = _driver.FindElement(By.Id("station_maxParallelUseable"));
            maxUsable.Clear();
            maxUsable.SendKeys(_maxParallelUsabel.ToString());

            IWebElement maxPower = _driver.FindElement(By.Id("station_maxPower"));
            maxPower.Clear();
            maxPower.SendKeys(_maxPower.ToString());

            _driver.FindElement(By.XPath("//div[2]/div/form/div[6]/button")).Click();

            System.Threading.Thread.Sleep(2000);

            if(expected)
            {
                Assert.IsTrue(_driver.FindElement(By.XPath("//div[2]/div[2]/div/table/tbody/tr[last()]/td/div/div[2]/div[1]")).Text.ToString().Equals("TestManu"));
                _driver.Navigate().GoToUrl("https://sopro-ss2020-team17.azurewebsites.net/Infrastructure");
            } else
            {
                try
                {
                    _driver.FindElement(By.XPath("//div[2]/div[2]/div/table/tbody/tr[last()]/td/div/div[2]/div[1]")).Text.ToString().Equals("TestManu");
                }
                catch
                {
                    Assert.IsTrue(true);
                    _driver.Navigate().GoToUrl("https://sopro-ss2020-team17.azurewebsites.net/Infrastructure");
                }
            }
        }

        [Test]
        // maxPower invalid
        [TestCase(3, 120, 3, 200, 4, -300, false)]
        // maxPara invlaid
        [TestCase(3, 120, 3, 200, -4, 300, false)]
        // type2Power invalid
        [TestCase(3, 120, 3, -200, 4, 300, false)]
        // type2Plug invald
        [TestCase(3, 120, -3, 200, 4, 300, false)]
        // ccsPower invalid
        [TestCase(3, -120, 3, 200, 4, 300, false)]
        // ccsPlugs invalid
        [TestCase(-3, 120, 3, 200, 4, 300, false)]
        // valid
        [TestCase(3, 120, 3, 200, 4, 300, true)]
        public void changeZone(int _ccsPlug, int _ccsPlugPower, int _type2Plug, int _type2PlugPower, int _maxParallelUsabel, int _maxPower, bool expected)
        {
            createZone(2, 200, 3, 120, 2, 2000, 200, true);

            IWebElement button1 = _driver.FindElement(By.XPath("//div[@class='flex-column']/div[last()]/div/img[2]"));

            ((IJavaScriptExecutor)_driver).ExecuteScript(js, button1);
            System.Threading.Thread.Sleep(1000);

            button1.Click();

            System.Threading.Thread.Sleep(1000);



            IWebElement button2 = _driver.FindElement(By.XPath("//div[@class='flex-column']/div[last()]/div[3]/div/div[last()]/div/div/div/form/button"));

            ((IJavaScriptExecutor)_driver).ExecuteScript(js, button2);
            System.Threading.Thread.Sleep(1000);

            button2.Click();

            System.Threading.Thread.Sleep(1000);

            IWebElement button3 = _driver.FindElement(By.XPath("//div[2]/div[2]/div/table/tbody/tr[last()]/td[2]/a/img"));

            ((IJavaScriptExecutor)_driver).ExecuteScript(js, button3);
            System.Threading.Thread.Sleep(1000);

            button3.Click();

            System.Threading.Thread.Sleep(1000);

            IWebElement manufacturer = _driver.FindElement(By.Id("station_manufacturer"));
            manufacturer.Clear();
            manufacturer.SendKeys("TestChangedManu");

            IWebElement ccsPlug = _driver.FindElement(By.Id("ccs"));
            ccsPlug.Clear();
            ccsPlug.SendKeys(_ccsPlug.ToString());

            IWebElement ccsPlugPower = _driver.FindElement(By.Id("ccsPower"));
            ccsPlugPower.Clear();
            ccsPlugPower.SendKeys(_ccsPlugPower.ToString());


            IWebElement type2Plug = _driver.FindElement(By.Id("type2"));
            type2Plug.Clear();
            type2Plug.SendKeys(_type2Plug.ToString());

            IWebElement type2PlugPower = _driver.FindElement(By.Id("type2Power"));
            type2PlugPower.Clear();
            type2PlugPower.SendKeys(_type2PlugPower.ToString());

            IWebElement maxUsable = _driver.FindElement(By.Id("station_maxParallelUseable"));
            maxUsable.Clear();
            maxUsable.SendKeys(_maxParallelUsabel.ToString());

            IWebElement maxPower = _driver.FindElement(By.Id("station_maxPower"));
            maxPower.Clear();
            maxPower.SendKeys(_maxPower.ToString());

            IWebElement submit = _driver.FindElement(By.XPath("//div[2]/div/form/div[6]/button"));

            ((IJavaScriptExecutor)_driver).ExecuteScript(js, submit);
            System.Threading.Thread.Sleep(1000);

            submit.Click();

            System.Threading.Thread.Sleep(2000);

            if (expected)
            {
                Assert.IsTrue(_driver.FindElement(By.XPath("//div[2]/div[2]/div/table/tbody/tr[last()]/td/div/div[2]/div[1]")).Text.ToString().Equals("TestChangedManu"));
                _driver.Navigate().GoToUrl("https://sopro-ss2020-team17.azurewebsites.net/Infrastructure");
                System.Threading.Thread.Sleep(1000);
            }
            else
            {
                try
                {
                    _driver.FindElement(By.XPath("//div[2]/div[2]/div/table/tbody/tr[last()]/td/div/div[2]/div[1]")).Text.ToString().Equals("TestManu");
                }
                catch
                {
                    Assert.IsTrue(true);
                    _driver.Navigate().GoToUrl("https://sopro-ss2020-team17.azurewebsites.net/Infrastructure");
                    System.Threading.Thread.Sleep(1000);
                }
            }

        }

        [Test]
        // Kein Steckertyp ausgewählt
        [TestCase(false, 20, 60, 200, true, true, false)]
        // socStart ist invalide
        [TestCase(true, -20, 60, 200, true, true, false)]
        // socEnd ist invalide
        [TestCase(true, 20, -50, 200, true, true, false)]
        // socStart > socEnd -> invalide
        [TestCase(true, 90, 80, 200, true, true, false)]
        // StartZeit ist invaldie
        [TestCase(true, 20, 60, 200, false, true, false)]
        // EndZeit ist invalide
        [TestCase(true, 20, 60, 200, true, false, false)]
        // Buchung ist valide
        [TestCase(true, 20, 60, 200, true, true, true)]
        public void createBooking(bool plugTypeRight, int socStart, int socEnd, int capacity, bool startTimeRight, bool endTimeRight, bool expected)
        {
            createZone(2, 120, 3, 100, 3, 1200, 2000, true);

            bookingSetUp();

            IWebElement location = _driver.FindElement(By.XPath("//select[@id='locationId']"));
            
            SelectElement selectElement = new SelectElement(location);
            selectElement.SelectByText("UITestLocation");
            
            
            if (!plugTypeRight)
            {
                IWebElement plugType = _driver.FindElement(By.XPath("//input[@id='ccs']"));

                ((IJavaScriptExecutor)_driver).ExecuteScript(js, plugType);
                System.Threading.Thread.Sleep(1000);

                plugType.Click();
            }
            
            IWebElement socS= _driver.FindElement(By.XPath("//input[@id='booking_socStart']"));
            socS.Clear();
            socS.SendKeys(socStart.ToString());
            

            IWebElement socE = _driver.FindElement(By.XPath("//input[@id='booking_socEnd']"));
            socE.Clear();
            socE.SendKeys(socEnd.ToString());

            IWebElement cap = _driver.FindElement(By.XPath("//input[@id='booking_capacity']"));
            cap.Clear();
            cap.SendKeys(capacity.ToString());

            if(startTimeRight && endTimeRight)
            {
                IWebElement startTime = _driver.FindElement(By.XPath("//input[@id='booking_startTime']"));
                string date = DateTime.Now.Add(new TimeSpan(1, 0, 0, 0)).Date.ToString("dd.MM.yyyy");
                startTime.SendKeys(date);
                startTime.SendKeys(Keys.ArrowRight);
                startTime.SendKeys("1030");

                IWebElement endTime = _driver.FindElement(By.XPath("//input[@id='booking_endTime']"));
                string dateEnd = DateTime.Now.Add(new TimeSpan(1, 0, 0, 0)).Date.ToString("dd.MM.yyyy");
                endTime.SendKeys(dateEnd);
                endTime.SendKeys(Keys.ArrowRight);
                endTime.SendKeys("1500");

            } else if (startTimeRight && !endTimeRight)
            {
                IWebElement startTime = _driver.FindElement(By.XPath("//input[@id='booking_startTime']"));
                string date = DateTime.Now.Add(new TimeSpan(1, 0, 0, 0)).Date.ToString("dd.MM.yyyy");
                startTime.SendKeys(date);
                startTime.SendKeys(Keys.ArrowRight);
                startTime.SendKeys("1030");

                IWebElement endTime = _driver.FindElement(By.XPath("//input[@id='booking_endTime']"));
                string dateEnd = DateTime.Now.Add(new TimeSpan(-1, 0, 0, 0)).Date.ToString("dd.MM.yyyy");
                endTime.SendKeys(dateEnd);
                endTime.SendKeys(Keys.ArrowRight);
                endTime.SendKeys("1500");

            } else if (!startTimeRight && endTimeRight)
            {
                IWebElement startTime = _driver.FindElement(By.XPath("//input[@id='booking_startTime']"));
                string date = DateTime.Now.Add(new TimeSpan(-1, 0, 0, 0)).Date.ToString("dd.MM.yyyy");
                startTime.SendKeys(date);
                startTime.SendKeys(Keys.ArrowRight);
                startTime.SendKeys("1030");

                IWebElement endTime = _driver.FindElement(By.XPath("//input[@id='booking_endTime']"));
                string dateEnd = DateTime.Now.Add(new TimeSpan(1, 0, 0, 0)).Date.ToString("dd.MM.yyyy");
                endTime.SendKeys(dateEnd);
                endTime.SendKeys(Keys.ArrowRight);
                endTime.SendKeys("1500");
            }

            IWebElement submit = _driver.FindElement(By.XPath("//button[@type='submit']"));

            ((IJavaScriptExecutor)_driver).ExecuteScript(js, submit);
            System.Threading.Thread.Sleep(1000);

            submit.Click();

            System.Threading.Thread.Sleep(5000);

            if (expected)
            {
                Assert.IsTrue(_driver.Url.ToString().Contains("https://sopro-ss2020-team17.azurewebsites.net/Booking"));
                deleteBooking();
                locationSetUp();
            }
            if (!expected)
            {
                Assert.IsTrue(_driver.Url.ToString().Contains("https://sopro-ss2020-team17.azurewebsites.net/Booking/Create"));
                locationSetUp();
            }

        }

        

        [TearDown]
        public void tearDown()
        {
            deleteLocation();
            settings.kill();
            _driver.Quit();
        }
    }
}
