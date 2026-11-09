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

namespace OpenBarcode.Linear
{
    /// <summary>
    /// UPC-A barcode. A UPC-A code is encoded as an EAN-13 whose first digit is zero.
    /// Accepts 11 digits (the check digit is calculated) or 12 digits (the check digit is verified).
    /// </summary>
    public class UPC : LinearBarcode
    {
        private readonly EAN ean;

        /// <param name="code">11 digits, or 12 digits with a correct check digit.</param>
        /// <exception cref="ArgumentNullException"><paramref name="code"/> is null.</exception>
        /// <exception cref="ArgumentException">The input has the wrong length, contains a non-digit, or has a wrong check digit.</exception>
        public UPC(string code)
        {
            if (code == null)
            {
                throw new ArgumentNullException(nameof(code));
            }
            if (code.Length != 11 && code.Length != 12)
            {
                throw new ArgumentException("UPC-A codes must have 11 digits, or 12 digits including the check digit.", nameof(code));
            }
            if (!EAN.IsDigits(code))
            {
                throw new ArgumentException("UPC-A codes must contain only the digits 0-9.", nameof(code));
            }

            string data = code.Substring(0, 11);
            // The UPC-A check digit is the EAN-13 check digit of "0" + data.
            int check = EAN.CheckDigit("0" + data);
            if (code.Length == 12 && code[11] - '0' != check)
            {
                throw new ArgumentException(
                    "Check digit mismatch: the UPC-A check digit should be " + check + ".", nameof(code));
            }

            this.ean = new EAN("0" + data);
        }

        public string Code()
        {
            return ean.Code().Substring(1);
        }

        public int[] Barcode()
        {
            return ean.Barcode();
        }
    }
}
