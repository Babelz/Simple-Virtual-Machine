using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SVM
{
    internal static class ReturnCodes
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
        #endregion
    }
}
