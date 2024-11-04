using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;

namespace SeleniumNUnitExample
{
    [TestFixture]
    public class KlickWebsiteTests
    {
        private IWebDriver? driver;

        [OneTimeSetUp]
        public void Setup()
        {
            driver = new ChromeDriver(@"C:\Users\opilane\source\repos\SeleniumTesting\SeleniumTesting\Drivers");
            driver.Manage().Window.Maximize();
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
            driver.Navigate().GoToUrl("https://www.klick.ee/");
        }

        [Test]
        public void NavigateToLaptopsAndCustomerAccount()
        {
            var laptopsCategory = driver!.FindElement(By.LinkText("Sülearvutid"));
            laptopsCategory.Click();
            System.Threading.Thread.Sleep(1000);
            Assert.That(driver.PageSource.Contains("Sülearvutid"), "Laptops category page not displayed.");

            driver.Navigate().Back();
            System.Threading.Thread.Sleep(1000);

            var kliendikontoLink = driver.FindElement(By.LinkText("Kliendikonto"));
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView(true);", kliendikontoLink);
            kliendikontoLink.Click();
            System.Threading.Thread.Sleep(1000);

            Assert.That(driver.PageSource.Contains("Kliendikonto"), "Customer Account page not displayed.");
        }

        [OneTimeTearDown]
        public void Teardown()
        {
            driver?.Quit();
            driver?.Dispose();
        }
    }
}
