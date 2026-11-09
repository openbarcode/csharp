using System;
using OpenBarcode.Linear;
using Xunit;

namespace OpenBarcode.Tests
{
    public class EanTests
    {
        // Cross-language vector: "400638133393" -> "4006381333931", 95 modules.
        private const string Modules = "10100011010100111010111101111010001001011001101010100001010000101000010111010010000101100110101";

        // ISBN-13 978-0-306-40615-7, which uses the "LGGLGL" parity (first digit 9).
        private const string IsbnModules = "10101110110001001010011101111010100111010111101010101110011100101010000110011010011101000100101";

        [Fact]
        public void TwelveDigitsGetCheckDigitAndMatchVector()
        {
            var ean = new EAN("400638133393");

            Assert.Equal("4006381333931", ean.Code());
            Assert.Equal(95, ean.Barcode().Length);
            Assert.Equal(Modules, TestHelpers.Bits(ean.Barcode()));
        }

        [Fact]
        public void ThirteenDigitsWithCorrectCheckDigitAreAccepted()
        {
            var ean = new EAN("4006381333931");

            Assert.Equal("4006381333931", ean.Code());
            Assert.Equal(Modules, TestHelpers.Bits(ean.Barcode()));
        }

        [Fact]
        public void OddParityLeadingDigitMatchesIsbnVector()
        {
            var ean = new EAN("978030640615");

            Assert.Equal("9780306406157", ean.Code());
            Assert.Equal(IsbnModules, TestHelpers.Bits(ean.Barcode()));
        }

        [Fact]
        public void WrongCheckDigitThrows()
        {
            Assert.Throws<ArgumentException>(() => new EAN("4006381333932"));
        }

        [Theory]
        [InlineData("40063813339X")]
        [InlineData("400638 33393")]
        [InlineData("-00638133393")]
        public void NonDigitsThrow(string code)
        {
            Assert.Throws<ArgumentException>(() => new EAN(code));
        }

        [Theory]
        [InlineData("")]
        [InlineData("40063813339")]
        [InlineData("40063813339313")]
        public void WrongLengthThrows(string code)
        {
            Assert.Throws<ArgumentException>(() => new EAN(code));
        }

        [Fact]
        public void NullThrows()
        {
            Assert.Throws<ArgumentNullException>(() => new EAN(null));
        }
    }
}
