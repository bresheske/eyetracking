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
    public class TestSize1280x720 : BaseTestFixture
    {

        [TestCase]
        public void LowLight_LowQuality()
        {
            // Arrange
            var test = TestCriteria.FromJsonFile(@"..\..\TestCriteriaResources\TestSize1280x720\LowLight_LowQuality.json");

            // Act
            var results = base.RunTest(test);

            // Assert (Print)
            base.PrintResults(test, results);
        }

        [TestCase]
        public void LowLight_MidQuality()
        {
            // Arrange
            var test = TestCriteria.FromJsonFile(@"..\..\TestCriteriaResources\TestSize1280x720\LowLight_MidQuality.json");

            // Act
            var results = base.RunTest(test);

            // Assert (Print)
            base.PrintResults(test, results);
        }

        [TestCase]
        public void LowLight_HighQuality()
        {
            // Arrange
            var test = TestCriteria.FromJsonFile(@"..\..\TestCriteriaResources\TestSize1280x720\LowLight_HighQuality.json");

            // Act
            var results = base.RunTest(test);

            // Assert (Print)
            base.PrintResults(test, results);
        }

        [TestCase]
        public void UnevenLight_HighQuality()
        {
            // Arrange
            var test = TestCriteria.FromJsonFile(@"..\..\TestCriteriaResources\TestSize1280x720\UnevenLight_HighQuality.json");

            // Act
            var results = base.RunTest(test);

            // Assert (Print)
            base.PrintResults(test, results);
        }

        [TestCase]
        public void UnevenLight_MidQuality()
        {
            // Arrange
            var test = TestCriteria.FromJsonFile(@"..\..\TestCriteriaResources\TestSize1280x720\UnevenLight_MidQuality.json");

            // Act
            var results = base.RunTest(test);

            // Assert (Print)
            base.PrintResults(test, results);
        }

        [TestCase]
        public void UnevenLight_LowQuality()
        {
            // Arrange
            var test = TestCriteria.FromJsonFile(@"..\..\TestCriteriaResources\TestSize1280x720\UnevenLight_LowQuality.json");

            // Act
            var results = base.RunTest(test);

            // Assert (Print)
            base.PrintResults(test, results);
        }

        [TestCase]
        public void HighLight_LowQuality()
        {
            // Arrange
            var test = TestCriteria.FromJsonFile(@"..\..\TestCriteriaResources\TestSize1280x720\HighLight_LowQuality.json");

            // Act
            var results = base.RunTest(test);

            // Assert (Print)
            base.PrintResults(test, results);
        }

        [TestCase]
        public void HighLight_MidQuality()
        {
            // Arrange
            var test = TestCriteria.FromJsonFile(@"..\..\TestCriteriaResources\TestSize1280x720\HighLight_MidQuality.json");

            // Act
            var results = base.RunTest(test);

            // Assert (Print)
            base.PrintResults(test, results);
        }

        [TestCase]
        public void HighLight_HighQuality()
        {
            // Arrange
            var test = TestCriteria.FromJsonFile(@"..\..\TestCriteriaResources\TestSize1280x720\HighLight_HighQuality.json");

            // Act
            var results = base.RunTest(test);

            // Assert (Print)
            base.PrintResults(test, results);
        }
    }
}
