using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SVM
{
    public static class ReturnCodes
    {
        #region Fields
        /// <summary>
        /// Default return code.
        /// </summary>
        public const byte DEFAULT_RET_CODE = 0;

        /// <summary>
        /// Debug exception was thrown.
        /// </summary>
        public const byte DEBUG_EXCEPTION = 1;

        /// <summary>
        /// Abort has been called.
        /// </summary>
        public const byte ABORT_CALLED = 2;

        /// <summary>
        /// pc is in invalid state or invalid opcode was found.
        /// </summary>
        public const byte PC_CORRUPTED_OR_NOT_OPCODE = 3;

        /// <summary>
        /// Attempt to write protected memory was made.
        /// </summary>
        public const byte ACCESSING_PROTECTED_MEMORY = 4;

        public const byte INVALID_WORD_SIZE = 5;

        public const byte STACK_UNDERFLOW = 6;

        public const byte STACK_OVERFLOW = 7;

        public const byte REGISTER_OVERFLOW = 8;

        public const byte INVALID_REGISTER_ADDRESS = 9;

        public const byte INVALID_JUMP_ADDRESS = 10;
        #endregion

        public static string ToString(byte returnCode)
        {
            if      (returnCode == 0)   return "No errors";
            else if (returnCode == 1)   return "Debug exception";
            else if (returnCode == 2)   return "Abort was called";
            else if (returnCode == 3)   return "Program counter is corrupted or byte pointed by it was not an opcode";
            else if (returnCode == 4)   return "Attempt to access protected memory was made";
            else if (returnCode == 5)   return "Invalid word size";
            else if (returnCode == 6)   return "Stack underflow";
            else if (returnCode == 7)   return "Stack overflow";
            else if (returnCode == 8)   return "Register overflow";
            else if (returnCode == 9)   return "Invalid register address";
            else if (returnCode == 10)  return "Invalid jump address";
            else                        return "Custom return code";
        }
    }
}
