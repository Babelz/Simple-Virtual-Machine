using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SVM
{
    class Program
    {
        static VirtualMachine svm = new VirtualMachine(); 
        static ByteCodeProgram program = new ByteCodeProgram();

        private static void StartLoggerThread()
        {
        }

        static void Main(string[] args)
        {
            Process.GetCurrentProcess().PriorityBoostEnabled = true;

            program.AddBytes(Opcodes.Push_Direct, Sizes.WORD, 1, 0);

            /*for (int i = 0; i < 5000000; i++)
            {
                program.AddBytes(Opcodes.Push_Direct, Sizes.WORD, 1, 0);
                program.AddBytes(Opcodes.Add_DirectStack, Sizes.WORD, Sizes.WORD);
            }

            program.AddBytes(Opcodes.Top, Sizes.WORD, Registers.BA);

            for (int i = 0; i < int.MaxValue; i++)
            {
                AddDirectStackPerformanceTest();
            }*/

            long at = 0;
            long bt = 0;

            int times = 10;

            for (int j = 0; j < times; j++)
            {

                int chunks = 32;

                MemoryManager b = new MemoryManager(Sizes.CHUNK_2048KB * chunks);
                
                byte[] buffer = new byte[Sizes.CHUNK_2048KB * 128];
                
                int block = 8;
                
                var sw = Stopwatch.StartNew();
               

                for (int i = 0; i < b.HighAddress; i += block)
                {
                    b.ReadBytes(i, i + block, buffer);
                }

                sw.Stop();

                Console.WriteLine("Elapsed: " + sw.ElapsedMilliseconds);
                bt += sw.ElapsedMilliseconds;

                Console.WriteLine();
            }

            Console.WriteLine("A avg: " + at / times);
            Console.WriteLine("B avg: " + bt / times);

            long atv = at / times;
            long btv = bt / times;

            long max = Math.Max(atv, btv);
            long min = Math.Min(atv, btv);

            long maxPercent = max / 100;

            long percents = 0;
            long total = 0;
            while (max - total > min)
            {
                percents++;
                total += maxPercent;
            }

            Console.WriteLine("Diff: " + percents + "%");

            Console.ReadKey();
        }

        public static void AddDirectStackPerformanceTest()
        {
            svm.Initialize();

            Stopwatch sw = new Stopwatch();

            //Console.WriteLine("Begin...");
            sw.Start();

            svm.RunProgram(program);

            sw.Stop();
            Console.WriteLine("End...");

            Console.WriteLine("Elapsed: " + sw.ElapsedMilliseconds);
            Console.WriteLine("BA: " + svm.ReadRegisterValue(Registers.BA));

            /*svm.DumpStack();
            svm.DumpRegisters();*/
        }
    }
}
