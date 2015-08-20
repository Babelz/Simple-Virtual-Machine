using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SVM
{
    public static class Sizes
    {
        /*
         * Size keywords. 
         */

        // Size keywords supported by the machine are:
        //  - hword 8-bits
        //  - word 16-bits
        //  - lword 32-bits
        //  - dword 64-bits

        /// <summary>
        /// 8-bit word.
        /// </summary>
        public const byte HWORD = 1;

        /// <summary>
        /// 16-bit word.
        /// </summary>
        public const byte WORD = 2;

        /// <summary>
        /// 32-bit word.
        /// </summary>
        public const byte LWORD = 4;

        /// <summary>
        /// 64-bit word.
        /// </summary>
        [Obsolete(@"Machine has not yet been tested with 64-bit 
                  members and is highly unlike that it can work
                  with variables of this size")]
        public const byte DWORD = 8;

        /*
         * Memory chunk sizes.
         */

        /// <summary>
        /// 8192.
        /// </summary>
        public const int CHUNK_8KB = 8192;

        /// <summary>
        /// 16 384.
        /// </summary>
        public const int CHUNK_16KB = 16384;

        /// <summary>
        /// 32 768.
        /// </summary>
        public const int CHUNK_32KB = 32768;

        /// <summary>
        /// 65 536
        /// </summary>
        public const int CHUNK_64KB = 65536;

        /// <summary>
        /// 131 072
        /// </summary>
        public const int CHUNK_128KB = 131072;

        /// <summary>
        /// 262 144
        /// </summary>
        public const int CHUNK_256KB = 262144;
        
        /// <summary>
        /// 524 288
        /// </summary>
        public const int CHUNK_512KB = 524288;

        /// <summary>
        /// 1 048 576
        /// </summary>
        public const int CHUNK_1024KB = 1048576;

        /// <summary>
        /// 2 097 152
        /// </summary>
        public const int CHUNK_2048KB = 2097152;

        public static byte ToByte(string size)
        {
            size = size.ToLower();

            if (size == "hword") return HWORD;
            if (size == "word")  return WORD;
            if (size == "lword") return LWORD;
            if (size == "dword") return DWORD;

            return 0;
        }
        public static string ToString(byte size)
        {
            if (size == 1) return "hword";
            if (size == 2) return "word";
            if (size == 4) return "lword";
            if (size == 8) return "dword";

            return string.Empty;
        }
    }
}
