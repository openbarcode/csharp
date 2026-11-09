using System;
using OpenBarcode.Linear;
using Xunit;

namespace OpenBarcode.Tests
{
    public class UpcTests
    {
        // Cross-language vector: "03600029145" -> "036000291452", 95 modules.
        private const string Modules = "10100011010111101010111100011010001101000110101010110110011101001100110101110010011101101100101";

        [Fact]
        public void ElevenDigitsGetCheckDigitAndMatchVector()
        {
            var upc = new UPC("03600029145");

            Assert.Equal("036000291452", upc.Code());
            Assert.Equal(95, upc.Barcode().Length);
            Assert.Equal(Modules, TestHelpers.Bits(upc.Barcode()));
        }

        [Fact]
        public void TwelveDigitsWithCorrectCheckDigitAreAccepted()
        {
            var upc = new UPC("036000291452");

            Assert.Equal("036000291452", upc.Code());
            Assert.Equal(Modules, TestHelpers.Bits(upc.Barcode()));
        }

        [Fact]
        public void WrongCheckDigitThrows()
        {
            Assert.Throws<ArgumentException>(() => new UPC("036000291453"));
        }

        [Theory]
        [InlineData("03600A29145")]
        [InlineData("036000291 5")]
        [InlineData("-3600029145")]
        [InlineData("+36000291452")]
        public void NonDigitsThrow(string code)
        {
            Assert.Throws<ArgumentException>(() => new UPC(code));
        }

        [Theory]
        [InlineData("")]
        [InlineData("0360002914")]
        [InlineData("0360002914523")]
        public void WrongLengthThrows(string code)
        {
            Assert.Throws<ArgumentException>(() => new UPC(code));
        }

        [Fact]
        public void NullThrows()
        {
            Assert.Throws<ArgumentNullException>(() => new UPC(null));
        }

        [Fact]
        public void EncodingIsTheEanThirteenWithLeadingZero()
        {
            var upc = new UPC("03600029145");
            var ean = new EAN("003600029145");

            Assert.Equal(ean.Barcode(), upc.Barcode());
        }
    }
}
