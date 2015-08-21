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

            Tokenizer t = new Tokenizer();
            var tt = t.Tokenize(prog);

            LexicalAnalyzer a = new LexicalAnalyzer();
            a.Analyze(tt);

            CodeGenerator cg = new CodeGenerator();
            cg.

            return 0;
        }
    }
}
