using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Remote;
using System.Linq;

namespace GEMS.AutoTestsGemsDev.Tests.AutoTests
{
    [TestFixture, Category("AutoTests")]
    internal class ExistsLinkTests
    {
        private const string NameButtonToMenu = "Продукты";
        private RemoteWebDriver _m_driver;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            _m_driver = new FirefoxDriver(@"C:\Users\Zver\source\repos\OtherApp\geckodriver-v0.27.0-win64")
            {
                Url = "https://gemsdev.ru/"
            };
            _m_driver.Manage().Window.Maximize();
            var links = _m_driver.FindElements(By.XPath(".//nav[@class='menu']//ul/li/a"))
                                 .Where(l => l.GetAttribute("textContent").Contains(NameButtonToMenu))
                                 .ToList();
            if (links.Count() == 0)
            {
                OneTimeTearDown();
                Assert.Fail($"Не надена кнопка {NameButtonToMenu} в меню.");
            }
            links[0].Click();
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            _m_driver.Close();
        }

        [Test]
        public void OtherNamesSections_SectionsIsExists()
        {
            // assert
            var contentSection = "Государственная система обеспечения градостроительной деятельности";
            var expectedLink = "https://xn--c1aaceme9acfqh.xn--p1ai/";
            var section = _m_driver.FindElements(By.XPath($".//*[self::h1 or self::h2]"))
                                   .First(s => s.GetAttribute("textContent").Contains(contentSection))
                                   .FindElement(By.XPath(".."));

            // act
            var links = section.FindElements(By.XPath($".//a"));
            
            // assert
            Assert.IsTrue(links.Any(s => s.GetAttribute("href").Contains(expectedLink)));
        }
    }
}
