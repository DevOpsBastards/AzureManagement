using Microsoft.VisualStudio.TestTools.UnitTesting;
using AzureManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzureManagement.Unit.Tests
{
    [TestClass()]
    public class WebAppTests
    {
        [TestMethod()]
        public void ToString_Formats_Correctly()
        {
            // Arrange
            var webapp = new WebApp
            {
                Name = "TEST"
            };
            webapp.Urls.Add("https://www.site1.com");
            webapp.Urls.Add("https://www.site2.com");

            var expectedReturnVar = $"TEST|https://www.site1.com,https://www.site2.com";

            // Act          
            var actualReturnVal = webapp.ToString();

            // Assert 
            Assert.AreEqual(expectedReturnVar, actualReturnVal, "ToString didnt return proper format.");



        }
    }
}