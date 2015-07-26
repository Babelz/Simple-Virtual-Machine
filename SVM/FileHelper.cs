using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SVM
{
    internal static class FileHelper
    {
        public static void DumpData(string header, byte[] bytes, int columns, string path)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append(header);
            sb.Append("\n");
            sb.Append("\n");

            int j = 0;
            for (int i = 0; i < bytes.Length; i++)
            {
                sb.AppendFormat("{0:x2}", bytes[i]);
                sb.Append(" ");

                j++;

                if (j == columns)
                {
                    j = 0;

                    sb.Append("\n");
                }
            }

            string[] lines = sb.ToString().Split(new string[] { "\n" }, StringSplitOptions.None);

            File.WriteAllLines(path, lines);
        }

        public static void DumpRegisters(string header, byte[] registers, string path)
        {
            // TODO: implement.
        }
    }
}
