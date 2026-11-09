using System.Linq;

namespace OpenBarcode.Tests
{
    internal static class TestHelpers
    {
        // Writes modules as a string of '1' (bar) and '0' (space) for easy comparison.
        internal static string Bits(int[] modules)
        {
            return string.Concat(modules.Select(m => m == 1 ? "1" : "0"));
        }
    }
}
