using AzureManagement.Logging.Interfaces;
using AzureManagement.Logging.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AzureManagement.Unit.Tests.LoggingTests
{
    [TestClass]
    public class LoggingServiceTests
    {
        private readonly ILogger _logger = new LoggingService();

        [TestMethod]
        public void LoggingServiceWhenInitializedIsNotNull()
        {
            Assert.IsNotNull(_logger, "LoggingService is null");
        }

        [TestMethod]
        public void LoggerName_CallingAssemblyName()
        {
            // Arrange
            var service = new LoggingServiceSpy();

            // Act
            service.Info("Test");

            // Assert 
            Assert.AreEqual("AzureManagement.Unit.Tests", service.LastLoggerNameUsed);
        }

        [TestMethod]
        public void LoggerName_CallingAssemblyName_WithError()
        {
            // Arrange
            var service = new LoggingServiceSpy();
            var testString = "test";

            // Act
            service.Error(testString);

            // Assert 
            Assert.AreEqual("AzureManagement.Unit.Tests", service.LastLoggerNameUsed);
            Assert.AreEqual(testString, service.LastMessage, "Message is not the same");
        }

        [TestMethod]
        public void LoggerName_CallingAssemblyNameWithStaticMethod()
        {
            Assert.AreEqual("AzureManagement.Unit.Tests", CallingAssemblyNameWithStaticMethod());
        }

        private static string CallingAssemblyNameWithStaticMethod()
        {
            // Arrange
            var service = new LoggingServiceSpy();

            // Act
            service.Info("messageouput");
            return service.LastLoggerNameUsed;

        }

        private class LoggingServiceSpy : LoggingService
        {
            public string LastLoggerNameUsed { get; private set; }
            public string LastMessage { get; set; }

            public override void Info(string message)
            {
                LastLoggerNameUsed = Name;
                LastMessage = message;
                //base.Info(message);
            }

            public override void Error(string message)
            {
                LastLoggerNameUsed = Name;
                LastMessage = message;
                base.Error(message);
            }
        }
    }
}