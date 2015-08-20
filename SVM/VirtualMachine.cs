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
        private readonly CacheManager caches;

        /// <summary>
        /// Collection of supported math functions. Used with math opcodes.
        /// </summary>
        private readonly Action<byte[], byte[], byte[]>[] arithmeticFunctions;

        /// <summary>
        /// Stack pointer.
        /// </summary>
        private int sp;

        /// <summary>
        /// Program counter.
        /// </summary>
        private int pc;

        private bool running;
        #endregion

        #region Properties
        public int StackSize
        {
            get
            {
                return memory.HighAddress - Registers.HighAddress;
            }
        }
        public int StackLowAddress
        {
            get
            {
                return Registers.HighAddress;
            }
        }
        public int StackHighAddress
        {
            get
            {
                return sp;
            }
        }

        public int ProgramCounter
        {
            get
            {
                return pc;
            }
        }
        #endregion

        public VirtualMachine()
        {
            // 64Kb should be enough memory for the 
            // starters.
            memory = new MemoryManager(Sizes.CHUNK_32KB);
            
            // 8Kb should be enough for the cache
            // at this time.
            caches = new CacheManager(Sizes.CHUNK_8KB);

            arithmeticFunctions = new Action<byte[], byte[], byte[]>[]
            {
                // Int.
                ByteHelper.AddIntBytes,
                ByteHelper.SubtractIntBytes,
                ByteHelper.DivideIntBytes,
                ByteHelper.MultiplyIntBytes,
                ByteHelper.ModuloFromIntBytes,

                // Float.
                ByteHelper.AddFloatBytes,
                ByteHelper.SubtractFloatBytes,
                ByteHelper.DivideFloatBytes,
                ByteHelper.MultiplyFloatBytes,
                ByteHelper.ModuloFromFloatBytes
            };
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

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void ZeroOperation(Func<int, bool> condition)
        {
            byte stackSize = NextProgramByte();
            byte addressSize = NextProgramByte();
            byte[] addressBytes = NextProgramBytes(addressSize, 0);

            byte[] cache = caches.GetCacheOfSize(stackSize, 1);

            // Validation.
            if (!IsValidWordSize(stackSize)) return;
            if (!IsValidWordSize(addressSize)) return;

            // Read value from stack.
            memory.ReadBytes(sp - stackSize, sp, cache);

            int value = ByteHelper.ToInt(cache);

            // Do testing.
            if (condition(value))
            {
                // Set pc to given address.
                int address = ByteHelper.ToInt(addressBytes);

                if (address > program.Length || address < 0)
                {
                    Exit(ReturnCodes.INVALID_JUMP_ADDRESS);

                    return;
                }

                pc = address - 1;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void EqOperation(Func<int, int, bool> condition)
        {
            byte aSize = NextProgramByte();
            byte bSize = NextProgramByte();
            byte addressSize = NextProgramByte();
            byte[] addressBytes = NextProgramBytes(addressSize, 0);

            // Validation.
            if (!IsValidWordSize(aSize)) return;
            if (!IsValidWordSize(bSize)) return;
            if (!IsValidWordSize(addressSize)) return;

            byte[] aCache = caches.GetCacheOfSize(aSize, 1);
            byte[] bCache = caches.GetCacheOfSize(bSize, 2);

            memory.ReadBytes(sp - bSize - aSize, sp - bSize, aCache);
            memory.ReadBytes(sp - bSize, sp, bCache);

            int a = ByteHelper.ToInt(aCache);
            int b = ByteHelper.ToInt(bCache);

            if (condition(a, b))
            {
                int address = ByteHelper.ToInt(addressBytes);

                if (address > program.Length || address < 0)
                {
                    Exit(ReturnCodes.INVALID_JUMP_ADDRESS);

                    return;
                }

                pc = address - 1;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void Print(byte[] flags, byte[] values, int count)
        {
            if (flags[0] == Flags.STR)
            {
                string str = Convert.ToBase64String(values, 0, count);

                Console.WriteLine(str);
            }
            else if (flags[0] == Flags.INT)
            {
                byte[] valueCache = caches.GetCacheOfSize(count, 2);

                Array.Copy(values, 0, valueCache, 0, valueCache.Length);

                int value = ByteHelper.ToInt(valueCache);

                Console.WriteLine(value);
            }
            else
            {
                // Exit.
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private bool IsValidWordSize(int size)
        {
            if (size > Sizes.LWORD || size == 0)
            {
                Exit(ReturnCodes.INVALID_WORD_SIZE);

                return false;
            }

            return true;
        }

        private void Exit(byte exitCode)
        {
            running = false;

            memory.WriteByte(sp, exitCode);

            sp++;
        }

        private void InterpretBytecode(byte bytecode)
        {
            // Code execution steps
            //      1) Read bytes from program memory
            //      2) Validation
            //      3) Get caches
            //      4) Execute

            #region Stack operations

            if (bytecode == Bytecodes.Push_Direct)
            {
                // Get size of the variable in bytes.
                byte size = NextProgramByte();
                byte[] bytes = NextProgramBytes(size, 0);

                if (!IsValidWordSize(size)) return;

                memory.WriteBytes(sp, sp + size, bytes);

                MoveStackPointer(size);
            }
            else if (bytecode == Bytecodes.Push_Register)
            {
                // Read register address and get its size.
                byte register = NextProgramByte();
                byte registerCapacity = Registers.RegisterCapacity(register);

                // Get reusable buffer to store the bytes.
                byte[] cache = caches.GetCacheOfSize(registerCapacity, 0);

                // Copy memory to cache.
                memory.ReadBytes(register, register + registerCapacity, cache);

                // Get the bytes and write them to the stack.
                memory.WriteBytes(sp, sp + registerCapacity, cache);

                MoveStackPointer(registerCapacity);
            }
            else if (bytecode == Bytecodes.Pop)
            {
                // Get count of bytes to pop.
                byte bytes = NextProgramByte();

                if (sp - bytes <= StackLowAddress)
                {
                    Exit(ReturnCodes.STACK_UNDERFLOW);

                    return;
                }

                // Everything after this address is trash.
                MoveStackPointer(-(bytes + 1));
            }
            else if (bytecode == Bytecodes.Top)
            {
                byte size = NextProgramByte();
                byte register = NextProgramByte();
                byte registerCapacity = Registers.RegisterCapacity(register);

                // Validation.
                if (!IsValidWordSize(size)) return;

                if (size > registerCapacity)
                {
                    Exit(ReturnCodes.REGISTER_OVERFLOW);

                    return;
                }

                // Get cache.
                byte[] cache = caches.GetCacheOfSize(size, 0);

                // Read bytes from the stack.
                memory.ReadBytes(sp - size, sp, cache);

                // Copy top of the stack to given register.
                memory.WriteBytes(register, register + size, cache);
            }
            else if (bytecode == Bytecodes.Sp)
            {
                byte register = NextProgramByte();
                byte registerCapacity = Registers.RegisterCapacity(register);

                // Check that the stack pointer fits to this register.
                if (registerCapacity < 4)
                {
                    Exit(ReturnCodes.REGISTER_OVERFLOW);

                    return;
                }

                // Get cache to store results.
                byte[] cache = caches.GetCacheOfSize(Sizes.LWORD, 0);
                ByteHelper.ToBytes(sp, cache);

                memory.WriteBytes(register, register + 4, cache);
            }
            else if (bytecode == Bytecodes.Push_Bytes)
            {
                byte elementSize = NextProgramByte();
                byte elementsCountSize = NextProgramByte();
                byte[] elementsCountBytes = NextProgramBytes(elementsCountSize, 0);

                // Validation.
                if (!IsValidWordSize(elementSize)) return;
                if (!IsValidWordSize(elementsCountSize)) return;

                // Get elements count.
                int elementsCount = ByteHelper.ToInt(elementsCountBytes);

                // Read es * ews amount of bytes from program memory.
                byte[] bytes = NextProgramBytes(elementSize * elementsCount, 1);

                // Write to stack.
                memory.WriteBytes(sp, sp + bytes.Length, bytes);
            }

            #endregion

            #region Register operations

            else if (bytecode == Bytecodes.Load)
            {
                byte register = NextProgramByte();
                byte size = NextProgramByte();
                byte[] bytes = NextProgramBytes(size, 0);

                byte registerCapacity = Registers.RegisterCapacity(register);

                // Validation.
                if (!IsValidWordSize(size)) return;

                if (size > registerCapacity)
                {
                    Exit(ReturnCodes.REGISTER_OVERFLOW);

                    return;
                }

                memory.WriteBytes(register, register + size, bytes);
            }
            else if (bytecode == Bytecodes.Load_Direct)
            {
                byte register = NextProgramByte();
                byte registerCapacity = Registers.RegisterCapacity(register);
                byte[] bytes = NextProgramBytes(registerCapacity, 0);

                if (registerCapacity != bytes.Length)
                {
                    Exit(ReturnCodes.REGISTER_OVERFLOW);

                    return;
                }

                memory.WriteBytes(register, register + bytes.Length, bytes);
            }
            else if (bytecode == Bytecodes.CopyStack)
            {
                byte register = NextProgramByte();
                byte valueSize = NextProgramByte();
                byte addressSize = NextProgramByte();
                byte[] addressBytes = NextProgramBytes(valueSize, 0);
                byte registerCapacity = Registers.RegisterCapacity(register);

                // Validation.
                if (!IsValidWordSize(valueSize)) return;
                if (!IsValidWordSize(addressSize)) return;

                if (registerCapacity < valueSize)
                {
                    Exit(ReturnCodes.REGISTER_OVERFLOW);

                    return;
                }

                int address = ByteHelper.ToInt(addressBytes);

                // Add address offset since next address bytes could 
                // point to the same memory area.
                byte[] cache = caches.GetCacheOfSize(valueSize, 1);

                // Copy memory to given cache.
                memory.ReadBytes(address, address + valueSize, cache);

                // Copy value from the stack to given register.
                memory.WriteBytes(register, register + registerCapacity, cache);
            }
            else if (bytecode == Bytecodes.CopyStack_Register)
            {
                byte size = NextProgramByte();
                byte addressRegister = NextProgramByte();
                byte targetRegister = NextProgramByte();

                byte targetRegisterCapacity = Registers.RegisterCapacity(targetRegister);
                byte addressRegisterCapacity = Registers.RegisterCapacity(addressRegister);

                byte[] addressBytes = caches.GetCacheOfSize(size, 0);
                byte[] valueBytes = caches.GetCacheOfSize(size, 1);

                // Read address bytes to given cache.
                memory.ReadBytes(addressRegister, addressRegister + addressRegisterCapacity, addressBytes);

                int address = ByteHelper.ToInt(addressBytes);

                // Validation.
                if (!IsValidWordSize(size)) return;
                
                if (address < Registers.LowAddress || address > Registers.HighAddress)
                {
                    Exit(ReturnCodes.INVALID_REGISTER_ADDRESS);

                    return;
                }
                if (targetRegisterCapacity < size)
                {
                    Exit(ReturnCodes.REGISTER_OVERFLOW);

                    return;
                }

                // Copy value from the stack to given register.
                memory.ReadBytes(sp - size, sp, valueBytes);
                memory.WriteBytes(targetRegister, targetRegister + targetRegisterCapacity, valueBytes);
            }
            else if (bytecode == Bytecodes.Clear)
            {
                byte register = NextProgramByte();
                byte registerCapacity = Registers.RegisterCapacity(register);

                memory.Clear(register, register + registerCapacity);
            }

            #endregion

            #region Flow operations

            else if (bytecode == Bytecodes.Abort)
            {
                Exit(ReturnCodes.ABORT_CALLED);
            }
            else if (bytecode == Bytecodes.Jez)
            {
                ZeroOperation(i => i == 0);
            }
            else if (bytecode == Bytecodes.Jlz)
            {
                ZeroOperation(i => i < 0);
            }
            else if (bytecode == Bytecodes.Jgz)
            {
                ZeroOperation(i => i > 0);
            }
            else if (bytecode == Bytecodes.Jeq)
            {
                EqOperation((a, b) => a == b);
            }
            else if (bytecode == Bytecodes.Jneq)
            {
                EqOperation((a, b) => a != b);
            }
            else if (bytecode == Bytecodes.Jmp)
            {
                byte addressSize = NextProgramByte();
                byte[] addressBytes = NextProgramBytes(addressSize, 0);

                int address = ByteHelper.ToInt(addressBytes);

                // Validation.
                if (!IsValidWordSize(addressSize)) return;

                if (address > program.Length || address < 0)
                {
                    Exit(ReturnCodes.INVALID_JUMP_ADDRESS);

                    return;
                }

                // One will be reduced from all jump addresses
                // since we increase our pc by one after opcode
                // has been executed.
                pc = address - 1;
            }
            else if (bytecode == Bytecodes.Jmp_Stack)
            {
                byte size = NextProgramByte();
                byte[] cache = caches.GetCacheOfSize(size, 0);

                memory.ReadBytes(sp - size, sp, cache);

                int address = ByteHelper.ToInt(cache);

                // Validation.
                if (!IsValidWordSize(size)) return;

                if (address > program.Length || address < 0)
                {
                    Exit(ReturnCodes.INVALID_JUMP_ADDRESS);

                    return;
                }

                pc = address - 1;
            }
            else if (bytecode == Bytecodes.Nop)
            {
                // No operation.
            }
            #endregion

            #region Memory operations

            else if (bytecode == Bytecodes.ZeroMemory)
            {
                byte size = NextProgramByte();
                byte[] bytes = NextProgramBytes(size, 0);

                int bytesToClear = ByteHelper.ToInt(bytes);

                // Validation.
                if (!IsValidWordSize(size)) return;

                if (sp - bytesToClear < StackLowAddress)
                {
                    Exit(ReturnCodes.STACK_UNDERFLOW);

                    return;
                }

                memory.Clear(sp - bytesToClear, sp);
            }
            else if (bytecode == Bytecodes.PtrStack)
            {
                // ptrstack [address_size] [address] [value_size] [value]
                byte addressSize = NextProgramByte();
                byte[] addressBytes = NextProgramBytes(addressSize, 0);

                byte valueSize = NextProgramByte();
                byte[] valueBytes = NextProgramBytes(valueSize, 0);

                int address = ByteHelper.ToInt(addressBytes);

                // Validation.
                if (!IsValidWordSize(addressSize)) return;
                if (!IsValidWordSize(valueSize)) return;

                if (address <= StackLowAddress)
                {
                    Exit(ReturnCodes.ACCESSING_PROTECTED_MEMORY);

                    return;
                }

                memory.WriteBytes(address, address + addressBytes.Length, valueBytes);
            }
            else if (bytecode == Bytecodes.PtrStack_Register)
            {
                byte register = NextProgramByte();
                byte size = NextProgramByte();
                byte[] bytes = NextProgramBytes(size, 0);

                byte registerCapacity = Registers.RegisterCapacity(register);

                // Copy address from the memory to given cache.
                byte[] cache = caches.GetCacheOfSize(size, 1);
                memory.ReadBytes(register, register + registerCapacity, cache);

                int address = ByteHelper.ToInt(bytes);

                // Validation.
                if (!IsValidWordSize(size)) return;

                if (address <= StackLowAddress)
                {
                    Exit(ReturnCodes.ACCESSING_PROTECTED_MEMORY);

                    return;
                }

                memory.WriteBytes(address, address + bytes.Length, cache);
            }

            #endregion

            #region Flag operations

            else if (bytecode == Bytecodes.Set_Flag_Direct)
            {
                byte value = NextProgramByte();

                memory.WriteByte(Registers.RFLAGS, value);
            }
            else if (bytecode == Bytecodes.Set_Flag_Direct)
            {
                byte register = NextProgramByte();
                byte value = memory.ReadByte(register);

                memory.WriteByte(Registers.RFLAGS, value);
            }
            else if (bytecode == Bytecodes.Set_Flag_Direct)
            {
                byte value = memory.ReadByte(sp);

                memory.WriteByte(Registers.RFLAGS, value);
            }
            else if (bytecode == Bytecodes.Print)
            {
                byte size = NextProgramByte();
                byte[] bytesCountBytes = NextProgramBytes(size, 0);

                // Validation.
                if (!IsValidWordSize(size)) return;

                int bytesCount = ByteHelper.ToInt(bytesCountBytes);

                byte[] bytesCache = caches.GetCache(CacheManager.M_CACHE);
                byte[] flagsCache = caches.GetCacheOfSize(1, 1);

                memory.ReadBytes(sp - bytesCount, sp, bytesCache);
                memory.ReadBytes(Registers.RFLAGS, Registers.RFLAGS + 1, flagsCache);

                Print(flagsCache, bytesCache, bytesCount);
            }
            else if(bytecode == Bytecodes.Print_Offset) 
            {
                byte addressSize = NextProgramByte();
                byte[] lowAddressBytes = NextProgramBytes(addressSize, 0);
                byte[] highAddressBytes = NextProgramBytes(addressSize, 1);

                byte bytesCountSize = NextProgramByte();
                byte[] bytesCountBytes = NextProgramBytes(bytesCountSize, 2);

                // Validation.
                if (!IsValidWordSize(addressSize)) return;
                if (!IsValidWordSize(bytesCountSize)) return;

                byte[] bytesCache = caches.GetCache(CacheManager.M_CACHE);
                byte[] flagsCache = caches.GetCacheOfSize(1, 3);

                int lowAddress = ByteHelper.ToInt(lowAddressBytes);
                int highAddress = ByteHelper.ToInt(highAddressBytes);
                int bytesCount = ByteHelper.ToInt(bytesCountBytes);

                memory.ReadBytes(lowAddress, highAddress, bytesCache);
                memory.ReadBytes(Registers.RFLAGS, Registers.RFLAGS + 1, flagsCache);

                Print(flagsCache, bytesCache, bytesCount);
            }

            #endregion

            #region Arithmetic operations

            else if (bytecode == Bytecodes.Arithmetic_Stack)
            {
                byte aSize = NextProgramByte();
                byte bSize = NextProgramByte();

                // Validation.
                if (!IsValidWordSize(aSize)) return;
                if (!IsValidWordSize(bSize)) return;

                byte[] aCache = caches.GetCacheOfSize(aSize, 0);
                byte[] bCache = caches.GetCacheOfSize(bSize, 1);
                byte[] rCache = caches.GetCacheOfSize(aSize, 2);

                memory.ReadBytes(sp - aSize, sp, aCache);
                memory.ReadBytes(sp - aSize - bSize, sp - aSize, bCache);

                arithmeticFunctions[memory.ReadByte(Registers.RFLAGS)](aCache, bCache, rCache);

                MoveStackPointer(-(aSize + bSize));

                memory.WriteBytes(sp, sp + aSize, rCache);

                MoveStackPointer(aSize);
            }
            else if (bytecode == Bytecodes.Arithmetic_Register)
            {
                byte aRegister = NextProgramByte();
                byte bRegister = NextProgramByte();

                byte aRegisterCapacity = Registers.RegisterCapacity(aRegister);
                byte bRegisterCapacity = Registers.RegisterCapacity(bRegister);

                byte[] aCache = caches.GetCacheOfSize(aRegisterCapacity, 0);
                byte[] bCache = caches.GetCacheOfSize(bRegisterCapacity, 1);
                byte[] rCache = caches.GetCacheOfSize(aRegisterCapacity, 2);

                memory.ReadBytes(aRegister, aRegister + aRegisterCapacity, aCache);
                memory.ReadBytes(bRegister, bRegister + bRegisterCapacity, bCache);

                arithmeticFunctions[memory.ReadByte(Registers.RFLAGS)](aCache, bCache, rCache);

                memory.WriteBytes(sp, sp + aRegisterCapacity, rCache);

                MoveStackPointer(aRegisterCapacity);
            }
            else if (bytecode == Bytecodes.Arithmetic_Register_Register)
            {
                byte aRegister = NextProgramByte();
                byte bRegister = NextProgramByte();
                byte rRegister = NextProgramByte();

                byte aRegisterCapacity = Registers.RegisterCapacity(aRegister);
                byte bRegisterCapacity = Registers.RegisterCapacity(bRegister);
                byte rRegisterCapacity = Registers.RegisterCapacity(rRegister);

                // Validation.
                int resultSize = aRegisterCapacity > bRegisterCapacity ? aRegisterCapacity: bRegisterCapacity;

                if (rRegisterCapacity < resultSize)
                {
                    Exit(ReturnCodes.REGISTER_OVERFLOW);

                    return;
                }

                // Just get the main cache for the result, this should 
                // not be in use at this point.
                byte[] aCache = caches.GetCacheOfSize(aRegisterCapacity, 0);
                byte[] bCache = caches.GetCacheOfSize(bRegisterCapacity, 1);
                byte[] rCache = caches.GetCacheOfSize(rRegisterCapacity, 2);

                // Copy data to caches.
                memory.ReadBytes(aRegister, aRegister + aRegisterCapacity, aCache);
                memory.ReadBytes(bRegister, bRegister + bRegisterCapacity, bCache);
                memory.ReadBytes(rRegister, rRegister + rRegisterCapacity, rCache);

                arithmeticFunctions[memory.ReadByte(Registers.RFLAGS)](aCache, bCache, rCache);

                memory.WriteBytes(rRegister, rRegister + rRegisterCapacity, rCache);
            }
            else if (bytecode == Bytecodes.Inc_Reg)
            {
                byte register = NextProgramByte();
                byte size = Registers.RegisterCapacity(register);

                byte[] cache = caches.GetCacheOfSize(size, 0);
                byte[] rCache = caches.GetCacheOfSize(size, 1);
                byte[] tCache = caches.GetCacheOfSize(size, 2);

                caches.ClearCache(size, 2);
                tCache[0] = 1;

                memory.ReadBytes(register, register + size, cache);

                ByteHelper.AddIntBytes(cache, tCache, rCache);

                memory.WriteBytes(register, register + size, rCache);
            }
            else if (bytecode == Bytecodes.Inc_Stack)
            {
                byte size = NextProgramByte();

                if (!IsValidWordSize(size)) return;

                byte[] cache = caches.GetCacheOfSize(size, 0);
                byte[] rCache = caches.GetCacheOfSize(size, 1);
                byte[] tCache = caches.GetCacheOfSize(size, 2);

                caches.ClearCache(size, 2);
                tCache[0] = 1;

                memory.ReadBytes(sp - size, sp, cache);

                ByteHelper.AddIntBytes(cache, tCache, rCache);

                memory.WriteBytes(sp - size, sp, rCache);
            }
            else if (bytecode == Bytecodes.Dec_Reg)
            {
                byte register = NextProgramByte();
                byte size = Registers.RegisterCapacity(register);

                byte[] cache = caches.GetCacheOfSize(size, 0);
                byte[] rCache = caches.GetCacheOfSize(size, 1);
                byte[] tCache = caches.GetCacheOfSize(size, 2);

                caches.ClearCache(size, 2);
                tCache[0] = 1;

                memory.ReadBytes(register, register + size, cache);

                ByteHelper.SubtractIntBytes(cache, tCache, rCache);

                memory.WriteBytes(register, register + size, rCache);
            }
            else if (bytecode == Bytecodes.Dec_Stack)
            {
                byte size = NextProgramByte();

                if (!IsValidWordSize(size)) return;

                byte[] cache = caches.GetCacheOfSize(size, 0);
                byte[] rCache = caches.GetCacheOfSize(size, 1);
                byte[] tCache = caches.GetCacheOfSize(size, 2);

                caches.ClearCache(size, 2);
                tCache[0] = 1;

                memory.ReadBytes(sp - size, sp, cache);

                ByteHelper.SubtractIntBytes(cache, tCache, rCache);

                memory.WriteBytes(sp - size, sp, rCache);
            }

            #endregion

            // Invalid bytecode.
            else
            {
                Exit(ReturnCodes.PC_CORRUPTED_OR_NOT_OPCODE);
            }
        }

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
            sp = Registers.HighAddress;

            // Reset pc and retpc registers.
            pc = 0;

            program = null;
        }

        public byte[] DumpStack()
        {
            byte[] stack = new byte[memory.HighAddress - 1];

            memory.ReadBytes(StackLowAddress, StackHighAddress, stack);

            return stack;
        }
        public byte[] DumpProgram()
        {
            return program;
        }
        public byte[] DumpRegisters()
        {
            byte[] registers = new byte[Registers.HighAddress];

            memory.ReadBytes(Registers.LowAddress, Registers.HighAddress - 1, registers);

            return registers;
        }

        public byte[] ReadMemoryBytes(int lowAddress, int highAddress)
        {
            byte[] chunk = new byte[highAddress - lowAddress];

            memory.ReadBytes(lowAddress, highAddress, chunk);

            return chunk;
        }
        public int ReadRegisterValue(byte register)
        {
            byte registerCapacity = Registers.RegisterCapacity(register);

            byte[] cache = new byte[registerCapacity];

            memory.ReadBytes(register, register + registerCapacity, cache);

            return ByteHelper.ToInt(cache);
        }

        public byte RunProgram(byte[] program)
         {
#if DEBUG
            try
            {
#endif
                Debug.Assert(program != null);

                this.program = program;

                running = true;

                while (running)
                {
                    InterpretBytecode(program[pc]);

                    MoveProgramCounter(1);

                    if (pc >= program.Length) return ReturnCodes.DEFAULT_RET_CODE;
                    if (!running)             return memory.ReadByte(sp - 1);
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

                return ReturnCodes.DEBUG_EXCEPTION;
            }
#endif
         }
    }
}
