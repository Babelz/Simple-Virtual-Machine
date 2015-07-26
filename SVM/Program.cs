using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace SVM
{
    class Program
    {
        static void Main(string[] args)
        {
            Process.GetCurrentProcess().PriorityBoostEnabled = true;

            List<byte[]> bs = new List<byte[]>();
            for (int i = 0; i < 100000; i++) bs.Add(ByteHelper.ToBytes(i, 4));

            for (int i = 0; i < bs.Count; i++)
            {
                var b = ByteHelper.AddBytes(bs[i], bs[i]);

                int j = ByteHelper.ToInt(b);

                Debug.Assert(j == i * 2, i.ToString() + " ---- " + j.ToString());
            }

            for (int i = 0; i < 25; i++)
            {
                AddDirectStackPerformanceTest();
            }

            Console.ReadKey();
        }

        public static void AddDirectStackPerformanceTest()
        {
            ByteCodeProgram program = new ByteCodeProgram();
            program.AddBytes(Opcodes.Push_Direct, Sizes.WORD, 1, 0);

            for (int i = 0; i < 1000000; i++)
            {
                program.AddBytes(Opcodes.Push_Direct, Sizes.WORD, 1, 0);
                program.AddBytes(Opcodes.Add_DirectStack, Sizes.WORD, Sizes.WORD);
            }

            program.AddBytes(Opcodes.Top, Sizes.WORD, Registers.BA);

            VirtualMachine svm = new VirtualMachine();
            svm.Initialize();

            Stopwatch sw = new Stopwatch();

            Console.WriteLine("Begin...");
            sw.Start();

            svm.RunProgram(program);

            sw.Stop();
            Console.WriteLine("End...");

            Console.WriteLine("Elapsed: " + sw.ElapsedMilliseconds);
            Console.WriteLine("BA: " + svm.ReadRegisterValue(Registers.BA));

            svm.DumpStack();
            svm.DumpRegisters();
        }
    }
}
