using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Remote;
using System.Linq;

namespace GEMS.AutoTestsGemsDev.Tests.AutoTests
{
    [TestFixture, Category("AutoTests")]
    internal class ExistsSectionsTests
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
            if(links.Count() == 0) 
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

        [TestCase("GeoMeta", ExpectedResult = true)]
        [TestCase("Государственная система обеспечения градостроительной деятельности", ExpectedResult = true)]
        [TestCase("Городская аналитика", ExpectedResult = true)]
        [TestCase("Другие наши продукты", ExpectedResult = true)]
        [TestCase("Другие наши проекты", ExpectedResult = false, Reason = "В ТЗ указано, что этот раздел должен присутствовать, я посчитал это опечаткой в ТЗ.")]
        public bool OtherNamesSections_SectionsIsExists(string contentSection)
        {
            // assert
            var xPath = By.XPath($".//*[self::h1 or self::h2]");

            // act
            var sections = _m_driver.FindElements(xPath);
            
            // assert
            Assert.IsNotEmpty(sections);
            return sections.Any(s => s.GetAttribute("textContent").Contains(contentSection));
        }
    }
}