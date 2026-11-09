/**
 * Copyright 2013 Floms, LLC (Yoel Nunez <y.nunez@developers.floms.com>)
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 * http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 *
 */
using System;
using System.Text;

namespace OpenBarcode.Linear
{
    /// <summary>
    /// Code 39 barcode, standard character set: 0-9, A-Z, space, - . $ / + %.
    /// Input is trimmed and upper-cased. The start and stop character "*" is added
    /// automatically. Each character is 12 modules wide (nine elements: five bars
    /// and four spaces, three of them wide), with a one-module gap between characters.
    /// </summary>
    public class Code39 : LinearBarcode
    {
        private const string Alphabet = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ-. $/+%";

        // Nine elements per character, starting with a bar. 'n' is a narrow element
        // (1 module) and 'w' is a wide element (2 modules). Order matches Alphabet.
        private static readonly string[] Patterns =
        {
            "nnnwwnwnn", "wnnwnnnnw", "nnwwnnnnw", "wnwwnnnnn", "nnnwwnnnw",
            "wnnwwnnnn", "nnwwwnnnn", "nnnwnnwnw", "wnnwnnwnn", "nnwwnnwnn", // 0-9
            "wnnnnwnnw", "nnwnnwnnw", "wnwnnwnnn", "nnnnwwnnw", "wnnnwwnnn", // A-E
            "nnwnwwnnn", "nnnnnwwnw", "wnnnnwwnn", "nnwnnwwnn", "nnnnwwwnn", // F-J
            "wnnnnnnww", "nnwnnnnww", "wnwnnnnwn", "nnnnwnnww", "wnnnwnnwn", // K-O
            "nnwnwnnwn", "nnnnnnwww", "wnnnnnwwn", "nnwnnnwwn", "nnnnwnwwn", // P-T
            "wwnnnnnnw", "nwwnnnnnw", "wwwnnnnnn", "nwnnwnnnw", "wwnnwnnnn", // U-Y
            "nwwnwnnnn",                                                    // Z
            "nwnnnnwnw", "wwnnnnwnn", "nwwnnnwnn", "nwnwnwnnn",             // - . space $
            "nwnwnnnwn", "nwnnnwnwn", "nnnwnwnwn"                           // / + %
        };

        private const string StartStop = "nwnnwnwnn";

        private readonly string code;
        private readonly int[] bars;

        /// <param name="code">Text to encode. It is trimmed and upper-cased.</param>
        /// <exception cref="ArgumentNullException"><paramref name="code"/> is null.</exception>
        /// <exception cref="ArgumentException">The text is empty, or contains a character Code 39 cannot encode (including "*").</exception>
        public Code39(string code)
        {
            if (code == null)
            {
                throw new ArgumentNullException(nameof(code));
            }

            string normalized = code.Trim().ToUpperInvariant();
            if (normalized.Length == 0)
            {
                throw new ArgumentException("Code 39 input must not be empty.", nameof(code));
            }
            foreach (char c in normalized)
            {
                if (Alphabet.IndexOf(c) < 0)
                {
                    throw new ArgumentException("Character '" + c + "' cannot be encoded in Code 39.", nameof(code));
                }
            }

            this.code = normalized;
            this.bars = Encode(normalized);
        }

        public string Code()
        {
            return code;
        }

        public int[] Barcode()
        {
            return (int[])bars.Clone();
        }

        private static int[] Encode(string data)
        {
            string framed = "*" + data + "*";

            var bits = new StringBuilder();
            for (int i = 0; i < framed.Length; i++)
            {
                if (i > 0)
                {
                    bits.Append('0'); // one-module gap between characters
                }

                string pattern = framed[i] == '*'
                    ? StartStop
                    : Patterns[Alphabet.IndexOf(framed[i])];

                for (int k = 0; k < pattern.Length; k++)
                {
                    int width = pattern[k] == 'w' ? 2 : 1;
                    char bit = (k % 2 == 0) ? '1' : '0';
                    bits.Append(bit, width);
                }
            }

            return Modules.FromBits(bits.ToString());
        }
    }
}
