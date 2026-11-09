/**
 * Copyright 2026 Floms, Inc.
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
    internal static class Modules
    {
        // Converts a string of '0' and '1' characters into one module per array element.
        internal static int[] FromBits(string bits)
        {
            var modules = new int[bits.Length];
            for (int i = 0; i < bits.Length; i++)
            {
                modules[i] = bits[i] == '1' ? 1 : 0;
            }
            return modules;
        }
    }
}
