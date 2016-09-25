using EyeTracking.Test.Objects;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EyeTracking.Test.Tests
{
    [TestFixture]
    public class CieraConfigurationTests : BaseTestFixture
    {

        [TestCase]
        public void StandardTests()
        {
            // Arrange
            var test = TestCriteria.FromJsonFile(@"..\..\TestCriteriaResources\CieraStandardConfigurationTestCriteria.json");

            // Act
            var results = base.RunTest(test);

            // Assert (Print)
            base.PrintResults(test, results);
        }

    }
}
