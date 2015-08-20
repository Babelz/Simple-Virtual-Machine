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
        /// Math int add flag.
        /// </summary>
        public const byte INT_ADD = 0;

        /// <summary>
        /// Math int sub flag.
        /// </summary>
        public const byte INT_SUB = 1;

        /// <summary>
        /// Math int  div flag.
        /// </summary>
        public const byte INT_DIV = 2;

        /// <summary>
        /// Math int mul flag.
        /// </summary>
        public const byte INT_MUL = 3;

        /// <summary>
        /// Math int mod flag.
        /// </summary>
        public const byte INT_MOD = 4;

        /// <summary>
        /// Math float add flag.
        /// </summary>
        public const byte FLOAT_ADD = 5;

        /// <summary>
        /// Math float sub flag.
        /// </summary>
        public const byte FLOAT_SUB = 6;

        /// <summary>
        /// Math float  div flag.
        /// </summary>
        public const byte FLOAT_DIV = 7;

        /// <summary>
        /// Math float mul flag.
        /// </summary>
        public const byte FLOAT_MUL = 8;

        /// <summary>
        /// Math float mod flag.
        /// </summary>
        public const byte FLOAT_MOD = 9;

        /// <summary>
        /// Sets standard out buffers out type to string.
        /// </summary>
        public const byte STR = 10;

        /// <summary>
        /// Sets standard out buffers out type to int.
        /// </summary>
        public const byte INT = 11;

        /// <summary>
        /// Sets standard out buffers out type to float.
        /// </summary>
        public const byte FLOAT = 11;
    }
}
