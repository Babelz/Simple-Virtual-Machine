using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SVMAssembler
{
    public enum TokenType
    {
        Declaration,
        Opcode
    }

    public sealed class Token
    {
        #region Fields
        private string[] arguments;
        #endregion

        #region Properties
        public TokenType Type
        {
            get;
            private set;
        }
        public string Header
        {
            get;
            private set;
        }

        public string RawLine
        {
            get;
            private set;
        }
        public int Linenumber
        {
            get;
            private set;
        }
        public int ArgumentsCount
        {
            get
            {
                return arguments.Length;
            }
        }
        #endregion

        public Token(TokenType type, string header, string[] arguments, string rawLine, int linenumber)
        {
            this.arguments = arguments;

            Type = type;
            Header = header;
            RawLine = rawLine;
            Linenumber = linenumber;
        }

        public string ArgumentAtIndex(int index)
        {
            return arguments[index];
        }
    }
}
