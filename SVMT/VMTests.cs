﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using SVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SVMT
{
    [TestClass]
    public class VMTests
    {
        private VirtualMachine svm = new VirtualMachine();

        [TestMethod]    
        public void Add_DirectStack_Tests()
        {
            // Add direct stack.
            ByteCodeProgram program1 = new ByteCodeProgram();

            program1.AddBytes(Opcodes.Push_Direct, Sizes.WORD, 8, 0);
            program1.AddBytes(Opcodes.Push_Direct, Sizes.WORD, 7, 0);
            program1.AddBytes(Opcodes.Add_DirectStack, Sizes.WORD, Sizes.WORD);
            program1.AddBytes(Opcodes.Top, Sizes.WORD, Registers.AA);

            svm.Initialize();
            svm.RunProgram(program1);

            int result = svm.ReadRegisterValue(Registers.AA);

            Assert.AreEqual(15, result, "Direct stack failed");
        }

        [TestMethod]
        public void Add_IndirectRegister_Stack_Tests()
        {
            Random r = new Random();

            for (int i = 0; i < 64; i++)
            {
                byte lbs1 = (byte)(i + r.Next(0, 64));
                byte lbs2 = (byte)(i + r.Next(0, 64));

                // Add direct stack.
                ByteCodeProgram program1 = new ByteCodeProgram();
                program1.AddBytes(Opcodes.Load, Registers.AB, Sizes.LWORD, lbs1, 0, 0, 0);
                program1.AddBytes(Opcodes.Load, Registers.BB, Sizes.LWORD, lbs2, 0, 0, 0);
                program1.AddBytes(Opcodes.Add_IndirectRegister_Stack, Registers.AB, Registers.BB);
                program1.AddBytes(Opcodes.Top, Sizes.LWORD, Registers.CB);

                svm.Initialize();
                svm.RunProgram(program1);

                int result = svm.ReadRegisterValue(Registers.CB);

                Assert.AreEqual(lbs1 + lbs2, result, "IndirectRegister_Stack failed");
            }
        }

        [TestMethod]
        public void Add_IndirectRegister_Register_Tests()
        {
            ByteCodeProgram program = new ByteCodeProgram();

            program.AddBytes(Opcodes.Load, Registers.AA, Sizes.WORD, 1, 0);
            program.AddBytes(Opcodes.Add_IndirectRegister_Register, Registers.AA, Registers.BA, Registers.CA);

            for (int i = 0; i < 1000; i++)
            {
                program.AddBytes(Opcodes.Load, Registers.BA, Sizes.WORD, 1, 0);
                program.AddBytes(Opcodes.Add_IndirectRegister_Register, Registers.AA, Registers.CA, Registers.CA);
            }

            svm.Initialize();
            svm.RunProgram(program);

            int result = svm.ReadRegisterValue(Registers.CA);

            Assert.AreEqual(1001, result);
        }

        [TestMethod]
        public void Add_DirectStackRegister_Stack_Tests()
        {
            ByteCodeProgram program = new ByteCodeProgram();

            program.AddBytes(Opcodes.Push_Direct, Sizes.WORD, 1, 0);

            for (int i = 0; i < 10000; i++)
            {
                program.AddBytes(Opcodes.Load, Registers.AA, Sizes.WORD, 1, 0);
                program.AddBytes(Opcodes.Add_DirectStackRegister_Stack, Sizes.WORD, Registers.AA);
            }

            program.AddBytes(Opcodes.Top, Sizes.WORD, Registers.BA);

            svm.Initialize();
            svm.RunProgram(program);

            int result = svm.ReadRegisterValue(Registers.BA);

            Assert.AreEqual(10001, result);
        }

        [TestMethod]
        public void Add_DirectStackRegister_Register_Tests()
        {
            ByteCodeProgram program = new ByteCodeProgram();

            program.AddBytes(Opcodes.Load, Registers.BA, Sizes.WORD, 1, 0);

            for (int i = 0; i < 10000; i++)
            {
                program.AddBytes(Opcodes.Push_Direct, Sizes.WORD, 1, 0);
                program.AddBytes(Opcodes.Add_DirectStackRegister_Register, Sizes.WORD, Registers.BA, Registers.BA);
            }

            svm.Initialize();
            svm.RunProgram(program);

            int result = svm.ReadRegisterValue(Registers.BA);

            Assert.AreEqual(result, 10001);
        }

        [TestMethod]
        public void IncReg_Tests()
        {
            ByteCodeProgram program = new ByteCodeProgram();
            
            for (int i = 0; i < 1000; i++)
            {
                program.AddBytes(Opcodes.Inc_Reg, Registers.AA);
            }

            svm.Initialize();
            svm.RunProgram(program);

            Assert.AreEqual(1000, svm.ReadRegisterValue(Registers.AA));
        }

        [TestMethod]
        public void DecReg_Tests()
        {
            ByteCodeProgram program = new ByteCodeProgram();

            program.AddBytes(Opcodes.Load, Registers.AA, Sizes.WORD).AddValue(1000, Sizes.WORD);
            program.AddBytes(Opcodes.Load, Registers.AB, Sizes.WORD).AddValue(1000, Sizes.WORD);
           
            for (int i = 0; i < 1000; i++)
            {
                program.AddBytes(Opcodes.Dec_Reg, Registers.AA);
            }

            svm.Initialize();
            svm.RunProgram(program);

            Assert.AreEqual(0, svm.ReadRegisterValue(Registers.AA));
            Assert.AreEqual(1000, svm.ReadRegisterValue(Registers.AB));
        }

        [TestMethod]
        public void IncStack_Tests()
        {
            ByteCodeProgram program = new ByteCodeProgram();

            program.AddBytes(Opcodes.Push_Direct, Sizes.WORD).AddValue(0, Sizes.WORD);

            for (int i = 0; i < 1000; i++)
            {
                program.AddBytes(Opcodes.Inc_Stack, Sizes.WORD);
            }

            program.AddBytes(Opcodes.CopyStack_Direct, Registers.AA, Sizes.WORD, Sizes.WORD, (byte)svm.StackLowAddress, 0);

            svm.Initialize();
            svm.RunProgram(program);

            Assert.AreEqual(1000, svm.ReadRegisterValue(Registers.AA));
        }

        [TestMethod]
        public void DecStack_Tests()
        {
        }
    }
}
