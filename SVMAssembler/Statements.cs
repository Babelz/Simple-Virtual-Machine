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
        private static readonly Regex word;
        private static readonly Regex number;
        private static readonly Regex register;
        private static readonly Regex quotedChar;
        private static readonly Regex quotedString;

        private static readonly Regex link;
        private static readonly Regex macro;

        private static readonly Regex push;
        private static readonly Regex pop;
        private static readonly Regex top;
        private static readonly Regex sp;
        private static readonly Regex pushb;
        private static readonly Regex ldstr;
        private static readonly Regex ldch;

        private static readonly Regex mnemonic;

        static Statements()
        {
            word = CreateRegex("(hword|word|lword|dword)");
            number = CreateRegex(@"\d+");
            register = CreateRegex(@"r\d{1,2}\w{1}");
            quotedChar = CreateRegex("'[a-zA-Z]'");
            quotedString = CreateRegex("\"[a-zA-Z]\"");

            link = CreateRegex(@"lnk.+\.svma");
            macro = CreateRegex("#def .+");

            push = CreateRegex("push");
            pop = CreateRegex("pop");
            top = CreateRegex("top");
            sp = CreateRegex("sp");
            pushb = CreateRegex("pushb");
            ldstr = CreateRegex("ldstr");
            ldch = CreateRegex("ldch");
            
            mnemonic = CreateRegex(string.Format("({0}|{1})", push, pop));
        }

        private static Regex CreateRegex(string pattern)
        {
            return new Regex(pattern, RegexOptions.Compiled);
        }
        
        public static bool IsWord(string str)
        {
            return word.IsMatch(str);
        }
        public static bool IsNumber(string str) 
        {
            return number.IsMatch(str);
        }
        public static bool IsRegister(string str) 
        {
            return register.IsMatch(str);
        }

        public static bool IsLink(string str)
        {
            return link.IsMatch(str);
        }
        public static bool IsMacro(string str)
        {
            return macro.IsMatch(str);
        }

        public static bool IsMnemonic(string str) 
        {
            return mnemonic.IsMatch(str);
        }

        public static bool IsPush(string str) 
        {
            return push.IsMatch(str);
        }
        public static bool IsPop(string str)
        {
            return pop.IsMatch(str);
        }
        public static bool IsTop(string str)
        {
            return top.IsMatch(str);
        }
        public static bool IsSp(string str)
        {
            return sp.IsMatch(str);
        }
        public static bool IsPushb(string str)
        {
            return pushb.IsMatch(str);
        }
        public static bool IsLdStr(string str)
        {
            return ldstr.IsMatch(str);
        }
        public static bool IsLdCh(string str)
        {
            return ldch.IsMatch(str);
        }
        public static bool IsCharacter(string str)
        {
            return quotedChar.IsMatch(str);
        }
        public static bool IsString(string str)
        {
            return quotedString.IsMatch(str);
        }
    }
}
