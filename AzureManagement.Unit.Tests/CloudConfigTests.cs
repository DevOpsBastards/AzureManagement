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
    public class CloudConfigTests
    {
        [TestMethod()]
        public void CloudConfigTest()
        {
            var config = new CloudAppConfig();
            Assert.IsNotNull(config, "Config is null");
        }

        [TestMethod()]
        public void GetCredentialsTest()
        {
            //Integration test
        }

        [TestMethod()]
        public void LoadConfigFromEnvironmentVariablesTest()
        {
            //Integration test
        }
    }
}