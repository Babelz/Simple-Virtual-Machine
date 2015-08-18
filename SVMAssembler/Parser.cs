using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SVMAssembler
{
    public sealed class Parser
    {
        public Parser()
        {
        }

        public void Parse(string[] lines)
        {
            for (int i = 0; i < lines.Length; i++)
            {
                string line = lines[i];

                line = line.Replace("\t", "");
                line = line.Replace("\r", "");
                line = line.Replace("\n", "");
                line = line.Trim();

                lines[i] = line;
            }
        }
    }
}
