using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace SVM
{
    // TODO:
    //  - create functions for bit mul and div 
    //  - flags register (for signed and unsigned opers etc..)
    //  - new error handling system to remove debug asserts and exceptions...
    //  - conditional operators
    public sealed class VirtualMachine
    {
        #region Fields
        // Program memory.
        private byte[] program;

        // General purpose 8-bit registers.
        // 0-3

        // General purpose 16-bit registers.
        // 4-7

        // General purpose 32-bit registers.
        // 8-11

        // General purpose 64-bit registers.
        // 9-12

        // VM memory.
        // 
        // Example of how bits are laid out in the memory:
        //    - 16-bit variable example:
        //          [0] = lsbs
        //          [1] = msbs
        private readonly MemoryManager memory;

        /// <summary>
        /// Stack pointer.
        /// </summary>
        private int sp;

        /// <summary>
        /// Program counter.
        /// </summary>
        private int pc;

        /// <summary>
        /// Program counter registers used to return from 
        /// function calls.
        /// </summary>
        private int retpc;
        #endregion

        #region Properties
        public int StackSize
        {
            get
            {
                return memory.HighAddress - Registers.RegisterHighAddress + 1;
            }
        }
        public int StackLowAddress
        {
            get
            {
                return Registers.RegisterHighAddress + 1;
            }
        }
        public int StackHighAddress
        {
            get
            {
                return sp;
            }
        }
        #endregion

        public VirtualMachine()
        {
            // 64Kb should be enough memory for the 
            // starters.
            memory = new MemoryManager(Sizes.CHUNK_64KB);
        }

        #region Private members
        /// <summary>
        /// Moves the program counter by given offset.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void MoveProgramCounter(int offset)
        {
            pc += offset;
        }

        /// <summary>
        /// Moves program counter by one and returns byte
        /// pointed by it.
        /// </summary>
        /// <returns>byte pointed by the program counter</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private byte NextProgramByte()
        {
            pc++;
            return program[pc];
        }

        /// <summary>
        /// Moves program counter by the count of the bytes
        /// and returns given amount of bytes from the program
        /// memory.
        /// </summary>
        /// <returns>bytes from given offset location</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private byte[] NextProgramBytes(int count)
        {
            int i = 0;
            int pcn = pc + count;
            byte[] bytes = new byte[count];

            while (pc != pcn)
            {
                pc++;

                bytes[i] = program[pc];
                
                i++;
            }

            return bytes;
        }

        /// <summary>
        /// Moves the stack pointer by given offset.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void MoveStackPointer(int offset)
        {
            sp += offset;

            // TODO: insert real exception handling.
            if (sp > StackLowAddress && sp < StackHighAddress) throw new InvalidOperationException("Stack underflow");
        }
        #endregion

        /// <summary>
        /// Should be called before any programs are executed.
        /// Resets the state of registers and memory of
        /// the machine.
        /// </summary>
        public void Initialize()
        {
            // Clear and reset memory size back to 
            // 64kb which should be enough for starters.
            memory.Clear();
            memory.Resize(Sizes.CHUNK_64KB);

            // Stack pointer should start from register 
            // high address since registers live below this 
            // address.
            sp = Registers.RegisterHighAddress + 1;

            // Reset pc and retpc registers.
            pc = retpc = 0;

            program = null;
        }

        private bool InterpretOpcode(byte opcode)
        {
            // Interpret next opcode and return.
            if (opcode == Opcodes.Push_Direct)
            {
                // Get size of the variable in bytes.
                byte bytes = NextProgramByte();

                Debug.Assert(bytes <= Sizes.DWORD);

                // Read the variable values.
                byte[] bits = NextProgramBytes(bytes);

                // Reserve room for the bytes and write them to the stack.
                memory.Reserve(bytes, sp);
                memory.WriteBytes(bits, sp);

                MoveStackPointer(bytes);
            }
            else if (opcode == Opcodes.Push_Register)
            {
                // Read register address and get its size.
                byte register = NextProgramByte();
                byte bytes = Registers.RegisterSize(register);

                Debug.Assert(bytes <= Sizes.DWORD);

                // Reserve memory for next push operation.
                memory.Reserve(bytes, sp);

                // Get the bytes and write them to the stack.
                byte[] bits = memory.ReadBytes(register, register + bytes);
                memory.WriteBytes(bits, sp);

                MoveStackPointer(bytes);
            }
            else if (opcode == Opcodes.Pop)
            {
                Debug.Assert(sp > Registers.RegisterHighAddress);

                byte bytes = NextProgramByte();

                // Everything after this address is trash.
                MoveStackPointer(bytes + 1);

                memory.WriteByte(0, sp);
            }
            else if (opcode == Opcodes.Top)
            {
                byte bytes = NextProgramByte();
                byte register = NextProgramByte();
                byte registerCapacity = Registers.RegisterSize(register);

                // Read bits.
                byte[] bits = memory.ReadBytes(sp - bytes, sp);

                Debug.Assert(registerCapacity >= bytes);

                memory.WriteBytes(bits, register);
            }
            else if (opcode == Opcodes.Sp)
            {
                byte register = NextProgramByte();
                byte bytes = Registers.RegisterSize(register);

                // Check that the stack pointer fits to this register.
                Debug.Assert(bytes >= 4);

                memory.WriteBytes(ByteHelper.ToBytes(sp, 4), register);
            }
            else if (opcode == Opcodes.Abort)
            {
                Debug.Print("Abort has been called.");

                memory.Reserve(1, sp);
                memory.WriteByte(ReturnCodes.ABORT_CALLED, sp);

                MoveStackPointer(1);

                return false;
            }
            else if (opcode == Opcodes.StackAlloc)
            {
                byte bytes = NextProgramByte();
                byte[] bits = NextProgramBytes(bytes);

                int bytesToAlloc = ByteHelper.ToInt(bits);

                memory.Reserve(bytesToAlloc, sp);
            }
            else if (opcode == Opcodes.ZeroMemory)
            {
                byte bytes = NextProgramByte();
                byte[] bits = NextProgramBytes(bytes);

                int bytesToClear = ByteHelper.ToInt(bits);

                memory.Clear(sp - bytesToClear, sp);
            }
            else if (opcode == Opcodes.Load)
            {
                byte register = NextProgramByte();
                byte bytes = NextProgramByte();
                byte[] bits = NextProgramBytes(bytes);

                byte registerCapacity = Registers.RegisterSize(register);

                Debug.Assert(registerCapacity >= bytes);

                memory.WriteBytes(bits, register);
            }
            else if (opcode == Opcodes.Clear)
            {
                byte register = NextProgramByte();
                byte bytes = Registers.RegisterSize(register);

                memory.Clear(register, register + bytes);
            }
            else if (opcode == Opcodes.CopyStack_Direct)
            {
                byte register = NextProgramByte();
                byte bytes = NextProgramByte();
                byte addressBytes = NextProgramByte();
                byte[] addressBits = NextProgramBytes(bytes);

                byte registerCapacity = Registers.RegisterSize(register);

                Debug.Assert(registerCapacity >= bytes);

                int address = ByteHelper.ToInt(addressBits);

                // Copy value from the stack to given register.
                memory.WriteBytes(memory.ReadBytes(address, address + bytes), register);
            }
            else if (opcode == Opcodes.CopyStack_IndirectRegister)
            {
                byte bytes = NextProgramByte();
                byte addressRegister = NextProgramByte();
                byte targetRegister = NextProgramByte();

                byte targetRegisterCapacity = Registers.RegisterSize(targetRegister);
                byte addressRegisterCapacity = Registers.RegisterSize(addressRegister);

                byte[] addressBits = memory.ReadBytes(addressRegister, addressRegister + addressRegisterCapacity);

                int address = ByteHelper.ToInt(addressBits);

                Debug.Assert(address >= Registers.RegistersLowAddress && address < Registers.RegisterHighAddress);
                Debug.Assert(targetRegisterCapacity >= bytes);

                // Copy value from the stack to given register.
                byte[] bits = memory.ReadBytes(sp - bytes, sp);
                memory.WriteBytes(bits, targetRegister);
            }
            else if (opcode == Opcodes.PtrStack)
            {
                byte bytes = NextProgramByte();
                byte[] addressBits = NextProgramBytes(bytes);

                int address = ByteHelper.ToInt(addressBits);

                Debug.Assert(address >= Registers.RegisterHighAddress);

                byte valueBytes = NextProgramByte();
                byte[] valueBits = NextProgramBytes(valueBytes);

                memory.WriteBytes(valueBits, address);
            }
            else if (opcode == Opcodes.PtrStack_IndirectRegister)
            {
                byte register = NextProgramByte();
                byte bytes = NextProgramByte();
                byte[] bits = NextProgramBytes(bytes);

                byte registerCapacity = Registers.RegisterSize(register);

                int address = ByteHelper.ToInt(memory.ReadBytes(register, register + registerCapacity));

                Debug.Assert(address >= Registers.RegisterHighAddress);

                memory.WriteBytes(bits, address);
            }
            else if (opcode == Opcodes.GenerateArray_IndirectRegister)
            {
                byte lowAddressRegister = NextProgramByte();
                byte highAddressRegister = NextProgramByte();
                byte bytesRegister = NextProgramByte();
                byte elementSize = NextProgramByte();
                
                byte bytesRegisterCapacity = Registers.RegisterSize(bytesRegister);

                int count = ByteHelper.ToInt(memory.ReadBytes(bytesRegister, bytesRegister + bytesRegisterCapacity));
                int totalBytes = count * elementSize;

                memory.Reserve(totalBytes, sp);

                // Store high and low addresses.
                memory.WriteBytes(ByteHelper.ToBytes(sp, 4), lowAddressRegister);
                memory.WriteBytes(ByteHelper.ToBytes(sp + totalBytes, 4), highAddressRegister);

                MoveStackPointer(totalBytes);
            }
            else if (opcode == Opcodes.GenerateArray_IndirectStack)
            {
                byte elementsCountRegister = NextProgramByte();
                byte elementSize = NextProgramByte();

                byte elementsCountRegisterCapacity = Registers.RegisterSize(elementsCountRegister);

                int count = ByteHelper.ToInt(memory.ReadBytes(elementsCountRegister, elementsCountRegister + elementsCountRegisterCapacity));
                int totalBytes = count * elementSize;

                memory.Reserve(totalBytes, sp);

                int beg = sp;
                int end = sp + totalBytes;

                MoveStackPointer(totalBytes);

                memory.WriteBytes(ByteHelper.ToBytes(sp, 4), sp);
                MoveStackPointer(4);

                memory.WriteBytes(ByteHelper.ToBytes(sp + totalBytes, 4), sp);
                MoveStackPointer(4);
            }
            else if (opcode == Opcodes.Add_DirectStack)
            {
                byte aBytes = NextProgramByte();
                byte bBytes = NextProgramByte();

                byte[] aBits = memory.ReadBytes(sp - aBytes, sp);
                byte[] bBits = memory.ReadBytes(sp - aBytes - bBytes, sp - aBytes);

                byte[] result = ByteHelper.AddBytes(aBits, bBits);

                MoveStackPointer(-(aBytes + bBytes));

                memory.Reserve(aBytes, sp);
                memory.WriteBytes(result, sp);

                MoveStackPointer(aBytes);
            }
            else if (opcode == Opcodes.Add_IndirectRegister_Stack)
            {
                byte aRegister = NextProgramByte();
                byte bRegister = NextProgramByte();
                
                byte aBytes = Registers.RegisterSize(aRegister);
                byte bBytes = Registers.RegisterSize(bRegister);

                byte[] aBits = memory.ReadBytes(aRegister, aRegister + aBytes);
                byte[] bBits = memory.ReadBytes(bRegister, bRegister + bBytes);

                byte[] result = ByteHelper.AddBytes(aBits, bBits);

                memory.Reserve(aBytes, sp);
                memory.WriteBytes(result, sp);

                MoveStackPointer(aBytes);
            }
            else if (opcode == Opcodes.Add_IndirectRegister_Register)
            {
                byte aRegister = NextProgramByte();
                byte bRegister = NextProgramByte();
                byte rRegister = NextProgramByte();

                byte aBytes = Registers.RegisterSize(aRegister);
                byte bBytes = Registers.RegisterSize(bRegister);
                byte rBytes = Registers.RegisterSize(rRegister);

                byte[] aBits = memory.ReadBytes(aRegister, aRegister + aBytes);
                byte[] bBits = memory.ReadBytes(bRegister, bRegister + bBytes);

                byte[] result = ByteHelper.AddBytes(aBits, bBits);

                memory.WriteBytes(result, rRegister);
            }
            else if (opcode == Opcodes.Add_DirectStackRegister_Stack)
            {
                byte bytes = NextProgramByte();
                byte register = NextProgramByte();

                byte registerBytes = Registers.RegisterSize(register);

                byte[] aBits = memory.ReadBytes(sp - bytes, sp);
                byte[] bBits = memory.ReadBytes(register, register + registerBytes);
                MoveStackPointer(-bytes);

                byte[] result = ByteHelper.AddBytes(aBits, bBits);

                memory.Reserve(bytes, sp);
                memory.WriteBytes(result, sp);

                MoveStackPointer(bytes);
            }
            else if (opcode == Opcodes.Add_DirectStackRegister_Register)
            {
                // Get the size.
                byte bytes = program[pc + 1];
                byte register = program[pc + 3];

                // Dirty hack optimizations?
                // TODO: see if some of the add variants can be
                //       done the same way.
                opcode = Opcodes.Add_DirectStackRegister_Stack;
                InterpretOpcode(opcode);

                byte[] result = memory.ReadBytes(sp - bytes, sp);

                memory.WriteBytes(result, register);

                // Clear stack from earlier calls.
                memory.Clear(sp - bytes, bytes);

                MoveStackPointer(-bytes);
                
                // Before this, we are pointing to this opcodes last argument.
                MoveProgramCounter(1);
            }
            else if (opcode == Opcodes.Inc_Reg)
            {
                byte register = NextProgramByte();
                byte bytes = Registers.RegisterSize(register);

                byte[] result = ByteHelper.AddBytes(memory.ReadBytes(register, register + bytes), ByteHelper.GetOneByteArray(bytes));

                memory.WriteBytes(result, register);
            }
            else if (opcode == Opcodes.Inc_Stack)
            {
                byte bytes = NextProgramByte();

                byte[] result = ByteHelper.AddBytes(memory.ReadBytes(sp - bytes, sp), ByteHelper.GetOneByteArray(bytes));

                memory.WriteBytes(result, sp - bytes);
            }
            else if (opcode == Opcodes.Dec_Reg)
            {
                byte register = NextProgramByte();
                byte bytes = Registers.RegisterSize(register);

                byte[] result = ByteHelper.SubtractBytes(memory.ReadBytes(register, register + bytes), ByteHelper.GetOneByteArray(bytes));

                memory.WriteBytes(result, register);
            }
            else if (opcode == Opcodes.Dec_Stack)
            {
                byte bytes = NextProgramByte();

                byte[] result = ByteHelper.SubtractBytes(memory.ReadBytes(sp - bytes, sp), ByteHelper.GetOneByteArray(bytes));

                memory.WriteBytes(result, sp - bytes);
            }
            else if (opcode == Opcodes.Halt)
            {
                Console.WriteLine("Halt was called!");
            }
            else
            {
                memory.Reserve(1, sp);
                memory.WriteByte(ReturnCodes.PC_CORRUPTED_OR_NOT_OPCODE, sp);
                sp++;
                
                return false;
            }

            return true;
        }

        public void DumpStack()
        {
            FileHelper.DumpData("Stack dump", memory.ReadBytes(memory.LowAddress, memory.HighAddress - 1), 10, "stackdump.txt");
        }
        public void DumpProgram()
        {
            FileHelper.DumpData("Program dump", program, 10, "programdump.txt");
        }
        public void DumpRegisters()
        {
            FileHelper.DumpRegisters("Registers", memory.ReadBytes(0, 60), "registers.txt");
        }

        public byte[] ReadMemoryBytes(int lowAddress, int highAddress)
        {
            return memory.ReadBytes(lowAddress, highAddress);
        }
        public int ReadRegisterValue(byte register)
        {
            byte bytes = Registers.RegisterSize(register);

            byte[] bits = memory.ReadBytes(register, register + bytes);

            return ByteHelper.ToInt(bits);
        }

        public byte RunProgram(byte[] program)
         {
#if DEBUG
            try
            {
#endif
                Debug.Assert(program != null);

                this.program = program;

                bool running = true;

                while (running)
                {
                    running = InterpretOpcode(program[pc]);

                    MoveProgramCounter(1);

                    //if (!running) return stack.ReadByte(sp - 1);

                    if (pc >= program.Length) return ReturnCodes.DEFAULT_RET_CODE;
                }

                return memory.ReadByte(sp - 1);

#if DEBUG
            }
            catch (Exception e)
            {
                Console.WriteLine("Possible memory corruption, exception:\n\t " + e.Message);
                Console.WriteLine("PC: " + pc);
                Console.WriteLine("SP: " + sp);
                Console.WriteLine("Dumping stack, program memory and registers to root");

                DumpStack();
                DumpProgram();
                DumpRegisters();

                return ReturnCodes.DEBUG_EXCEPTION;
            }
#endif
        }
    }
}
