using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Linq;
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
        public void AutomateBuyingTenProducts()
        {
            driver.Navigate().GoToUrl("https://www.klick.ee/");
            Thread.Sleep(1000);

            // cookies
            var allowAllButton = driver.FindElement(By.Id("CybotCookiebotDialogBodyLevelButtonLevelOptinAllowAll"));
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", allowAllButton);
            Thread.Sleep(1000);

            // to Telefonid ja Lisad
            var telefonidLink = driver.FindElement(By.LinkText("Telefonid"));
            telefonidLink.Click();
            Thread.Sleep(2000);

            // collect phones
            var productElements = driver.FindElements(By.CssSelector(".product-link")).ToList();
            if (productElements.Count == 0)
            {
                Assert.Fail("No products found on the page to add to the cart.");
            }

            Random random = new Random();
            HashSet<int> selectedIndexes = new HashSet<int>();


            // add 3 random phones
            while (selectedIndexes.Count < 3 && selectedIndexes.Count < productElements.Count)
            {
                int index = random.Next(productElements.Count);
                if (!selectedIndexes.Contains(index))
                {
                    selectedIndexes.Add(index);

                    var productLink = productElements[index];
                    ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView(true);", productLink);
                    Thread.Sleep(1000);

                    ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", productLink);
                    Thread.Sleep(1000);

                    try
                    {
                        var buyButton = driver.FindElement(By.XPath("//button[contains(@class, 'add-to-cart') and contains(text(), 'Osta')]"));
                        ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", buyButton);
                        Thread.Sleep(1000);
                    }
                    catch (NoSuchElementException)
                    {
                        Assert.Fail("The 'Add to Cart' button could not be found on the product page.");
                    }

                    driver.Navigate().Back();
                    Thread.Sleep(1000);

                    productElements = driver.FindElements(By.CssSelector(".product-link")).ToList();
                }
            }

            // cart
            try
            {
                ((IJavaScriptExecutor)driver).ExecuteScript("window.scrollTo(5, 5);");

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
                emailInput.SendKeys("Testro@gmail.com");

                var nextButton = driver.FindElement(By.Id("Kliendi_info"));
                ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", nextButton);
                Thread.Sleep(1000);
            }
            catch (NoSuchElementException)
            {
                Assert.Fail("Could not find an element in the checkout form.");
            }

            // dropdown 
            var dropdown = driver.FindElement(By.XPath("//select[@name='faststorepickup']"));
            dropdown.Click();
            Thread.Sleep(1000);

            // select option 
            var option = driver.FindElement(By.XPath("//option[contains(text(), 'Jõhvi - Pargi Keskus')]"));
            option.Click();
            Thread.Sleep(1000);

            var phoneInput = driver.FindElement(By.XPath("//input[@name='phone-number']"));
            phoneInput.SendKeys("54574479");

            var proceedButton = driver.FindElement(By.XPath("//button[@data-testid='shippingSubmit']"));
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", proceedButton);
            Thread.Sleep(1000);
        }

        [OneTimeTearDown]
        public void Teardown()
        {
            driver?.Quit();
            driver?.Dispose();
        }
    }
}
