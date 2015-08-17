using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SVM
{
    public static class Flags
    {
        /// <summary>
        /// Math add flag.
        /// </summary>
        public const byte ADD = 0;

        /// <summary>
        /// Math sub flag.
        /// </summary>
        public const byte SUB = 1;

        /// <summary>
        /// Math div flag.
        /// </summary>
        public const byte DIV = 2;

        /// <summary>
        /// Math mul flag.
        /// </summary>
        public const byte MUL = 3;

        /// <summary>
        /// Math mod flag.
        /// </summary>
        public const byte MOD = 4;

        /// <summary>
        /// Sets standard out buffers out type to string.
        /// </summary>
        public const byte STR = 5;

        /// <summary>
        /// Sets standard out buffers out type to char.
        /// </summary>
        public const byte CH = 6;

        /// <summary>
        /// Sets standard out buffers out type to number.
        /// </summary>
        public const byte NUM = 7;
    }
}
