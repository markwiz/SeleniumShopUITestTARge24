using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using System.Linq;

namespace SeleniumShopUITestTARge24
{
    [TestClass]
    public class SpaceshipTests : TestBase
    {
        private void GoToSpaceshipsIndex()
        {
           
            Driver.Navigate().GoToUrl($"{BaseUrl}/Spaceships");
        }

        [TestMethod]
        public void Can_Navigate_To_Spaceships_Index()
        {
            GoToSpaceshipsIndex();

            
            Assert.IsTrue(Driver.Url.Contains("/Spaceships"));

            
            var createLink = Driver.FindElement(By.LinkText("Create"));
            Assert.IsNotNull(createLink);
        }


        [TestMethod]
        public void Can_Create_Spaceship_With_Valid_Data()
        {
            GoToSpaceshipsIndex();
            Driver.FindElement(By.LinkText("Create")).Click();

            Driver.FindElement(By.Id("Name")).SendKeys("Enterprise");
            Driver.FindElement(By.Id("Classification")).SendKeys("Explorer");

            SetDateTimeLocal("BuiltDate", DateTime.Now.AddDays(1));

            Driver.FindElement(By.Id("Crew")).SendKeys("5");
            Driver.FindElement(By.Id("EnginePower")).SendKeys("9000");

            Driver.FindElement(By.CssSelector("input[type='submit'][value='Create']")).Click();

            Assert.IsTrue(Driver.Url.Contains("/Spaceships"));

            var texts = Driver.FindElements(By.CssSelector("table tbody tr td")).Select(x => x.Text);
            Assert.IsTrue(texts.Contains("Enterprise"));
        }

        [TestMethod]
        public void Cannot_Create_Spaceship_With_Invalid_Data()
        {
            GoToSpaceshipsIndex();

            var rowsBefore = Driver.FindElements(By.CssSelector("table tbody tr")).Count;

            Driver.FindElement(By.LinkText("Create")).Click();

            
            Driver.FindElement(By.Id("Name")).SendKeys("BadShip");
            Driver.FindElement(By.Id("Classification")).SendKeys("Explorer");
            SetDateTimeLocal("BuiltDate", DateTime.Now.AddDays(1));
            Driver.FindElement(By.Id("Crew")).SendKeys("viis");
            Driver.FindElement(By.Id("EnginePower")).SendKeys("9000");

            Driver.FindElement(By.CssSelector("input[type='submit'][value='Create']")).Click();

            GoToSpaceshipsIndex();
            var rowsAfter = Driver.FindElements(By.CssSelector("table tbody tr")).Count;

            Assert.AreEqual(rowsBefore, rowsAfter);
        }


        [TestMethod]
        public void Can_View_Spaceship_Details()
        {
            GoToSpaceshipsIndex();

            var rows = Driver.FindElements(By.CssSelector("table tbody tr"));
            if (rows.Count == 0)
            {
                Can_Create_Spaceship_With_Valid_Data();
                GoToSpaceshipsIndex();
            }

            Driver.FindElement(By.LinkText("Details")).Click();

            var title = Driver.FindElement(By.TagName("h1")).Text;
            Assert.IsTrue(title.Contains("Details"));
        }

        [TestMethod]
        public void Can_Edit_Spaceship_With_Valid_Data()
        {
            GoToSpaceshipsIndex();

            
            var rows = Driver.FindElements(By.CssSelector("table tbody tr"));
            if (rows.Count == 0)
            {
                Can_Create_Spaceship_With_Valid_Data();
                GoToSpaceshipsIndex();
            }

            
            var updateLink = Driver.FindElement(By.LinkText("Update"));
            var href = updateLink.GetAttribute("href");

            
            Driver.Navigate().GoToUrl(href);

            
            var nameInput = Wait.Until(d => d.FindElement(By.Id("Name")));
            nameInput.Clear();
            nameInput.SendKeys("UpdatedName");

            
            Driver.FindElement(By.CssSelector("input[type='submit']")).Click();

            
            GoToSpaceshipsIndex();
            var texts = Driver.FindElements(By.CssSelector("table tbody tr td"))
                              .Select(x => x.Text);
            Assert.IsTrue(texts.Contains("UpdatedName"));
        }


        [TestMethod]
        public void Cannot_Edit_Spaceship_With_Invalid_Data()
        {
            GoToSpaceshipsIndex();

            var rows = Driver.FindElements(By.CssSelector("table tbody tr"));
            if (rows.Count == 0)
            {
                Can_Create_Spaceship_With_Valid_Data();
                GoToSpaceshipsIndex();
            }

            
            var oldCrew = Driver.FindElement(By.CssSelector("table tbody tr td:nth-child(4)")).Text;

            Driver.FindElement(By.LinkText("Update")).Click();

            var crewInput = Driver.FindElement(By.Id("Crew"));
            crewInput.Clear();
            crewInput.SendKeys("viis"); 

            Driver.FindElement(By.CssSelector("input[type='submit']")).Click();

            GoToSpaceshipsIndex();
            var newCrew = Driver.FindElement(By.CssSelector("table tbody tr td:nth-child(4)")).Text;

            
            Assert.AreEqual(oldCrew, newCrew);
        }



        [TestMethod]
        public void Can_Delete_Spaceship()
        {
            GoToSpaceshipsIndex();

            var rowsBefore = Driver.FindElements(By.CssSelector("table tbody tr")).Count;
            if (rowsBefore == 0)
            {
                Can_Create_Spaceship_With_Valid_Data();
                GoToSpaceshipsIndex();
                rowsBefore = Driver.FindElements(By.CssSelector("table tbody tr")).Count;
            }

            Driver.FindElement(By.LinkText("Delete")).Click();
            Driver.FindElement(By.CssSelector("input[type='submit'][value='Delete']")).Click();

            var rowsAfter = Driver.FindElements(By.CssSelector("table tbody tr")).Count;
            Assert.IsTrue(rowsAfter < rowsBefore);
        }
    }
}
