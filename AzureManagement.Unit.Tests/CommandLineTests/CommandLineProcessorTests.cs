using System;
using System.Collections.Generic;
using AzureManagement.CommandLine;
using AzureManagement.Logging.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AzureManagement.Unit.Tests.CommandLineTests
{
    [TestClass()]
    public class CommandLineProcessorTests
    {
        private readonly MockLogger _logger = new MockLogger();

        [TestMethod]
        public void CommandLineProcessor_nullArgs_DisplayUsage()
        {
            // Arrange

            var p = new CommandLineProcessor(null, _logger);

            // Act   
            var results = p.Options.GetUsage();

            // Assert  
            Assert.IsNotNull(results, "Command Line null error");

        }



        [TestMethod]
        public void CommandLineProcessor_IsDebugTrueWhenCommandLineIsSet_ActionLoadedintoProperty()
        {
            // Arrange
            var args = new string[] { "-d" };

            // Act   
            var p = new CommandLineProcessor(args, _logger);

            var results = p.Options.IsDebug;
            // Assert  
            Assert.AreEqual(true, results);
            //Assert.AreEqual(results, "listsites");
        }



        //[TestMethod]
        //public void CommandLineProcessor_IsThereASiteWhenCommandLineIsSet_SiteLoadedintoProperty()
        //{
        //    // Arrange
        //    string[] args = new string[] { "-s testsite" };

        //    // Act   
        //    CommandLineProcessor p = new CommandLineProcessor(args, _logger);

        //    var results = p.Options.SiteName;
        //    // Assert  
        //    Assert.AreEqual(results.Length > 0, true);
        //    Assert.AreEqual(results, "testsite");

        //}



    }

    public class MockLogger : ILogger
    {
        public string ValuefromMessage { get; set; }

        public string Name { get; }
        public void Info(string message)
        {
            ValuefromMessage = message;
        }

        public void Error(string message)
        {
            ValuefromMessage = message;
        }

        public void Error(string message, Exception ex)
        {
            ValuefromMessage = message;
        }
    }

}