using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SVMAssembler
{
    public sealed class Opcode
    {
    }

    public static class Opcodes
    {
        #region Stack operations

        public static Opcode Push8 = new Opcode();
        public static Opcode Push16 = new Opcode();
        public static Opcode Push32 = new Opcode();
        
        #endregion
    }
}
