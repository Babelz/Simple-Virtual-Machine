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
        /// No errors.
        /// </summary>
        public static byte NO_ERRORS = 0;

        /// <summary>
        /// Debug exception was thrown.
        /// </summary>
        public static byte DEBUG_EXCEPTION = 1;

        /// <summary>
        /// Abort has been called.
        /// </summary>
        public static byte ABORT_CALLED = 2;

        /// <summary>
        /// pc is in invalid state or invalid opcode was found.
        /// </summary>
        public static byte PC_CORRUPTED_OR_NOT_OPCODE = 3;
        #endregion
    }
}
