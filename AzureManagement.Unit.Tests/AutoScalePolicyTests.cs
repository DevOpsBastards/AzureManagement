using Microsoft.VisualStudio.TestTools.UnitTesting;
using AzureManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AzureManagement.Logging.Interfaces;
using AzureManagement.Logging.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting.Logging;

namespace AzureManagement.Unit.Tests
{
    [TestClass()]
    public class AutoScalePolicyTests
    {
        [TestMethod()]
        public void AutoScalePolicyTest()
        {
            ILogger log = new LoggingService();
            var asp = new AutoScalePolicy(log);
            Assert.IsNotNull(asp);
        }

        [TestMethod()]
        public void CreateAutoscalePoliciesTest()
        {
            //Integration test
        }
    }
}