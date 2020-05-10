using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace engenious.Helper
{
    /// <summary>
    /// Helper class for bit operations.
    /// </summary>
    public static class BitHelper
    {
        /// <summary>
        /// Converts BigEndian to LittleEndian and vice versa.
        /// </summary>
        /// <param name="val">The value convert.</param>
        /// <returns>The converted value.</returns>
        public static uint EndiannessConvert(uint val)
        {
            return (val << 24) | (val << 8) & 0xFF0000 | (val >> 8) & 0xFF00 | (val >> 24);
        }

        /// <summary>
        /// Converts values read by <see cref="BitConverter"/> to little endian.
        /// </summary>
        /// <param name="val">The value from the <see cref="BitConverter"/>.</param>
        /// <returns>The value as little endian.</returns>
        public static uint BitConverterToLittleEndian(uint val)
        {
            if (BitConverter.IsLittleEndian)
                return val;
            return EndiannessConvert(val);
        }

        /// <summary>
        /// Converts values read by <see cref="BitConverter"/> to big endian.
        /// </summary>
        /// <param name="val">The value from the <see cref="BitConverter"/>.</param>
        /// <returns>The value as big endian.</returns>
        public static uint BitConverterToBigEndian(uint val)
        {
            if (BitConverter.IsLittleEndian)
                return EndiannessConvert(val);
            return val;
        }
    }
}
