using IamApi.Extensions;
using Xunit;

namespace IamApiTests.ExtensionsTests
{
    public class StringExtensionsTestscs
    {
        [Fact]
        public void IsNullOrWhiteSpace_Null()
        {
            string sampleString = null;

            Assert.True(sampleString.IsNullOrWhiteSpace());
        }

        [Fact]
        public void IsNullOrWhiteSpace_WhiteSpace()
        {
            var sampleString = "   ";

            Assert.True(sampleString.IsNullOrWhiteSpace());
        }

        [Fact]
        public void IsNullOrWhiteSpace_Empty()
        {
            var sampleString = "";

            Assert.True(sampleString.IsNullOrWhiteSpace());
        }

        [Fact]
        public void IsNullOrWhiteSpace_HasValue()
        {
            var sampleString = "Sample String";

            Assert.False(sampleString.IsNullOrWhiteSpace());
        }
    }
}
