using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
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
        public const byte HighAddress = 82;

        /*
         * 8-bit registers.
         */
        public const byte R8A = 0;
        public const byte R8B = 1;
        public const byte R8C = 2;
        public const byte R8D = 3;

        /* 
         * 16-bit registers.
         */
        public const byte R16A = 4;
        public const byte R16B = 6;
        public const byte R16C = 8;
        public const byte R16D = 10;

        /*
         * 32-bit registers.
         */
        public const byte R32A = 12;
        public const byte R32B = 16;
        public const byte R32C = 20;
        public const byte R32D = 24;

        /*
         * 64-bit registers.
         */
        public const byte R64A = 28;
        public const byte R64B = 36;
        public const byte R64C = 44;
        public const byte R64D = 52;

        /*
         * Machines "private" 32-bit work registers.
         * Should not be used by the user.
         */
        public const byte W32A = 56;
        public const byte W32B = 60;
        public const byte W32C = 64;
        public const byte W32D = 68;
        public const byte W32E = 72;
        public const byte W32F = 76;
        public const byte W32G = 80;

        /// <summary>
        /// Low address of the flags register.
        /// 8-bit register.
        /// </summary>
        public const byte RFLAGS = 81;

        static Registers()
        {
        }

        /// <summary>
        /// Returns the size of given register in bytes.
        /// </summary>
        /// <param name="lowAddress">start address of the register</param>
        /// <returns>size of the register in bytes</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte GetRegisterCapacity(byte lowAddress)
        {
            if      (lowAddress >= R8A && lowAddress <= R8C)        return 1;
            else if (lowAddress >= R16A && lowAddress <= R16D)      return 2;
            else if (lowAddress >= R32A && lowAddress <= R32D)      return 4;
            else if (lowAddress >= R64A && lowAddress <= R64D)      return 8;  
            else if (lowAddress <= W32A && lowAddress <= W32G)      return 4;
            else if (lowAddress == RFLAGS)                          return 4;
            else                                                    return 0;   // Invalid register address.
        }
    }
}
