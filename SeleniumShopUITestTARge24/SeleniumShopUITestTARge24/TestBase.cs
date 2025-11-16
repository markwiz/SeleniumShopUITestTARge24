using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System;

namespace SeleniumShopUITestTARge24
{
    public class TestBase
    {
        protected IWebDriver Driver;
        protected WebDriverWait Wait;

        protected string BaseUrl = "https://localhost:7282"; 

        [TestInitialize]
        public void Setup()
        {
            var options = new ChromeOptions();
            

            Driver = new ChromeDriver(options);
            Driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);
            Wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(10));
        }

        [TestCleanup]
        public void Teardown()
        {
            Driver.Quit();
        }

        protected void GoHome()
        {
            Driver.Navigate().GoToUrl(BaseUrl);
        }

        
        protected void SetDateTimeLocal(string elementId, DateTime dt)
        {
            var formatted = dt.ToString("yyyy-MM-ddTHH:mm");
            var element = Driver.FindElement(By.Id(elementId));

            IJavaScriptExecutor js = (IJavaScriptExecutor)Driver;
            js.ExecuteScript("arguments[0].value = arguments[1]", element, formatted);
        }
    }
}
