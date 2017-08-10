using Microsoft.VisualStudio.TestTools.UnitTesting;
using AzureManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AzureManagement.Logging.Interfaces;
using AzureManagement.Logging.Services;

namespace AzureManagement.Unit.Tests
{
    [TestClass()]
    public class ResourceGroupControllerTests
    {
        [TestMethod()]
        public void ResourceGroupControllerTest()
        {
            ILogger logger = new LoggingService();
            var rsgController = new ResourceGroupController(logger);
            Assert.IsNotNull(rsgController);

        }

        [TestMethod()]
        public void CreateResourceGroupTest()
        {
            //Integration test
        }

        [TestMethod()]
        public void ListAllResourGroupsTest()
        {
            //Integration test
        }

        [TestMethod()]
        public void CreateApplicationInsightsTest()
        {
            //Integration test
        }

        [TestMethod()]
        public void CreateAppServicePlanTest()
        {
            //Integration test
        }
    }
}