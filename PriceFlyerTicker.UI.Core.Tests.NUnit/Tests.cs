using NUnit.Framework;

using PriceFlyerTicker.UI.Core.Services;

namespace PriceFlyerTicker.UI.Core.Tests.NUnit
{
    // TODO WTS: Add appropriate unit tests.
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Test1()
        {
            Assert.Pass();
        }

        // TODO WTS: Remove or update this once your app is using real data and not the SampleDataService.
        // This test serves only as a demonstration of testing functionality in the Core library.
        [Test]
        public void EnsureSampleDataServiceReturnsContentGridData()
        {
            var dataService = new SampleDataService();
            var actual = dataService.GetContentGridData();

            Assert.AreNotEqual(0, actual.Count);
        }
    }
}
