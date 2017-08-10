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
    public class EnumerationTests
    {
        [TestMethod()]
        public void GetAllTest()
        {
            var EnvironmentTypeFromInput = Enumeration.FromDisplayName<EnvironmentType>("Test");
            Assert.AreEqual(EnvironmentType.Test, EnvironmentTypeFromInput);

        }


    }
}