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
    public class EnvironmentTypeTests
    {
        [TestMethod()]
        public void EnvironmentTypeTest()
        {
            var EnvType = new EnvironmentType();
            Assert.IsNotNull(EnvType);
        }
    }
}