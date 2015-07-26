﻿using System;
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
        static VirtualMachine svm = new VirtualMachine(); 
        static ByteCodeProgram program = new ByteCodeProgram();

        static void Main(string[] args)
        {
            Process.GetCurrentProcess().PriorityBoostEnabled = true;

            program.AddBytes(Opcodes.Push_Direct, Sizes.WORD, 1, 0);

            for (int i = 0; i < 100000; i++)
            {
                program.AddBytes(Opcodes.Push_Direct, Sizes.WORD, 1, 0);
                program.AddBytes(Opcodes.Add_DirectStack, Sizes.WORD, Sizes.WORD);
            }

            program.AddBytes(Opcodes.Top, Sizes.WORD, Registers.BA);

            for (int i = 0; i < int.MaxValue; i++)
            {
                AddDirectStackPerformanceTest();
            }

            Console.ReadKey();
        }

        public static void AddDirectStackPerformanceTest()
        {
            svm.Initialize();

            Stopwatch sw = new Stopwatch();

            Console.WriteLine("Begin...");
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
