using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SVMAssembler
{
    public sealed class Token
    {
        public readonly Opcode Opcode;
        public readonly string[] Args;

        public Token(Opcode opcode, string[] args)
        {
            Opcode = opcode;
            Args = args;
        }
    }

    public sealed class Tokenizer
    {
        public Tokenizer()
        {
        }

        /*
         * Simple tokenizer, can only handle one operation code per 
         * line.
         * 
         * Example of working lines:
         *  push8 32
         *  push32 128
         *  
         * Example of invalid line:
         *  push8 32 push32 128
         *  
         * So one operation code + its args per line.
         */

        public IEnumerable<Opcode> Tokenize(string[] lines)
        {
            List<Opcode> tokens = new List<Opcode>();

            return tokens;
        }
    }
}
