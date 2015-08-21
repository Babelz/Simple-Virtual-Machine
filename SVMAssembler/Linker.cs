using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SVMAssembler
{
    public sealed class Linker
    {
        public Linker()
        {
        }

        public string[] Link(string[] lines)
        {
            Logger.Instance.LogMessage("Linking...");

            // Find entrypoint files linkakes.
            string[] linkages = lines.Where(l => Statements.IsLink(l))
                                     .Select(l => l.Replace("lnk ", "").Trim())
                                     .ToArray();
            
            // Read files and parse them.
            Parser parser = new Parser();

            string[][] files = linkages.Select(f => File.ReadAllLines(f))
                                       .ToArray()
                                       .ToArray();

            for (int i = 0; i < files.Length; i++)
            {
                string[] file = files[i];

                parser.Parse(file);

                // Link this file.
                file = Link(file);

                files[i] = file;
            }


            // Insert linkages.
            List<string> result = lines.ToList();
            
            for (int i = 0; i < linkages.Length; i++)
            {
                for (int j = 0; j < result.Count; j++)
                {
                    if (result[j].Contains(linkages[i]))
                    {
                        result.RemoveAt(j);
                        result.InsertRange(j, files[i]);
                    }
                }
            }

            return result.ToArray();
        }
    }
}
