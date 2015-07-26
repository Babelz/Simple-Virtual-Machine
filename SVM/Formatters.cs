using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SVM
{
    public static class Formatters
    {
        public static string SizeMismatch(int pc, int abytes, int bbytes) 
        {
            return string.Format("Warning: size mismatch at {0}, adding {1}-bytes to {2}-bytes.", pc, abytes, bbytes);
        }
    }
}
