using Microsoft.VisualStudio.TestTools.UnitTesting;
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

        /*
         * Add tests
         */

        [TestMethod]
        public void Add_DirectStack_Tests()
        {
            // Add direct stack.
            BytecodeBuffer program = new BytecodeBuffer();

            program.AddBytes(Bytecodes.Push_Direct, Sizes.WORD, 8, 0);
            program.AddBytes(Bytecodes.Push_Direct, Sizes.WORD, 7, 0);
            program.AddBytes(Bytecodes.Set_Flag_Direct, Flags.INT_ADD);
            program.AddBytes(Bytecodes.Arithmetic_Stack, Sizes.WORD, Sizes.WORD);
            program.AddBytes(Bytecodes.Top, Sizes.WORD, Registers.R16A);

            svm.Initialize();
            svm.RunProgram(program);

            int result = svm.ReadRegisterValue(Registers.R16A);

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
                BytecodeBuffer program = new BytecodeBuffer();
                program.AddBytes(Bytecodes.Set_Flag_Direct, Flags.INT_ADD);
                program.AddBytes(Bytecodes.Load, Registers.R32A, Sizes.LWORD, lbs1, 0, 0, 0);
                program.AddBytes(Bytecodes.Load, Registers.R32B, Sizes.LWORD, lbs2, 0, 0, 0);
                program.AddBytes(Bytecodes.Arithmetic_Register, Registers.R32A, Registers.R32B);
                program.AddBytes(Bytecodes.Top, Sizes.LWORD, Registers.R32C);

                svm.Initialize();
                byte pretc = svm.RunProgram(program);

                string ms = ReturnCodes.ToString(pretc);

                int result = svm.ReadRegisterValue(Registers.R32C);

                Assert.AreEqual(lbs1 + lbs2, result, "IndirectRegister_Stack failed, " + ms.ToLower());
            }
        }

        [TestMethod]
        public void Add_IndirectRegister_Register_Tests()
        {
            BytecodeBuffer program = new BytecodeBuffer();

            program.AddBytes(Bytecodes.Load, Registers.R16A, Sizes.WORD, 1, 0);
            program.AddBytes(Bytecodes.Set_Flag_Direct, Flags.INT_ADD);
            program.AddBytes(Bytecodes.Arithmetic_Register_Register, Registers.R16A, Registers.R16B, Registers.R16C);

            for (int i = 0; i < 1000; i++)
            {
                program.AddBytes(Bytecodes.Load, Registers.R16B, Sizes.WORD, 1, 0);
                program.AddBytes(Bytecodes.Arithmetic_Register_Register, Registers.R16A, Registers.R16C, Registers.R16C);
            }

            svm.Initialize();
            svm.RunProgram(program);

            int result = svm.ReadRegisterValue(Registers.R16C);

            Assert.AreEqual(1001, result);
        }

        /*
         * Sub tests
         */

        [TestMethod]
        public void Sub_DirectStack_Tests()
        {
            // Add direct stack.
            BytecodeBuffer program = new BytecodeBuffer();

            // a - b..
            program.AddBytes(Bytecodes.Push_Direct, Sizes.WORD, 10, 0);
            program.AddBytes(Bytecodes.Push_Direct, Sizes.WORD, 15, 0);
            program.AddBytes(Bytecodes.Set_Flag_Direct, Flags.INT_SUB);
            program.AddBytes(Bytecodes.Arithmetic_Stack, Sizes.WORD, Sizes.WORD);
            program.AddBytes(Bytecodes.Top, Sizes.WORD, Registers.R16A);

            svm.Initialize();
            svm.RunProgram(program);

            int result = svm.ReadRegisterValue(Registers.R16A);

            Assert.AreEqual(5, result, "Direct stack failed");
        }

        [TestMethod]
        public void Sub_IndirectRegister_Stack_Tests()
        {
            Random r = new Random();

            for (int i = 0; i < 64; i++)
            {
                byte lbs1 = (byte)(i + r.Next(0, 64));
                byte lbs2 = (byte)(i + r.Next(0, 64));

                // Add direct stack.
                BytecodeBuffer program1 = new BytecodeBuffer();
                program1.AddBytes(Bytecodes.Set_Flag_Direct, Flags.INT_SUB);
                program1.AddBytes(Bytecodes.Load, Registers.R32B, Sizes.LWORD, Math.Min(lbs1, lbs2), 0, 0, 0);
                program1.AddBytes(Bytecodes.Load, Registers.R32A, Sizes.LWORD, Math.Max(lbs1, lbs2), 0, 0, 0);
                program1.AddBytes(Bytecodes.Arithmetic_Register, Registers.R32A, Registers.R32B);
                program1.AddBytes(Bytecodes.Top, Sizes.LWORD, Registers.R32C);

                svm.Initialize();
                svm.RunProgram(program1);

                int result = svm.ReadRegisterValue(Registers.R32C);

                Assert.AreEqual(Math.Max(lbs1, lbs2) - Math.Min(lbs1, lbs2), result, "IndirectRegister_Stack failed");
            }
        }

        [TestMethod]
        public void Sub_IndirectRegister_Register_Tests()
        {
            BytecodeBuffer program = new BytecodeBuffer();

            program.AddBytes(Bytecodes.Load, Registers.R16A, Sizes.WORD).AddValue(1000, 2);
            program.AddBytes(Bytecodes.Load, Registers.R16B, Sizes.WORD, 1, 0);
            program.AddBytes(Bytecodes.Set_Flag_Direct, Flags.INT_SUB);

            for (int i = 0; i < 1000; i++)
            {
                program.AddBytes(Bytecodes.Arithmetic_Register_Register, Registers.R16A, Registers.R16B, Registers.R16A);
            }

            svm.Initialize();
            svm.RunProgram(program);

            int result = svm.ReadRegisterValue(Registers.R16A);

            Assert.AreEqual(0, result);
        }

        [TestMethod]
        public void IncReg_Tests()
        {
            BytecodeBuffer program = new BytecodeBuffer();
            
            for (int i = 0; i < 1000; i++)
            {
                program.AddBytes(Bytecodes.Inc_Reg, Registers.R16A);
            }

            svm.Initialize();
            svm.RunProgram(program);

            Assert.AreEqual(1000, svm.ReadRegisterValue(Registers.R16A));
        }

        [TestMethod]
        public void DecReg_Tests()
        {
            BytecodeBuffer program = new BytecodeBuffer();

            program.AddBytes(Bytecodes.Load, Registers.R16A, Sizes.WORD).AddValue(1000, Sizes.WORD);
            program.AddBytes(Bytecodes.Load, Registers.R32A, Sizes.WORD).AddValue(1000, Sizes.WORD);
           
            for (int i = 0; i < 1000; i++)
            {
                program.AddBytes(Bytecodes.Dec_Reg, Registers.R16A);
            }

            svm.Initialize();
            svm.RunProgram(program);

            Assert.AreEqual(0, svm.ReadRegisterValue(Registers.R16A));
            Assert.AreEqual(1000, svm.ReadRegisterValue(Registers.R32A));
        }

        [TestMethod]
        public void IncStack_Tests()
        {
            BytecodeBuffer program = new BytecodeBuffer();

            program.AddBytes(Bytecodes.Push_Direct, Sizes.WORD).AddValue(0, Sizes.WORD);

            for (int i = 0; i < 1000; i++)
            {
                program.AddBytes(Bytecodes.Inc_Stack, Sizes.WORD);
            }

            program.AddBytes(Bytecodes.CopyStack, Registers.R16A, Sizes.WORD, Sizes.WORD, (byte)svm.StackLowAddress, 0);

            svm.Initialize();
            svm.RunProgram(program);

            Assert.AreEqual(1000, svm.ReadRegisterValue(Registers.R16A));
        }

        [TestMethod]
        public void DecStack_Tests()
        {
            BytecodeBuffer program = new BytecodeBuffer();

            program.AddBytes(Bytecodes.Push_Direct, Sizes.WORD).AddValue(1000, Sizes.WORD);

            for (int i = 0; i < 1000; i++)
            {
                program.AddBytes(Bytecodes.Dec_Stack, Sizes.WORD);
            }

            program.AddBytes(Bytecodes.CopyStack, Registers.R16A, Sizes.WORD, Sizes.WORD, (byte)svm.StackLowAddress, 0);

            svm.Initialize();
            svm.RunProgram(program);

            Assert.AreEqual(0, svm.ReadRegisterValue(Registers.R16A));
        }

        [TestMethod]
        public void JezTests()
        {
            BytecodeBuffer program = new BytecodeBuffer();
            program.AddBytes(Bytecodes.Push_Direct, Sizes.HWORD, 0);
            program.AddBytes(Bytecodes.Jez, Sizes.HWORD, Sizes.HWORD).AddValue(11, Sizes.HWORD);
            program.AddBytes(Bytecodes.Nop);
            program.AddBytes(Bytecodes.Nop);
            program.AddBytes(Bytecodes.Nop);    // 9
            program.AddBytes(Bytecodes.Abort);  // 10
            program.AddBytes(Bytecodes.Push_Direct, Sizes.HWORD, 1);
            program.AddBytes(Bytecodes.CopyStack, Registers.R8A, Sizes.HWORD, Sizes.HWORD, (byte)(svm.StackLowAddress + 1));

            svm.Initialize();
            byte result = svm.RunProgram(program);
            string msg = ReturnCodes.ToString(result);

            Assert.AreEqual(0, result, msg);

            Assert.AreEqual(1, svm.ReadRegisterValue(Registers.R8A));
        }

        [TestMethod]
        public void JgzTests()
        {
            BytecodeBuffer program = new BytecodeBuffer();
            program.AddBytes(Bytecodes.Push_Direct, Sizes.HWORD, 1);
            program.AddBytes(Bytecodes.Jgz, Sizes.HWORD, Sizes.HWORD).AddValue(11, Sizes.HWORD);
            program.AddBytes(Bytecodes.Nop);
            program.AddBytes(Bytecodes.Nop);
            program.AddBytes(Bytecodes.Nop);    // 9
            program.AddBytes(Bytecodes.Abort);  // 10
            program.AddBytes(Bytecodes.Push_Direct, Sizes.HWORD, 1);
            program.AddBytes(Bytecodes.CopyStack, Registers.R8A, Sizes.HWORD, Sizes.HWORD, (byte)(svm.StackLowAddress + 1));

            svm.Initialize();
            byte result = svm.RunProgram(program);
            string msg = ReturnCodes.ToString(result);

            Assert.AreEqual(0, result, msg);

            Assert.AreEqual(1, svm.ReadRegisterValue(Registers.R8A));
        }

        [TestMethod]
        public void JlzTests()
        {
            BytecodeBuffer program = new BytecodeBuffer();
            program.AddBytes(Bytecodes.Push_Direct, Sizes.WORD).AddValue(-5, Sizes.WORD);
            program.AddBytes(Bytecodes.Jlz, Sizes.WORD, Sizes.WORD).AddValue(13, Sizes.WORD);
            program.AddBytes(Bytecodes.Nop);
            program.AddBytes(Bytecodes.Nop);
            program.AddBytes(Bytecodes.Nop);    // 11
            program.AddBytes(Bytecodes.Abort);  // 12
            program.AddBytes(Bytecodes.Push_Direct, Sizes.WORD).AddValue(1, Sizes.WORD);
            program.AddBytes(Bytecodes.CopyStack, Registers.R16A, Sizes.WORD, Sizes.WORD, (byte)(svm.StackLowAddress + 2), 0);

            svm.Initialize();
            byte result = svm.RunProgram(program);
            string msg = ReturnCodes.ToString(result);

            Assert.AreEqual(0, result, msg);

            Assert.AreEqual(1, svm.ReadRegisterValue(Registers.R16A));
        }

        [TestMethod]
        public void EqTests()
        {
            BytecodeBuffer program = new BytecodeBuffer();

            program.AddBytes(Bytecodes.Push_Direct, Sizes.HWORD).AddValue(10, Sizes.HWORD);
            program.AddBytes(Bytecodes.Push_Direct, Sizes.HWORD).AddValue(10, Sizes.HWORD);
            program.AddBytes(Bytecodes.Jeq, Sizes.HWORD, Sizes.HWORD, Sizes.HWORD).AddValue(10, Sizes.HWORD);
            program.AddBytes(Bytecodes.Abort);

            svm.Initialize();
            byte result = svm.RunProgram(program);
            string msg = ReturnCodes.ToString(result);

            Assert.AreEqual(0, result, msg);
        }

        [TestMethod]
        public void NeqTests()
        {
            BytecodeBuffer program = new BytecodeBuffer();

            svm.Initialize();
            svm.RunProgram(program);
        }

        [TestMethod]
        public void JumpTests()
        {
            BytecodeBuffer program = new BytecodeBuffer();
            program.AddBytes(Bytecodes.Jmp, Sizes.WORD).AddValue(12, Sizes.WORD);
            program.AddBytes(Bytecodes.Nop);
            program.AddBytes(Bytecodes.Nop);
            program.AddBytes(Bytecodes.Nop);
            program.AddBytes(Bytecodes.Nop);
            program.AddBytes(Bytecodes.Push_Direct, Sizes.WORD).AddValue(2, Sizes.WORD);
            program.AddBytes(Bytecodes.Push_Direct, Sizes.WORD).AddValue(1, Sizes.WORD);
            program.AddBytes(Bytecodes.CopyStack, Registers.R16A, Sizes.WORD, Sizes.WORD, (byte)svm.StackLowAddress, 0);

            svm.Initialize();
            byte result = svm.RunProgram(program);
            string msg = ReturnCodes.ToString(result);

            Assert.AreEqual(0, result, msg);

            Assert.AreEqual(1, svm.ReadRegisterValue(Registers.R16A));
        }

        [TestMethod]
        public void JumpStackTests()
        {
            BytecodeBuffer program = new BytecodeBuffer();
            program.AddBytes(Bytecodes.Push_Direct, Sizes.WORD).AddValue(14, Sizes.WORD);
            program.AddBytes(Bytecodes.Jmp_Stack, Sizes.WORD);
            program.AddBytes(Bytecodes.Nop);
            program.AddBytes(Bytecodes.Nop);
            program.AddBytes(Bytecodes.Nop);
            program.AddBytes(Bytecodes.Nop);
            program.AddBytes(Bytecodes.Push_Direct, Sizes.WORD).AddValue(2, Sizes.WORD);
            program.AddBytes(Bytecodes.Push_Direct, Sizes.WORD).AddValue(1, Sizes.WORD);
            program.AddBytes(Bytecodes.CopyStack, Registers.R16A, Sizes.WORD, Sizes.WORD, (byte)(svm.StackLowAddress + 2), 0);

            svm.Initialize();
            byte result = svm.RunProgram(program);
            string msg = ReturnCodes.ToString(result);

            Assert.AreEqual(0, result, msg);

            Assert.AreEqual(1, svm.ReadRegisterValue(Registers.R16A));
        }
    }
}
