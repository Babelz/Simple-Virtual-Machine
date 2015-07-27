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
        public const byte LowAddress = 0;
        
        /// <summary>
        /// High address of registers. 
        /// </summary>
        public const byte HighAddress = 62;

        /*
         * 8-bit registers.
         */
        public const byte A = 0;
        public const byte B = 1;
        public const byte C = 2;
        public const byte D = 3;

        /* 
         * 16-bit registers.
         */
        public const byte AA = 4;
        public const byte BA = 6;
        public const byte CA = 8;
        public const byte DA = 10;

        /*
         * 32-bit registers.
         */
        public const byte AB = 12;
        public const byte BB = 16;
        public const byte CB = 20;
        public const byte DB = 24;

        /*
         * 64-bit registers.
         */
        public const byte AC = 28;
        public const byte BC = 36;
        public const byte CC = 44;
        public const byte DC = 52;

        /// <summary>
        /// Low address of the flags register.
        /// 16-bit register.
        /// </summary>
        public const byte FLAGS = 60;

        /// <summary>
        /// Returns the size of given register in bytes.
        /// </summary>
        /// <param name="lowAddress">start address of the register</param>
        /// <returns>size of the register in bytes</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte RegisterSize(byte lowAddress)
        {
            if (lowAddress >= A && lowAddress <= C)             return 1;
            else if (lowAddress >= AA && lowAddress <= DA)      return 2;
            else if (lowAddress >= AB && lowAddress <= DB)      return 4;
            else if (lowAddress >= AC && lowAddress <= DC)      return 8;   
            else if (lowAddress == FLAGS)                       return 4;
            else                                                return 0;   // Invalid register address.
        }
    }
}
