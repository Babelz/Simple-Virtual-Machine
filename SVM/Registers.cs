using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace SVM
{
    /// <summary>
    /// Contains begin addresses of given registers.
    /// </summary>
    public static class Registers
    {
        /// <summary>
        /// Low address of registers.
        /// </summary>
        public static readonly byte RegistersLowAddress = 0;
        
        /// <summary>
        /// High address of registers.
        /// </summary>
        public static readonly byte RegisterHighAddress = 66;

        /*
         * 8-bit registers.
         */
        public static readonly byte A = 0;
        public static readonly byte B = 1;
        public static readonly byte C = 2;
        public static readonly byte D = 3;

        /*
         * 16-bit registers.
         */
        public static readonly byte AA = 4;
        public static readonly byte BA = 6;
        public static readonly byte CA = 8;
        public static readonly byte DA = 10;

        /*
         * 32-bit registers.
         */
        public static readonly byte AB = 14;
        public static readonly byte BB = 18;
        public static readonly byte CB = 22;
        public static readonly byte DB = 26;

        /*
         * 64-bit registers.
         */
        public static readonly byte AC = 34;
        public static readonly byte BC = 42;
        public static readonly byte CC = 50;
        public static readonly byte DC = 58;

        /// <summary>
        /// Low address of the flags register.
        /// 16-bit register.
        /// </summary>
        public static readonly byte FLAGS = 66;

        /// <summary>
        /// Returns the size of given register in bytes.
        /// </summary>
        /// <param name="lowAddress">start address of the register</param>
        /// <returns>size of the register in bytes</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte RegisterSize(byte lowAddress)
        {
            if (lowAddress >= A && lowAddress <= D)             return 1;
            else if (lowAddress >= AA && lowAddress <= DA)      return 2;
            else if (lowAddress >= AB && lowAddress <= DB)      return 4;
            else if (lowAddress >= AC && lowAddress <= DC)      return 8;   
            else if (lowAddress == FLAGS)                       return 4;
            else                                                return 0;   // Invalid register address.
        }
    }
}
