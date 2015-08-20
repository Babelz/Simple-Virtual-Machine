using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SVMAssembler
{
    public static class Statements
    {
        public static readonly Regex Word;
        public static readonly Regex IdentifierReference;
        public static readonly Regex Number;
        public static readonly Regex Address;

        public static readonly Regex Add_RegReg;
        public static readonly Regex Add_RegVal;
        public static readonly Regex Add_ValVal;
        public static readonly Regex Add_Size;
        public static readonly Regex Add_SizeSize;
        public static readonly Regex Add_IDReg;
        public static readonly Regex Add_IDValue;
        public static readonly Regex Add_IDID;

        public static ReadOnlyCollection<Regex> Add;

        static Statements()
        {
        }

        private static Regex CreateRegex(string pattern)
        {
            return new Regex(pattern, RegexOptions.Compiled);
        }
    }
}
