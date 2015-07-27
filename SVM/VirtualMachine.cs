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
        /// Cache memory.
        /// </summary>
        private readonly Caches caches;

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
                return memory.HighAddress - Registers.HighAddress + 1;
            }
        }
        public int StackLowAddress
        {
            get
            {
                return Registers.HighAddress + 1;
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
            memory = new MemoryManager(Sizes.CHUNK_16KB);
            
            // 8Kb should be enough for the cache
            // at this time.
            caches = new Caches(Sizes.CHUNK_8KB);
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
        private byte[] NextProgramBytes(int bytes, int offset)
        {
            byte[] cache = caches.GetCacheOfSize(bytes, offset);
            int j = 0;

            do
            {
                pc++;

                cache[j] = program[pc];
                
                j++;

            } while (j < bytes);

            return cache;
        }

        /// <summary>
        /// Moves the stack pointer by given offset.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void MoveStackPointer(int bytes)
        {
            sp += bytes;
        }

        #endregion

        /// <summary>
        /// Should be called before any programs are executed.
        /// Resets the state of registers and memory of
        /// the machine.
        /// </summary>
        public void Initialize()
        {
            memory.Clear();
            caches.ClearCaches();

            // Stack pointer should start from register 
            // high address since registers live below this 
            // address.
            sp = Registers.HighAddress + 1;

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
                byte size = NextProgramByte();

                Debug.Assert(size <= Sizes.DWORD);

                // Read the variable values.
                byte[] bits = NextProgramBytes(size, 0);

                // Reserve room for the bytes and write them to the stack.
                memory.Reserve(size, sp);
                memory.WriteBytes(sp, sp + size, bits);

                MoveStackPointer(size);
            }
            else if (opcode == Opcodes.Push_Register)
            {
                // Read register address and get its size.
                byte register = NextProgramByte();
                byte registerCapacity = Registers.RegisterSize(register);

                Debug.Assert(registerCapacity <= Sizes.DWORD);

                // Reserve memory for next push operation.
                memory.Reserve(registerCapacity, sp);

                // Get reusable buffer to store the bytes.
                byte[] cache = caches.GetCacheOfSize(registerCapacity, 0);

                // Copy memory to cache.
                memory.ReadBytes(register, register + registerCapacity, cache);

                // Get the bytes and write them to the stack.
                memory.WriteBytes(sp, sp + registerCapacity, cache);

                MoveStackPointer(registerCapacity);
            }
            else if (opcode == Opcodes.Pop)
            {
                Debug.Assert(sp > Registers.HighAddress);

                // Get count of bytes to pop.
                byte bytes = NextProgramByte();

                // Everything after this address is trash.
                MoveStackPointer(-(bytes + 1));
            }
            else if (opcode == Opcodes.Top)
            {
                byte size = NextProgramByte();
                byte register = NextProgramByte();
                byte registerCapacity = Registers.RegisterSize(register);

                // Get cache.
                byte[] cache = caches.GetCacheOfSize(size, 0);

                // Read bits from the stack.
                memory.ReadBytes(sp - size, sp, cache);

                Debug.Assert(registerCapacity >= size);

                // Copy top of the stack to given register.
                memory.WriteBytes(register, register + size, cache);
            }
            else if (opcode == Opcodes.Sp)
            {
                byte register = NextProgramByte();
                byte registerCapacity = Registers.RegisterSize(register);

                // Check that the stack pointer fits to this register.
                Debug.Assert(registerCapacity >= 4);

                // Get cache to store results.
                byte[] cache = caches.GetCacheOfSize(Sizes.LWORD, 0);
                ByteHelper.ToBytes(sp, cache);

                memory.WriteBytes(register, register + 4, cache);
            }
            else if (opcode == Opcodes.Abort)
            {
                Console.WriteLine("Abort has been called");
                
                memory.Reserve(1, sp);
                memory.WriteByte(sp, ReturnCodes.ABORT_CALLED);

                MoveStackPointer(1);

                return false;
            }
            else if (opcode == Opcodes.StackAlloc)
            {
                byte size = NextProgramByte();
                byte[] bytes = NextProgramBytes(size, 0);

                int bytesToAlloc = ByteHelper.ToInt(bytes);

                memory.Reserve(bytesToAlloc, sp);
            }
            else if (opcode == Opcodes.ZeroMemory)
            {
                byte size = NextProgramByte();
                byte[] bytes = NextProgramBytes(size, 0);

                int bytesToClear = ByteHelper.ToInt(bytes);

                memory.Clear(sp - bytesToClear, sp);
            }
            else if (opcode == Opcodes.Load)
            {
                byte register = NextProgramByte();
                byte size = NextProgramByte();
                byte[] bytes = NextProgramBytes(size, 0);

                byte registerCapacity = Registers.RegisterSize(register);

                Debug.Assert(registerCapacity >= size);

                memory.WriteBytes(register, register + size, bytes);
            }
            else if (opcode == Opcodes.Clear)
            {
                byte register = NextProgramByte();
                byte registerCapacity = Registers.RegisterSize(register);

                memory.Clear(register, register + registerCapacity);
            }
            else if (opcode == Opcodes.CopyStack_Direct)
            {
                byte register = NextProgramByte();
                byte valueSize = NextProgramByte();
                byte addressSize = NextProgramByte();
                byte[] addressBytes = NextProgramBytes(valueSize, 0);

                byte registerCapacity = Registers.RegisterSize(register);

                Debug.Assert(registerCapacity >= valueSize);

                int address = ByteHelper.ToInt(addressBytes);

                // Add address offset since next address bits could 
                // point to the same memory area.
                byte[] cache = caches.GetCacheOfSize(valueSize, 1);

                // Copy memory to given cache.
                memory.ReadBytes(address, address + valueSize, cache);

                // Copy value from the stack to given register.
                memory.WriteBytes(register, register + registerCapacity, cache);
            }
            else if (opcode == Opcodes.CopyStack_IndirectRegister)
            {
                byte size = NextProgramByte();
                byte addressRegister = NextProgramByte();
                byte targetRegister = NextProgramByte();

                byte targetRegisterCapacity = Registers.RegisterSize(targetRegister);
                byte addressRegisterCapacity = Registers.RegisterSize(addressRegister);

                byte[] addressBytes = caches.GetCacheOfSize(size, 0);
                byte[] valueBytes = caches.GetCacheOfSize(size, 1);

                // Read address bits to given cache.
                memory.ReadBytes(addressRegister, addressRegister + addressRegisterCapacity, addressBytes);

                int address = ByteHelper.ToInt(addressBytes);

                Debug.Assert(address >= Registers.LowAddress && address < Registers.HighAddress);
                Debug.Assert(targetRegisterCapacity >= size);

                // Copy value from the stack to given register.
                memory.ReadBytes(sp - size, sp, valueBytes);
                memory.WriteBytes(targetRegister, targetRegister + targetRegisterCapacity, valueBytes);
            }
            else if (opcode == Opcodes.PtrStack)
            {
                // ptrstack [address_size] [address] [value_size] [value]
                byte addressSize = NextProgramByte();
                byte[] addressBytes = NextProgramBytes(addressSize, 0);

                byte valueSize = NextProgramByte();
                byte[] valueBytes = NextProgramBytes(valueSize, 0);

                int address = ByteHelper.ToInt(addressBytes);

                // TODO: could point anywhere in release build... Make this staph!
                Debug.Assert(address >= Registers.HighAddress);

                memory.WriteBytes(address, address + addressBytes.Length, valueBytes);
            }
            else if (opcode == Opcodes.PtrStack_IndirectRegister)
            {
                byte register = NextProgramByte();
                byte size = NextProgramByte();
                byte[] bytes = NextProgramBytes(size, 0);

                byte registerCapacity = Registers.RegisterSize(register);

                // Copy address from the memory to given cache.
                byte[] cache = caches.GetCacheOfSize(size, 1);
                memory.ReadBytes(register, register + registerCapacity, cache);

                int address = ByteHelper.ToInt(bytes);

                // TODO: could point anywhere... Make this staph!

                Debug.Assert(address >= Registers.HighAddress);

                memory.WriteBytes(address, address + bytes.Length, cache);
            }
            else if (opcode == Opcodes.GenerateArray_IndirectRegister)
            {
                byte lowAddressRegister = NextProgramByte();
                byte highAddressRegister = NextProgramByte();

                byte bytesRegister = NextProgramByte();
                byte elementSize = NextProgramByte();
                
                byte bytesRegisterCapacity = Registers.RegisterSize(bytesRegister);

                // Copy elements count to cache.
                byte[] cache = caches.GetCacheOfSize(bytesRegisterCapacity, 0);
                memory.ReadBytes(bytesRegister, bytesRegister + bytesRegisterCapacity, cache);

                int count = ByteHelper.ToInt(cache);
                int totalBytes = count * elementSize;

                memory.Reserve(totalBytes, sp);

                // Store high and low addresses.
                ByteHelper.ToBytes(sp, cache);
                memory.WriteBytes(lowAddressRegister, Registers.RegisterSize(lowAddressRegister), cache);

                ByteHelper.ToBytes(sp + totalBytes, cache);
                memory.WriteBytes(highAddressRegister, highAddressRegister + Registers.RegisterSize(highAddressRegister), cache);

                MoveStackPointer(totalBytes);
            }
            else if (opcode == Opcodes.GenerateArray_IndirectStack)
            {
                byte elementsCountRegister = NextProgramByte();
                byte elementSize = NextProgramByte();

                byte elementsCountRegisterCapacity = Registers.RegisterSize(elementsCountRegister);

                // Copy elements count to cache.
                byte[] cache = caches.GetCacheOfSize(elementsCountRegisterCapacity, 0);
                memory.ReadBytes(elementsCountRegister, elementsCountRegister + elementsCountRegisterCapacity, cache);

                int count = ByteHelper.ToInt(cache);
                int totalBytes = count * elementSize;

                memory.Reserve(totalBytes, sp);

                int beg = sp;
                int end = sp + totalBytes;

                MoveStackPointer(totalBytes);

                // Get bytes and write them to the memory.
                ByteHelper.ToBytes(sp, cache);
                memory.WriteBytes(sp, sp + 4, cache);
                MoveStackPointer(4);

                ByteHelper.ToBytes(sp + totalBytes, cache);
                memory.WriteBytes(sp, sp + 4, cache);
                MoveStackPointer(4);
            }
            else if (opcode == Opcodes.Add_DirectStack)
            {
                byte aSize = NextProgramByte();
                byte bSize = NextProgramByte();

                byte[] aCache = caches.GetCacheOfSize(aSize, 0);
                byte[] bCache = caches.GetCacheOfSize(bSize, 1);
                byte[] rCache = caches.GetCacheOfSize(aSize, 2);

                memory.ReadBytes(sp - aSize, sp, aCache);
                memory.ReadBytes(sp - aSize - bSize, sp - aSize, bCache);

                ByteHelper.AddBytes(aCache, bCache, rCache);

                MoveStackPointer(-(aSize + bSize));

                memory.Reserve(aSize, sp);
                memory.WriteBytes(sp, sp + aSize, rCache);

                MoveStackPointer(aSize);
            }
            else if (opcode == Opcodes.Add_IndirectRegister_Stack)
            {
                byte aRegister = NextProgramByte();
                byte bRegister = NextProgramByte();
                
                byte aRegisterCapacity = Registers.RegisterSize(aRegister);
                byte bRegisterCapacity = Registers.RegisterSize(bRegister);

                byte[] aCache = caches.GetCacheOfSize(aRegisterCapacity, 0);
                byte[] bCache = caches.GetCacheOfSize(bRegisterCapacity, 1);
                byte[] rCache = caches.GetCacheOfSize(aRegisterCapacity, 2);

                memory.ReadBytes(aRegister, aRegister + aRegisterCapacity, aCache);
                memory.ReadBytes(bRegister, bRegister + bRegisterCapacity, bCache);

                ByteHelper.AddBytes(aCache, bCache, rCache);

                memory.Reserve(aRegisterCapacity, sp);
                memory.WriteBytes(sp, sp + aRegisterCapacity, rCache);

                MoveStackPointer(aRegisterCapacity);
            }
            else if (opcode == Opcodes.Add_IndirectRegister_Register)
            {
                byte aRegister = NextProgramByte();
                byte bRegister = NextProgramByte();
                byte rRegister = NextProgramByte();

                byte aRegisterCapacity = Registers.RegisterSize(aRegister);
                byte bRegisterCapacity = Registers.RegisterSize(bRegister);
                byte rRegisterCapacity = Registers.RegisterSize(rRegister);

                // Just get the main cache for the result, this should 
                // not be in use at this point.
                byte[] aCache = caches.GetCacheOfSize(aRegisterCapacity, 0);
                byte[] bCache = caches.GetCacheOfSize(bRegisterCapacity, 1);
                byte[] rCache = caches.GetCache(Caches.M_CACHE);

                // Copy data to caches.
                memory.ReadBytes(aRegister, aRegister + aRegisterCapacity, aCache);
                memory.ReadBytes(bRegister, bRegister + bRegisterCapacity, bCache);
                memory.ReadBytes(rRegister, rRegister + rRegisterCapacity, rCache);

                ByteHelper.AddBytes(aCache, bCache, rCache);

                memory.WriteBytes(rRegister, rRegister + rRegisterCapacity, rCache);
            }
            else if (opcode == Opcodes.Add_DirectStackRegister_Stack)
            {
                byte size = NextProgramByte();
                byte register = NextProgramByte();

                byte registerCapacity = Registers.RegisterSize(register);

                // Copy data to caches.
                byte[] aCache = caches.GetCacheOfSize(size, 0);
                byte[] bCache = caches.GetCacheOfSize(size, 1);
                byte[] rCache = caches.GetCacheOfSize(size, 2);

                memory.ReadBytes(sp - size, sp, aCache);
                memory.ReadBytes(register, register + registerCapacity, bCache);

                MoveStackPointer(-size);

                ByteHelper.AddBytes(aCache, bCache, rCache);

                memory.Reserve(size, sp);
                memory.WriteBytes(sp, sp + size, rCache);

                MoveStackPointer(size);
            }
            else if (opcode == Opcodes.Add_DirectStackRegister_Register)
            {
                // Get the size.
                byte size = program[pc + 1];
                byte register = program[pc + 3];

                // Dirty hack optimizations?
                // TODO: see if some of the add variants can be
                //       done the same way.
                opcode = Opcodes.Add_DirectStackRegister_Stack;
                InterpretOpcode(opcode);

                // Copy the result to given cache.
                byte[] cache = caches.GetCacheOfSize(size, 0);
                memory.ReadBytes(sp - size, sp, cache);

                memory.WriteBytes(register, register + size, cache);

                // Clear stack from earlier calls.
                memory.Clear(sp - size, size);

                MoveStackPointer(-size);
                
                // Before this, we are pointing to this opcodes last argument.
                MoveProgramCounter(1);
            }
            else if (opcode == Opcodes.Inc_Reg)
            {
                byte register = NextProgramByte();
                byte size = Registers.RegisterSize(register);

                byte[] cache = caches.GetCacheOfSize(size, 0);
                byte[] rCache = caches.GetCacheOfSize(size, 1);

                memory.ReadBytes(register, register + size, cache);

                ByteHelper.AddBytes(cache, ByteHelper.GetOneByteArray(size), rCache);

                memory.WriteBytes(register, register + size, rCache);


                Console.WriteLine(ByteHelper.ToInt(rCache));
            }
            else if (opcode == Opcodes.Inc_Stack)
            {
                byte size = NextProgramByte();

                byte[] cache = caches.GetCacheOfSize(size, 0);
                byte[] rCache = caches.GetCacheOfSize(size, 1);

                memory.ReadBytes(sp - size, sp, cache);

                ByteHelper.AddBytes(cache, ByteHelper.GetOneByteArray(size), rCache);

                memory.WriteBytes(sp - size, sp, rCache);
            }
            else if (opcode == Opcodes.Dec_Reg)
            {
                byte register = NextProgramByte();
                byte size = Registers.RegisterSize(register);

                byte[] cache = caches.GetCacheOfSize(size, 0);
                byte[] rCache = caches.GetCacheOfSize(size, 1);

                memory.ReadBytes(register, register + size, cache);

                ByteHelper.SubtractBytes(cache, ByteHelper.GetOneByteArray(size), rCache);

                memory.WriteBytes(register, register + size, rCache);
            }
            else if (opcode == Opcodes.Dec_Stack)
            {
                byte size = NextProgramByte();

                byte[] cache = caches.GetCacheOfSize(size, 0);
                byte[] rCache = caches.GetCacheOfSize(size, 1);

                memory.ReadBytes(sp - size, sp, cache);

                ByteHelper.SubtractBytes(cache, ByteHelper.GetOneByteArray(size), rCache);

                memory.WriteBytes(sp - size, sp, rCache);
            }
            else if (opcode == Opcodes.Halt)
            {
                Console.WriteLine("Halt was called");
            }
            else
            {
                memory.Reserve(1, sp);
                memory.WriteByte(sp, ReturnCodes.PC_CORRUPTED_OR_NOT_OPCODE);

                sp++;
                
                return false;
            }

            return true;
        }

        public void DumpStack()
        {
            byte[] buffer = new byte[memory.HighAddress - 1];

            memory.ReadBytes(memory.LowAddress, memory.HighAddress, buffer);

            FileHelper.DumpData("Stack dump", buffer, 10, "stackdump.txt");
        }
        public void DumpProgram()
        {
            FileHelper.DumpData("Program dump", program, 10, "programdump.txt");
        }
        public void DumpRegisters()
        {
            byte[] buffer = new byte[Registers.HighAddress];

            memory.ReadBytes(Registers.LowAddress, Registers.HighAddress, buffer);

            FileHelper.DumpRegisters("Registers", buffer, "registers.txt");
        }

        public byte[] ReadMemoryBytes(int lowAddress, int highAddress)
        {
            byte[] buffer = new byte[highAddress - lowAddress];

            memory.ReadBytes(lowAddress, highAddress, buffer);

            return buffer;
        }
        public int ReadRegisterValue(byte register)
        {
            byte registerCapacity = Registers.RegisterSize(register);

            byte[] buffer = new byte[registerCapacity];

            memory.ReadBytes(register, register + registerCapacity, buffer);

            return ByteHelper.ToInt(buffer);
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

                return Returs.DEBUG_EXCEPTION;
            }
#endif
         }
    }
}
