using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using System.Linq;

namespace SeleniumShopUITestTARge24
{
    [TestClass]
    public class RealEstateTests : TestBase
    {
        private void GoToRealEstateIndex()
        {
            Driver.Navigate().GoToUrl($"{BaseUrl}/RealEstate");
        }

        [TestMethod]
        public void Can_Navigate_To_RealEstate_Index()
        {
            GoToRealEstateIndex();

            
            Assert.IsTrue(Driver.Url.Contains("/RealEstate"));

            
            var createLink = Driver.FindElement(By.LinkText("Create"));
            Assert.IsNotNull(createLink);
        }


        [TestMethod]
        public void Can_Create_RealEstate_With_Valid_Data()
        {
            GoToRealEstateIndex();
            Driver.FindElement(By.LinkText("Create")).Click();

            Driver.FindElement(By.Id("Area")).SendKeys("55");
            Driver.FindElement(By.Id("Location")).SendKeys("Tallinn");
            Driver.FindElement(By.Id("RoomNumber")).SendKeys("3");
            Driver.FindElement(By.Id("BuildingType")).SendKeys("Korter");

            Driver.FindElement(By.CssSelector("input[type='submit'][value='Create']")).Click();

            Assert.IsTrue(Driver.Url.Contains("/RealEstate"));

            var rows = Driver.FindElements(By.CssSelector("table tbody tr"));
            Assert.IsTrue(rows.Count > 0);
        }

        [TestMethod]
        public void Cannot_Create_RealEstate_With_Invalid_Data()
        {
            GoToRealEstateIndex();

            
            var rowsBefore = Driver.FindElements(By.CssSelector("table tbody tr")).Count;

            Driver.FindElement(By.LinkText("Create")).Click();

            
            Driver.FindElement(By.Id("Area")).SendKeys("abc");
            Driver.FindElement(By.Id("Location")).SendKeys("Tallinn");
            Driver.FindElement(By.Id("RoomNumber")).SendKeys("3");
            Driver.FindElement(By.Id("BuildingType")).SendKeys("Korter");

            Driver.FindElement(By.CssSelector("input[type='submit'][value='Create']")).Click();

            
            GoToRealEstateIndex();
            var rowsAfter = Driver.FindElements(By.CssSelector("table tbody tr")).Count;

            
            Assert.AreEqual(rowsBefore, rowsAfter);
        }


        [TestMethod]
        public void Can_View_RealEstate_Details()
        {
            GoToRealEstateIndex();

            var rows = Driver.FindElements(By.CssSelector("table tbody tr"));
            if (rows.Count == 0)
            {
                Can_Create_RealEstate_With_Valid_Data();
                GoToRealEstateIndex();
            }

            Driver.FindElement(By.LinkText("Details")).Click();

            var title = Driver.FindElement(By.TagName("h1")).Text;
            Assert.IsTrue(title.Contains("Details"));
        }

        [TestMethod]
        public void Can_Edit_RealEstate_With_Valid_Data()
        {
            GoToRealEstateIndex();

            var rows = Driver.FindElements(By.CssSelector("table tbody tr"));
            if (rows.Count == 0)
            {
                Can_Create_RealEstate_With_Valid_Data();
                GoToRealEstateIndex();
            }

            Driver.FindElement(By.LinkText("Update")).Click();

            var areaInput = Driver.FindElement(By.Id("Area"));
            areaInput.Clear();
            areaInput.SendKeys("99");

            Driver.FindElement(By.CssSelector("input[type='submit']")).Click();

            GoToRealEstateIndex();
            var cellTexts = Driver.FindElements(By.CssSelector("table tbody tr td")).Select(x => x.Text);
            Assert.IsTrue(cellTexts.Contains("99"));
        }

        [TestMethod]
public void Cannot_Edit_RealEstate_With_Invalid_Data()
{
    GoToRealEstateIndex();

    
    var rows = Driver.FindElements(By.CssSelector("table tbody tr"));
    if (rows.Count == 0)
    {
        Can_Create_RealEstate_With_Valid_Data();
        GoToRealEstateIndex();
    }

    
    var oldArea = Driver.FindElement(By.CssSelector("table tbody tr td:nth-child(3)")).Text;

    Driver.FindElement(By.LinkText("Update")).Click();

    var areaInput = Driver.FindElement(By.Id("Area"));
    areaInput.Clear();
    areaInput.SendKeys("abc"); 

    Driver.FindElement(By.CssSelector("input[type='submit']")).Click();

    
    GoToRealEstateIndex();
    var newArea = Driver.FindElement(By.CssSelector("table tbody tr td:nth-child(3)")).Text;

    Assert.AreEqual(oldArea, newArea);
}


        [TestMethod]
        public void Can_Delete_RealEstate()
        {
            GoToRealEstateIndex();

            var rowsBefore = Driver.FindElements(By.CssSelector("table tbody tr")).Count;
            if (rowsBefore == 0)
            {
                Can_Create_RealEstate_With_Valid_Data();
                GoToRealEstateIndex();
                rowsBefore = Driver.FindElements(By.CssSelector("table tbody tr")).Count;
            }

            Driver.FindElement(By.LinkText("Delete")).Click();
            Driver.FindElement(By.CssSelector("input[type='submit'][value='Delete']")).Click();

            var rowsAfter = Driver.FindElements(By.CssSelector("table tbody tr")).Count;
            Assert.IsTrue(rowsAfter < rowsBefore);
        }
    }
}
