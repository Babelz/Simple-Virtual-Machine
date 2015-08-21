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

        private static readonly Regex push;
        private static readonly Regex pop;

        private static readonly Regex instruction;

        static Statements()
        {
            word = CreateRegex("(hword|word|lword|dword)");
            number = CreateRegex(@"\d+");
            register = CreateRegex(@"r\d{1,2}\w{1}");

            push = CreateRegex("push");
            pop = CreateRegex("pop");
            
            instruction = new Regex(string.Format("({0}|{1})", push, pop));
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

        public static bool IsInstruction(string str) 
        {
            return instruction.IsMatch(str);
        }

        public static bool IsPush(string str) 
        {
            return push.IsMatch(str);
        }
    }
}
