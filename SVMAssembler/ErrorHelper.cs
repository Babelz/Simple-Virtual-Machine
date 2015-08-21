using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SVMAssembler
{
    public static class ErrorHelper
    {
        public static string TooFewArguments(Token token)
        {
            return string.Format("At line {0}, too few arguments. Line: \"{1}\"", token.Linenumber, token.RawLine);
        }
        public static string InvalidArgument(Token token)
        {
            return string.Format("At line {0}, invalid argument(s). Line: \"{1}\"", token.Linenumber, token.RawLine);
        }
        public static string SizeMisMatch(Token token, int expected)
        {
            return string.Format("At line {0} there is a size mismatch, expected {1}-byte(s) but got larger", token.Linenumber, expected);
        }
    }
}
