using System;
using OpenBarcode.Linear;
using Xunit;

namespace OpenBarcode.Tests
{
    public class Code39Tests
    {
        // Cross-language vector: "CODE39" with start/stop, 8 characters x 12 modules + 7 gaps = 103.
        private const string CodeModules = "1001011011010110110100101011010110100101010110010110110101100101011011001010101011001011010100101101101";

        // Cross-language vector: "HELLO WORLD" (space is a valid character), 13 x 12 + 12 gaps = 168.
        private const string HelloWorldModules = "100101101101011010100110101101011001010101101010011010110101001101101011010010100110101101011001101010101101011010010110101011001010110101001101010110010110100101101101";

        [Fact]
        public void CodeMatchesVector()
        {
            var bc = new Code39("CODE39");

            Assert.Equal("CODE39", bc.Code());
            Assert.Equal(103, bc.Barcode().Length);
            Assert.Equal(CodeModules, TestHelpers.Bits(bc.Barcode()));
        }

        [Fact]
        public void HelloWorldMatchesVector()
        {
            var bc = new Code39("HELLO WORLD");

            Assert.Equal("HELLO WORLD", bc.Code());
            Assert.Equal(168, bc.Barcode().Length);
            Assert.Equal(HelloWorldModules, TestHelpers.Bits(bc.Barcode()));
        }

        [Fact]
        public void InputIsUpperCased()
        {
            var lower = new Code39("code39");

            Assert.Equal("CODE39", lower.Code());
            Assert.Equal(CodeModules, TestHelpers.Bits(lower.Barcode()));
        }

        [Fact]
        public void LeadingAndTrailingSpacesAreTrimmed()
        {
            var bc = new Code39("  A ");

            Assert.Equal("A", bc.Code());
            Assert.Equal(3 * 12 + 2, bc.Barcode().Length); // "*A*": 3 characters and 2 gaps
        }

        [Theory]
        [InlineData("A*B")]
        [InlineData("*")]
        [InlineData("AB!")]
        [InlineData("HELLO?")]
        [InlineData("É")]
        [InlineData("é")]
        public void UnsupportedCharactersThrow(string code)
        {
            Assert.Throws<ArgumentException>(() => new Code39(code));
        }

        [Theory]
        [InlineData("")]
        [InlineData("   ")]
        public void EmptyInputThrows(string code)
        {
            Assert.Throws<ArgumentException>(() => new Code39(code));
        }

        [Fact]
        public void NullThrows()
        {
            Assert.Throws<ArgumentNullException>(() => new Code39(null));
        }
    }
}
