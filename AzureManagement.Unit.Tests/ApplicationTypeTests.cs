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
    public class ApplicationTypeTests
    {
        [TestMethod()]
        public void ApplicationTypeTest()
        {
            var applicationType = new ApplicationType();
            Assert.IsNotNull(applicationType, "AppplicationType is null");
        }
    }
}