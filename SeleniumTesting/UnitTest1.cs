using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Threading;

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
        }

        [Test]
        public void AddMultipleProductsToCartAndOpenCart()
        {
            driver.Navigate().GoToUrl("https://www.klick.ee/");

            Thread.Sleep(1000);

            var allowAllButton = driver.FindElement(By.Id("CybotCookiebotDialogBodyLevelButtonLevelOptinAllowAll"));
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", allowAllButton);

            Thread.Sleep(1000);

            var productUrls = new List<string>
            {
                "https://www.klick.ee/nutitelefon-samsung-galaxy-m15-5g-4-128gb",
                "https://www.klick.ee/sulearvuti-dell-inspiron-3520-15-120hz-i5-16gb-1000gb-hobe",
                "https://www.klick.ee/nutitelefon-honor-x6b-4-128gb",
                "https://www.klick.ee/sulearvuti-lenovo-thinkpad-l13-gen-3-i5-16gb-256gb-must-1",
                "https://www.klick.ee/nutitelefon-samsung-galaxy-a34-5g-6-128gb-must-enterprise-edition"
            };

            foreach (var url in productUrls)
            {
                driver.Navigate().GoToUrl(url);
                Thread.Sleep(200);

                try
                {
                    var buyButton = driver.FindElement(By.XPath("//button[contains(@class, 'add-to-cart') and contains(text(), 'Osta')]"));
                    ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", buyButton);
                    Thread.Sleep(1000);
                }
                catch (NoSuchElementException)
                {
                    Assert.Fail($"The 'Osta' button could not be found or clicked for product: {url}");
                }

                driver.Navigate().GoToUrl("https://www.klick.ee/");

                Thread.Sleep(200);
            }

            try
            {
                var cartButton = driver.FindElement(By.CssSelector("button.relative.bg-cl-transparent.brdr-none.inline-flex.p10.icon.pointer"));
                ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", cartButton);
                Thread.Sleep(1000);

                Assert.That(driver.PageSource.Contains("Ostukorv"), "Cart page not displayed.");
            }
            catch (NoSuchElementException)
            {
                Assert.Fail("The 'Ostukorv' button could not be found or clicked.");
            }

            try
            {
                var checkoutButton = driver.FindElement(By.CssSelector("a[data-testid='subscribeSubmit']"));
                ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", checkoutButton);
                Thread.Sleep(1000);

                Assert.That(driver.Url.Contains("/checkout"), "Checkout page not displayed.");
            }
            catch (NoSuchElementException)
            {
                Assert.Fail("The 'Suundu kassasse' button could not be found or clicked.");
            }

            try
            {
                var firstNameInput = driver.FindElement(By.Name("first-name"));
                firstNameInput.SendKeys("Testo");

                var lastNameInput = driver.FindElement(By.Name("last-name"));
                lastNameInput.SendKeys("Testorro");

                var emailInput = driver.FindElement(By.Name("email-address"));
                emailInput.SendKeys("Tesro@gmail.com");

                var nextButton = driver.FindElement(By.Id("Kliendi_info"));
                ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", nextButton);
                Thread.Sleep(5000);
            }
            catch (NoSuchElementException)
            {
                Assert.Fail("Could not find an element in the checkout form.");
            }

            // Click on the dropdown to open it
            var dropdown = driver.FindElement(By.XPath("//select[@name='faststorepickup']"));
            dropdown.Click();
            Thread.Sleep(5000); 

            // Select the desired option by visible text
            var option = driver.FindElement(By.XPath("//option[contains(text(), 'Jõhvi - Pargi Keskus')]"));
            option.Click();
            Thread.Sleep(5000);

            var phoneInput = driver.FindElement(By.XPath("//input[@name='phone-number']"));
            phoneInput.SendKeys("54574479");

            var proceedButton = driver.FindElement(By.XPath("//button[@data-testid='shippingSubmit']"));
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", proceedButton);
            Thread.Sleep(10000);
        }

        [OneTimeTearDown]
        public void Teardown()
        {
            driver?.Quit();
            driver?.Quit();
            driver?.Dispose();
        }
    }
}
