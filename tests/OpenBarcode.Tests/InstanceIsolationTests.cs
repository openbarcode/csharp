using OpenBarcode.Linear;
using Xunit;

namespace OpenBarcode.Tests
{
    // Regression tests: barcode instances must not share module data, either with
    // each other or with the caller.
    public class InstanceIsolationTests
    {
        [Fact]
        public void Code39InstancesAreIndependent()
        {
            var first = new Code39("CODE39");
            string before = TestHelpers.Bits(first.Barcode());

            var second = new Code39("HELLO WORLD");

            Assert.Equal(before, TestHelpers.Bits(first.Barcode()));
            Assert.Equal("CODE39", first.Code());
            Assert.Equal("HELLO WORLD", second.Code());
            Assert.NotEqual(TestHelpers.Bits(first.Barcode()), TestHelpers.Bits(second.Barcode()));
        }

        [Fact]
        public void EanAndUpcInstancesAreIndependent()
        {
            var ean = new EAN("400638133393");
            string before = TestHelpers.Bits(ean.Barcode());

            var upc = new UPC("12345678905");

            Assert.Equal(before, TestHelpers.Bits(ean.Barcode()));
            Assert.Equal("4006381333931", ean.Code());
            Assert.Equal("123456789050", upc.Code());
        }

        [Fact]
        public void ModifyingReturnedArrayDoesNotChangeBarcode()
        {
            var bc = new Code39("CODE39");
            string expected = TestHelpers.Bits(bc.Barcode());

            int[] modules = bc.Barcode();
            modules[0] = 1 - modules[0];

            Assert.Equal(expected, TestHelpers.Bits(bc.Barcode()));
        }
    }
}
