using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SVMAssembler
{
    public static class Mnemonics
    {
        public const string Push    =   "push";
        public const string Push8   =   "push8";
        public const string Push16  =   "push16";
        public const string Push32  =   "push32";
        public const string PushReg =   "pushreg";

        public const string Pop     =   "pop";
        public const string PopReg  =   "pop";
        public const string Pop8    =   "pop8";
        public const string Pop16   =   "pop16";
        public const string Pop32   =   "pop32";

        public const string Top     =   "top";
        public const string Sp      =   "sp";
        public const string Pusb    =   "pushb";
        public const string LdStr   =   "ldstr";
        public const string LdCh    =   "ldch";
    }
}
