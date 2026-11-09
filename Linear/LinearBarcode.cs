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
namespace OpenBarcode.Linear
{
    /// <summary>
    /// A one-dimensional barcode.
    /// </summary>
    public interface LinearBarcode
    {
        /// <summary>
        /// The encoded text. For UPC and EAN this includes the check digit.
        /// </summary>
        string Code();

        /// <summary>
        /// The modules from left to right: 1 is a bar, 0 is a space.
        /// Each call returns a new array, so callers may change it freely.
        /// </summary>
        int[] Barcode();
    }
}
