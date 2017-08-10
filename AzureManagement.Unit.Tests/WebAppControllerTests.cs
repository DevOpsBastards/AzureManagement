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
    public class WebAppControllerTests
    {
        [TestMethod()]
        public void WebAppControllerTest()
        {
            ILogger log = new LoggingService();
            var webAppCon = new WebAppController(log);
            Assert.IsNotNull(webAppCon);
        }

        [TestMethod()]
        public void CreateWebAppProcessTest()
        {
            //Integration test
        }

        [TestMethod()]
        public void CreateDeploymentSlotTest()
        {
            //Integration test
        }

        [TestMethod()]
        public void GetAllWebAppsForAllResGrpsAsStringTest()
        {
            //Integration test
        }

        [TestMethod()]
        public void GetWebAppsFromAzureTest()
        {
            //Integration test
        }

        [TestMethod()]
        public void GetWebAppsForResourceGroupFromAzureTest()
        {
            //Integration test;
        }

        [TestMethod()]
        public void CreateAutoScalePolicyForAppTest()
        {
            //TODO: ensure autoscalepolicy is generated correctly
        }

        [TestMethod()]
        public void CreateAlertRulesForAppTest()
        {
            //TODO: ensure Alert is generated correctly
        }
    }
}