using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Text;

namespace UI_Tests.Booking
{
    class BrowserSettings
    {
        private IWebDriver _webDriver { get; set; }
        private string _email { get; set; }
        private const string urlLogin = "https://localhost:44383/";

        private IWebElement logIn => _webDriver.FindElement(By.Name("email"));

        public void initBrowser(IWebDriver webDriver, string email)
        {
            _webDriver = webDriver;
            _email = email;
            NavigateTo(urlLogin);
            webDriver.Manage().Window.Maximize();
            System.Threading.Thread.Sleep(2000);
            LogIn();
        }

        private void LogIn()
        {
            logIn.SendKeys(_email);
            IWebElement submit = _webDriver.FindElement(By.XPath("//button[@type='submit']"));
            submit.Click();
            System.Threading.Thread.Sleep(2000);
        }

        public void NavigateTo(string url)
        {
            _webDriver.Navigate().GoToUrl(url);
        }

        public void kill()
        {
            _webDriver.Quit();
        }

    }
}
