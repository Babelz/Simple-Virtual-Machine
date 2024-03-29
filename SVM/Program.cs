﻿using System;
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
        static BytecodeBuffer program = new BytecodeBuffer();
        static Stopwatch sw = new Stopwatch();

        private static void StartLoggerThread()
        {
        }

        static void Main(string[] args)
        {
            Process.GetCurrentProcess().PriorityClass = ProcessPriorityClass.RealTime;
            Process.GetCurrentProcess().PriorityBoostEnabled = true;

            program.AddBytes(Bytecodes.Push_Direct, Sizes.WORD, 1, 0);

            program.AddBytes(Bytecodes.Set_Flag_Direct, Flags.INT_ADD);

            for (int i = 0; i < 1000000; i++)
            {
                program.AddBytes(Bytecodes.Push_Direct, Sizes.WORD, 1, 0);
                program.AddBytes(Bytecodes.Arithmetic_Stack, Sizes.WORD, Sizes.WORD);
            }

            program.AddBytes(Bytecodes.Top, Sizes.WORD, Registers.R16B);

            for (int i = 0; i < int.MaxValue; i++)
            {
                AddDirectStackPerformanceTest();
            }

            Console.ReadKey();
        }

        public static void AddDirectStackPerformanceTest()
        {
            svm.Initialize();

            sw.Reset();

            Console.WriteLine("Begin...");
            
            sw.Start();

            svm.RunProgram(program);

            sw.Stop();

            Console.WriteLine("End...");
            Console.WriteLine("Elapsed: " + sw.ElapsedMilliseconds);
            Console.WriteLine("BA: " + svm.ReadRegisterValue(Registers.R16B));
            Console.WriteLine();
        }
    }
}
