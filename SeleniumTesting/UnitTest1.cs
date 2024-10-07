using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;

namespace SeleniumNUnitExample
{
    [TestFixture]
    public class GoogleSearchTest1
    {
        private IWebDriver? driver;

        [SetUp]
        public void Setup()
        {
            // Initialize the ChromeDriver
            driver = new ChromeDriver(@"C:\Users\opilane\source\repos\SeleniumTesting\SeleniumTesting\Drivers");
            // Maximize the browser window
            driver.Manage().Window.Maximize();
            // Set an implicit wait time
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
        }

        [Test]
        public void SearchTest()
        {
            // Navigate to Google's homepage
            driver!.Navigate().GoToUrl("https://yaroslavyekasov22.thkit.ee/");

            //// Accept cookies if prompted (optional, depends on region)
            //try
            //{
            //    var acceptCookiesButton = driver.FindElement(By.Id("L2AGLb"));
            //    acceptCookiesButton.Click();
            //}
            //catch (NoSuchElementException)
            //{
            //    // Do nothing if the accept cookies button is not present
            //}

            // Find the search box using its name attribute
            //var searchBox = driver.FindElement(By.Name("ul"));

            //// Enter the search term
            //searchBox.SendKeys("Selenium WebDriver");

            //// Submit the search form
            //searchBox.Submit();

            //// Wait for the results page to load and display the results
            //// It's better to use explicit waits in real tests
            //System.Threading.Thread.Sleep(2000);

            //// Verify that the page title contains the search term
            //Assert.That(driver.Title.Contains("Selenium WebDriver"), "The page title does not contain the search term.");
        }

        [TearDown]
        public void Teardown()
        {
            if (driver != null)
            {
                driver.Quit();     // Close all browser windows and safely end the session
                driver.Dispose();  // Release all resources
                driver = null;     // Set driver to null to aid garbage collection
            }
        }
    }
}