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
    /// EAN-13 barcode, encoded as defined in the GS1 General Specifications.
    /// Accepts 12 digits (the check digit is calculated) or 13 digits (the
    /// check digit is verified).
    /// </summary>
    public class EAN : LinearBarcode
    {
        // Left-hand digits: odd parity (L) and even parity (G).
        private static readonly string[] LeftOdd =
        {
            "0001101", "0011001", "0010011", "0111101", "0100011",
            "0110001", "0101111", "0111011", "0110111", "0001011"
        };

        private static readonly string[] LeftEven =
        {
            "0100111", "0110011", "0011011", "0100001", "0011101",
            "0111001", "0000101", "0010001", "0001001", "0010111"
        };

        // Right-hand digits (C).
        private static readonly string[] Right =
        {
            "1110010", "1100110", "1101100", "1000010", "1011100",
            "1001110", "1010000", "1000100", "1001000", "1110100"
        };

        // Parity of the six left-hand digits, selected by the first digit.
        // L = LeftOdd, G = LeftEven.
        private static readonly string[] Parity =
        {
            "LLLLLL", "LLGLGG", "LLGGLG", "LLGGGL", "LGLLGG",
            "LGGLLG", "LGGGLL", "LGLGLG", "LGLGGL", "LGGLGL"
        };

        private const string StartGuard = "101";
        private const string CenterGuard = "01010";
        private const string EndGuard = "101";

        private readonly string code;
        private readonly int[] bars;

        /// <param name="code">12 digits, or 13 digits with a correct check digit.</param>
        /// <exception cref="ArgumentNullException"><paramref name="code"/> is null.</exception>
        /// <exception cref="ArgumentException">The input has the wrong length, contains a non-digit, or has a wrong check digit.</exception>
        public EAN(string code)
        {
            if (code == null)
            {
                throw new ArgumentNullException(nameof(code));
            }
            if (code.Length != 12 && code.Length != 13)
            {
                throw new ArgumentException("EAN-13 codes must have 12 digits, or 13 digits including the check digit.", nameof(code));
            }
            if (!IsDigits(code))
            {
                throw new ArgumentException("EAN-13 codes must contain only the digits 0-9.", nameof(code));
            }

            int check = CheckDigit(code.Substring(0, 12));
            if (code.Length == 13 && code[12] - '0' != check)
            {
                throw new ArgumentException(
                    "Check digit mismatch: the EAN-13 check digit should be " + check + ".", nameof(code));
            }

            this.code = code.Substring(0, 12) + check;
            this.bars = Encode(this.code);
        }

        public string Code()
        {
            return code;
        }

        public int[] Barcode()
        {
            return (int[])bars.Clone();
        }

        /// <summary>
        /// Returns the check digit for the first 12 digits of an EAN-13 code.
        /// Used by UPC as well, because UPC-A is an EAN-13 with a leading zero.
        /// </summary>
        internal static int CheckDigit(string twelveDigits)
        {
            int sum = 0;
            for (int i = 0; i < 12; i++)
            {
                int digit = twelveDigits[i] - '0';
                sum += (i % 2 == 0) ? digit : digit * 3;
            }
            return (10 - sum % 10) % 10;
        }

        internal static bool IsDigits(string value)
        {
            foreach (char c in value)
            {
                if (c < '0' || c > '9')
                {
                    return false;
                }
            }
            return true;
        }

        // digits: 13 digits, check digit included.
        private static int[] Encode(string digits)
        {
            string parity = Parity[digits[0] - '0'];

            var bits = new StringBuilder(95);
            bits.Append(StartGuard);
            for (int i = 1; i <= 6; i++)
            {
                int digit = digits[i] - '0';
                bits.Append(parity[i - 1] == 'L' ? LeftOdd[digit] : LeftEven[digit]);
            }
            bits.Append(CenterGuard);
            for (int i = 7; i <= 12; i++)
            {
                bits.Append(Right[digits[i] - '0']);
            }
            bits.Append(EndGuard);

            return Modules.FromBits(bits.ToString());
        }
    }
}
