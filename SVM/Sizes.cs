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
        public static readonly byte HWORD = 1;

        /// <summary>
        /// 16-bit word.
        /// </summary>
        public static readonly byte WORD = 2;

        /// <summary>
        /// 32-bit word.
        /// </summary>
        public static readonly byte LWORD = 4;

        /// <summary>
        /// 64-bit word.
        /// </summary>
        [Obsolete(@"Machine has not yet been tested with 64-bit 
                  members and is highly unlike that it can work
                  with variables of this size")]
        public static readonly byte DWORD = 8;

        /*
         * Memory chunk sizes.
         */

        /// <summary>
        /// 8 000.
        /// </summary>
        public const int CHUNK_8KB = 8000;

        /// <summary>
        /// 16 000.
        /// </summary>
        public const int CHUNK_16KB = 16000;

        /// <summary>
        /// 32 000.
        /// </summary>
        public const int CHUNK_32KB = 32000;

        /// <summary>
        /// 64 000
        /// </summary>
        public const int CHUNK_64KB = 64000;

        /// <summary>
        /// 128 000
        /// </summary>
        public const int CHUNK_128KB = 128000;

        /// <summary>
        /// 256 000
        /// </summary>
        public const int CHUNK_256KB = 256000;
        
        /// <summary>
        /// 512 000
        /// </summary>
        public const int CHUNK_512KB = 512000;

        /// <summary>
        /// 1 024 000
        /// </summary>
        public const int CHUNK_1024KB = 1024000;

        /// <summary>
        /// 2 048 000
        /// </summary>
        public const int CHUNK_2048KB = 2048000;

        public static byte ToByte(string size)
        {
            size = size.ToLower();

            if (size == "hword") return HWORD;
            if (size == "word") return WORD;
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
