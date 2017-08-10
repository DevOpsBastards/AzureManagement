using AzureManagement.CommandLine;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AzureManagement.Unit.Tests.CommandLineTests
{
    [TestClass()]
    public class CommandArgumentOptionsTests
    {
        [TestMethod()]
        public void CommandArgumentOptions_GetUsageTest_IsNotNull()
        {
            // Arrange
            //string expectedres = "";
            var cao = new CommandArgumentOptions();
            // Act          
            var results = cao.GetUsage();
            // Assert  
            Assert.IsNotNull(results);
        }

        [TestMethod()]
        public void CommandArgumentOptions_WhenNoArguementsGetUsageTest_hasData()
        {
            // Arrange
            var cao = new CommandArgumentOptions();
            // Act          
            var results = cao.GetUsage();
            // Assert  
            Assert.AreEqual(results.Length > 0, true);
        }

        [TestMethod()]
        public void CommandArgumentOptions_WhenNoArgumentsGetUsage_hasVersion()
        {
            // Arrange
            var cao = new CommandArgumentOptions();
            // Act          
            var results = cao.GetUsage();
            // Assert  
            Assert.AreEqual(results.Contains("Version"), true);
        }

        //TODO: all parameters of options should check for nul, min,max length of each param

    }
}