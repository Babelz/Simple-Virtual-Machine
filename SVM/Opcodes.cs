using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SVM
{
    public enum AddressingMode
    {
        /// <summary>
        /// Arguments are followed after the opcode.
        /// </summary>
        Direct,

        /// <summary>
        /// Arguments follow after the opcode and some are 
        /// contained inside registers.
        /// </summary>
        DirectRegister,

        /// <summary>
        /// Arguments are followed after opcode and some are
        /// contained inside the stack.
        /// </summary>
        DirectStack,

        /// <summary>
        /// Arguments are found inside registers.
        /// </summary>
        IndirectRegister,

        /// <summary>
        /// Arguments are found inside the stack.
        /// </summary>
        IndirectStack
    }

    [DebuggerDisplay("Code = {Code}")]
    public struct Opcode
    {
        public readonly byte Code;
        public readonly int Args;
        public readonly AddressingMode AddressingMode;

        public Opcode(byte code, int args = 0, AddressingMode addressingMode = 0) 
        {
            Code = code;
            Args = args;
            AddressingMode = addressingMode;
        }

        public static implicit operator byte(Opcode opcode)
        {
            return opcode.Code;
        }
    }

    /// <summary>
    /// Contains all opcodes supported by the virtual machine.
    /// Opcodes that are supported are:
    /// 
    /// Stack opcodes
    ///     1) push
    ///         - direct
    ///         - indirect
    ///     2) pop
    ///         - direct
    ///         - indirect
    ///     3) top
    ///     4) sp
    ///
    /// Register opcodes
    ///     1) load
    ///     2) clear
    ///     3) copystack
    ///         - direct register
    ///         - direct stack
    ///     
    /// Control flow
    ///     1) abort 
    ///     2) halt
    ///     
    /// Memory
    ///     1) stackalloc
    ///     2) zeromemory
    ///     3) array
    ///         - direct register
    ///         - direct stack
    ///     4) ptrstack
    ///         - direct
    ///         - indirect register
    /// </summary>
    public static class Opcodes
    {
        #region Debug bytecode validation
#if DEBUG
        static Opcodes()
        {
            // Check that all opcodes have unique
            // byte code. This is a dirty but the most
            // painless way to do this. Just use reflection to find 
            // all opcode fields and check them.
            IEnumerable<Opcode> codesQuery = typeof(Opcodes).GetFields(BindingFlags.Static | BindingFlags.Public)
                                                            .Where(o => o.GetValue(null).GetType() == typeof(Opcode))
                                                            .Select(o => o.GetValue(null))
                                                            .Cast<Opcode>();

            List<Opcode> codes = codesQuery.ToList();

            Console.WriteLine("Checking that all byte codes are unique...");

            for (int i = 0; i < codes.Count; i++)
            {
                for (int j = 0; j < codes.Count; j++)
                {
                    if (i == j) continue;

                    Debug.Assert(codes[i].Code != codes[j].Code, "Duplicated byte code found, code is " + codes[i].Code);
                }

                codes.RemoveAt(i);
            }

            Console.WriteLine("All byte codes are unique!");
            Console.WriteLine("Total " + codesQuery.Count() + " byte codes were found");

            // Print all unused byte codes.

            Console.WriteLine("Free byte codes are: ");

            List<byte> byteCodes = codesQuery.Select(c => c.Code).ToList();
            int k = 0;

            for (byte i = 0; i < 255; i++)
            {
                if (!byteCodes.Contains(i))
                {
                    k++;

                    Console.Write(i);

                    if (i + 1 < 255) Console.Write("\t ");

                    if (k >= 4)
                    {
                        Console.WriteLine();
                        k = 0;
                    }
                }
            }

            Console.WriteLine("\nTotal {0} byte codes are free", 255 - byteCodes.Count);
        }
#endif
        #endregion

        // TODO: reorder opcodes by byte code...

        /*
         * Stack operations.
         */
        #region

        /// <summary>
        /// push [bytes count] [bytes]
        /// </summary>
        public static readonly Opcode Push_Direct = new Opcode(0x0001, 2, AddressingMode.Direct);

        /// <summary>
        /// push [register address] 
        /// 
        /// Size of the value depends on the size of the register.
        /// </summary>
        public static readonly Opcode Push_Register = new Opcode(0x0011, 1, AddressingMode.IndirectRegister);

        /// <summary>
        /// pop [bytes count]
        /// </summary>
        public static readonly Opcode Pop = new Opcode(0x0002, 1, AddressingMode.Direct);

        /// <summary>
        /// top [bytes count] [register address] 
        ///
        /// Copies the given amount of bytes from the top of the stack to the given register.
        /// </summary>
        public static readonly Opcode Top = new Opcode(0x0003, 1, AddressingMode.DirectRegister);

        /// <summary>
        /// sp [register address]
        /// 
        /// Saves stack pointers current address to given register.
        /// </summary>
        public static readonly Opcode Sp = new Opcode(0x0004, 1, AddressingMode.Direct);

        #endregion

        /*
         * Control flow.
         */
        #region

        /// <summary>
        /// abort
        /// 
        /// Causes the program to abort. 
        /// </summary>
        public static readonly Opcode Abort = new Opcode(0x0005);

        /// <summary>
        /// halt
        /// 
        /// Causes the program to halt. Useful for debugging purposes.
        /// </summary>
        public static readonly Opcode Halt = new Opcode(0x0015);
        
        #endregion

        /*
         * Memory operations.
         */
        #region

        /// <summary>
        /// Allocates more memory on the stack.
        /// 
        /// stackalloc [size (word)] [bytes]
        /// </summary>
        public static readonly Opcode StackAlloc = new Opcode(0x0006, 2, AddressingMode.Direct);

        // TODO: is this shit needed?
        /// <summary>
        /// Clears given amount of bytes from the stack.
        /// 
        /// zeromemory [size (word)] [bytes]
        /// </summary>
        public static readonly Opcode ZeroMemory = new Opcode(0x00016, 2, AddressingMode.Direct);

        /// <summary>
        /// Allocates new array of given size. Stores begin and end address to given registers.
        /// 
        /// array [register (begin)] [register (end)] [register (elements count)] [elements size (word)]
        /// </summary>
        public static readonly Opcode GenerateArray_IndirectRegister = new Opcode(0x0026, 4, AddressingMode.IndirectRegister);

        /// <summary>
        /// Allocates new array of given size. Stores begin and end address to the stack.
        /// 
        /// array [register (elements count)] [elements size (word)]
        /// 
        /// Values store are 32-bit addresses.
        /// </summary>
        public static readonly Opcode GenerateArray_IndirectStack = new Opcode(0x0036, 4, AddressingMode.IndirectStack);
        
        #endregion

        /*
         * Register operations.
         */
        #region

        /// <summary>
        /// Loads given value to the given register.
        /// 
        /// load [register] [bytes count] [value]
        /// </summary>
        public static readonly Opcode Load = new Opcode(0x0007, 3, AddressingMode.Direct);
        
        /// <summary>
        /// Copies value from given address from the stack to given register
        /// 
        /// copystack [register] [value_size (word)] [address_size (word)] [address]
        /// </summary>
        public static readonly Opcode CopyStack_Direct = new Opcode(0x0017, 3, AddressingMode.Direct);

        /// <summary>
        /// Copies value from given address contained inside given register from the stack to given register
        /// 
        /// copystack [size] [address_register] [target_register]
        /// </summary>
        public static readonly Opcode CopyStack_IndirectRegister = new Opcode(0x0027, 3, AddressingMode.IndirectRegister);


        /// <summary>
        /// Sets given value at given location on the stack
        /// 
        /// setstack [address_size] [address] [value_size] [value]
        /// </summary>
        public static readonly Opcode PtrStack = new Opcode(0x0037, 3, AddressingMode.Direct);

        /// <summary>
        /// Sets given value at given location on the stack
        /// 
        /// setstack [address_register] [size] [value]
        /// </summary>
        public static readonly Opcode PtrStack_IndirectRegister = new Opcode(0x0047, 3, AddressingMode.IndirectRegister);

        /// <summary>
        /// Clears given register.
        /// 
        /// clear [register]
        /// </summary>
        public static readonly Opcode Clear = new Opcode(0x0008, 1, AddressingMode.Direct);

        #endregion

        /*
         * Arithmetic
         */
        #region

        /// <summary>
        /// Add two top values from the stack and store result on the stack.
        /// 
        /// add [size (word)] [size (word)]
        /// </summary>
        public static readonly Opcode Add_DirectStack = new Opcode(0x0009, 2);

        /// <summary>
        /// Add two register values together and store result on the stack.
        /// 
        /// add [register_a] [register_b]
        /// </summary>
        public static readonly Opcode Add_IndirectRegister_Stack = new Opcode(0x0019, 2, AddressingMode.IndirectRegister);
        
        /// <summary>
        /// Add two register values together and store result to given register.
        /// 
        /// add [register_a] [register_b] [result register]
        /// </summary>
        public static readonly Opcode Add_IndirectRegister_Register = new Opcode(0x0029, 2, AddressingMode.IndirectRegister);

        /// <summary>
        /// Add top of the stack and a register value together. Stores result to the stack.
        /// 
        /// add [size (word)] [register]
        /// </summary>
        public static readonly Opcode Add_DirectStackRegister_Stack = new Opcode(0x0039, 2, AddressingMode.DirectRegister);

        /// <summary>
        /// Add top of the stack and a register value together. Stores result to the given register.
        /// 
        /// add [size (word)] [register_a] [result register]
        /// </summary>
        public static readonly Opcode Add_DirectStackRegister_Register = new Opcode(0x0059, 3, AddressingMode.DirectRegister);

        /// <summary>
        /// Increase given registers value by one.
        /// 
        /// increg [register address]
        /// </summary>
        public static readonly Opcode Inc_Reg = new Opcode(0x001A, 1, AddressingMode.Direct);
        
        /// <summary>
        /// Increase top value of the stack by one.
        /// 
        /// incstack [size (word)]
        /// </summary>
        public static readonly Opcode Inc_Stack = new Opcode(0x002A, 1, AddressingMode.Direct);

        /// <summary>
        /// Decrease given registers value by one.
        /// 
        /// decreg [register address]
        /// </summary>
        public static readonly Opcode Dec_Reg = new Opcode(0x003A, 1, AddressingMode.Direct);
        
        /// <summary>
        /// Decrease top value of the stack by one.
        /// 
        /// decstack [size (word)]
        /// </summary>
        public static readonly Opcode Dec_Stack = new Opcode(0x004A, 1, AddressingMode.Direct);
        #endregion
    }
}
