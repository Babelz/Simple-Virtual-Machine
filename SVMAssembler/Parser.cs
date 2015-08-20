﻿using System;
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

                // Remove spaces and new lines.
                line = line.Replace("\t", "");
                line = line.Replace("\r", "");
                line = line.Replace("\n", "");

                // Check if line contains comments.
                int index = line.IndexOf(";");

                if (index != -1) line = line.Substring(0, index);

                line = line.Trim();

                lines[i] = line;
            }

            // Remove all empty lines.
            lines = lines.Where(l => !string.IsNullOrWhiteSpace(l))
                         .ToArray();
        }
    }
}
