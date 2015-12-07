﻿using SVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SVMAssembler
{
    class Program
    {
        static int Main(string[] args)
        {
            string[] prog = new string[] 
                {
                    "; paska commnet",
                    "",
                    "",
                    "",
                    "push word 32   ; ebin commnet",
                    "push8 255",
                    "push16 2424",
                    "push32 9284",
                    "pushreg r8a",
                    "pushreg r16a",
                    "pushreg r32a",
                };

            Parser p = new Parser();
            p.Parse(prog);

            Linker l = new Linker();
            l.Link(prog);

            Logger.Instance.GetMessages(LogLevel.Message).ToList().ForEach(s => Console.WriteLine(s)); Logger.Instance.ClearMessages(LogLevel.Message);
            Logger.Instance.GetMessages(LogLevel.Warning).ToList().ForEach(s => Console.WriteLine(s)); Logger.Instance.ClearMessages(LogLevel.Warning);
            Logger.Instance.GetMessages(LogLevel.Error).ToList().ForEach(s => Console.WriteLine(s)); Logger.Instance.ClearMessages(LogLevel.Error);

            Tokenizer t = new Tokenizer();
            var tt = t.Tokenize(prog);

            Logger.Instance.GetMessages(LogLevel.Message).ToList().ForEach(s => Console.WriteLine(s)); Logger.Instance.ClearMessages(LogLevel.Message);
            Logger.Instance.GetMessages(LogLevel.Warning).ToList().ForEach(s => Console.WriteLine(s)); Logger.Instance.ClearMessages(LogLevel.Warning);
            Logger.Instance.GetMessages(LogLevel.Error).ToList().ForEach(s => Console.WriteLine(s)); Logger.Instance.ClearMessages(LogLevel.Error);

            LexicalAnalyzer a = new LexicalAnalyzer();
            a.Analyze(tt);

            Logger.Instance.GetMessages(LogLevel.Message).ToList().ForEach(s => Console.WriteLine(s)); Logger.Instance.ClearMessages(LogLevel.Message);
            Logger.Instance.GetMessages(LogLevel.Warning).ToList().ForEach(s => Console.WriteLine(s)); Logger.Instance.ClearMessages(LogLevel.Warning);
            Logger.Instance.GetMessages(LogLevel.Error).ToList().ForEach(s => Console.WriteLine(s)); Logger.Instance.ClearMessages(LogLevel.Error);

            CodeGenerator cg = new CodeGenerator();
            BytecodeBuffer bf = cg.GenerateCode(tt);

            Logger.Instance.GetMessages(LogLevel.Message).ToList().ForEach(s => Console.WriteLine(s)); Logger.Instance.ClearMessages(LogLevel.Message);
            Logger.Instance.GetMessages(LogLevel.Warning).ToList().ForEach(s => Console.WriteLine(s)); Logger.Instance.ClearMessages(LogLevel.Warning);
            Logger.Instance.GetMessages(LogLevel.Error).ToList().ForEach(s => Console.WriteLine(s)); Logger.Instance.ClearMessages(LogLevel.Error);

            Console.WriteLine();
            Console.WriteLine("(string)\tProgram bytes: " + (prog.Select(s => s.Length).Sum() * 8));
            Console.WriteLine("(bytecode)\tProgram bytes: " + (bf.GetBytes().Length * 8));
            Console.WriteLine();

            return 0;
        }
    }
}
