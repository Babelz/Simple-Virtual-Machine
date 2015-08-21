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

    public sealed class Tokenizer
    {
        public Tokenizer()
        {
        }

        public IEnumerable<Token> Tokenize(string[] lines)
        {
            List<Token> result = new List<Token>();

            // Excepting that the lines are trimmed
            // by the parser.
            for (int i = 0; i < lines.Length; i++)
            {
                string line = lines[i];

                if (string.IsNullOrEmpty(line)) continue;

                string[] tokens = line.Split(' ', ',');

                if (Statements.IsInstruction(tokens[0]))
                {
                    // Is an opcode.
                    TokenType type = TokenType.Opcode;
                    string instruction = tokens[0].Trim();
                    string[] arguments = new string[tokens.Length - 1];
                    string rawLine = line;
                    int linenumber = i + 1;

                    // Copy args.
                    Array.Copy(tokens, 1, arguments, 0, arguments.Length);

                    // Remove unwanted characters and trim.
                    arguments = arguments.Select(s => s.Replace(",", "").Trim()).ToArray();

                    result.Add(new Token(type, instruction, arguments, rawLine, linenumber));
                }
            }

            return result;
        }
    }
}
