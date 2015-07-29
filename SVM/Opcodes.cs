using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace SVM
{
    /// <summary>
    /// Contains all bytecodes supported by the virtual machine.
    public static class Bytecodes
    {
        // Mask bits are 3 MSB's.

        /*
         * Stack operations.
         * Mask is 1000 0000 (128).
         */
        #region

        /// <summary>
        /// Code 128
        /// push [bytes count] [bytes]
        /// </summary>
        public const byte Push_Direct = 0x0080;

        /// <summary>
        /// Code 129
        /// push [register address] 
        /// 
        /// Size of the value depends on the size of the register.
        /// </summary>
        public const byte Push_IndirectRegister = 0x0081;

        /// <summary>
        /// Code 130
        /// pop [bytes count]
        /// </summary>
        public const byte Pop = 0x0082;

        /// <summary>
        /// Code 131
        /// top [bytes count] [register address] 
        ///
        /// Copies the given amount of bytes from the top of the stack to the given register.
        /// </summary>
        public const byte Top = 0x0083;

        /// <summary>
        /// Code 132
        /// sp [register address]
        /// 
        /// Saves stack pointers current address to given register.
        /// </summary>
        public const byte Sp = 0x0084;

        #endregion

        /*
         * Control flow.
         * Mask is 0100 0000 (64).
         */
        #region

        /// <summary>
        /// abort
        /// 
        /// Causes the program to abort. 
        /// </summary>
        public const byte Abort = 0x0005;

        /// <summary>
        /// halt
        /// 
        /// Causes the program to halt. Useful for debugging purposes.
        /// </summary>
        public const byte Halt = 0x0015;
        
        #endregion

        /*
         * Memory operations.
         * 0010 0000 (32).
         */
        #region

        /// <summary>
        /// Allocates more memory on the stack.
        /// 
        /// stackalloc [size (word)] [bytes]
        /// </summary>
        public const byte StackAlloc = 0x0020;

        // TODO: is this shit needed?
        /// <summary>
        /// Clears given amount of bytes from the stack.
        /// 
        /// zeromemory [size (word)] [bytes]
        /// </summary>
        public const byte ZeroMemory = 0x0021;

        /// <summary>
        /// Allocates new array of given size. Stores begin and end address to given registers.
        /// 
        /// array [register (begin)] [register (end)] [register (elements count)] [elements size (word)]
        /// </summary>
        public const byte GenerateArray_IndirectRegister = 0x0022;

        /// <summary>
        /// Allocates new array of given size. Stores begin and end address to the stack.
        /// 
        /// array [register (elements count)] [elements size (word)]
        /// 
        /// Values store are 32-bit addresses.
        /// </summary>
        public const byte GenerateArray_IndirectStack = 0x0023;

        /// <summary>
        /// Sets given value at given location on the stack
        /// 
        /// ptrstack [address_size] [address] [value_size] [value]
        /// </summary>
        public const byte PtrStack = 0x0024;

        /// <summary>
        /// Sets given value at given location on the stack
        /// 
        /// ptrstack [address_register] [size] [value]
        /// </summary>
        public const byte PtrStack_IndirectRegister = 0x0025;
        
        #endregion

        /*
         * Register operations.
         * 0001 0000 (32).
         */
        #region

        /// <summary>
        /// Loads given value to the given register.
        /// 
        /// load [register] [bytes count] [value]
        /// </summary>
        public const byte Load = 0x0010;
        
        /// <summary>
        /// Copies value from given address from the stack to given register
        /// 
        /// copystack [register] [value_size (word)] [address_size (word)] [address]
        /// </summary>
        public const byte CopyStack_Direct = 0x0011;

        /// <summary>
        /// Copies value from given address contained inside given register from the stack to given register
        /// 
        /// copystack [size] [address_register] [target_register]
        /// </summary>
        public const byte CopyStack_IndirectRegister = 0x0012;

        /// <summary>
        /// Clears given register.
        /// 
        /// clear [register]
        /// </summary>
        public const byte Clear = 0x00013;

        #endregion

        /*
         * Arithmetic operations
         * 11000 0000
         */
        #region

        /// <summary>
        /// Add two top values from the stack and store result on the stack.
        /// 
        /// add [size (word)] [size (word)]
        /// </summary>
        public const byte Add_DirectStack = 0x0000;

        /// <summary>
        /// Add two register values together and store result on the stack.
        /// 
        /// add [register_a] [register_b]
        /// </summary>
        public const byte Add_IndirectRegister_Stack = 0x0001;
        
        /// <summary>
        /// Add two register values together and store result to given register.
        /// 
        /// add [register_a] [register_b] [result register]
        /// </summary>
        public const byte Add_IndirectRegister_Register = 0x0002;

        /// <summary>
        /// Add top of the stack and a register value together. Stores result to the stack.
        /// 
        /// add [size (word)] [register]
        /// </summary>
        public const byte Add_DirectStackRegister_Stack = 0x0003;

        /// <summary>
        /// Add top of the stack and a register value together. Stores result to the given register.
        /// 
        /// add [size (word)] [register_a] [result register]
        /// </summary>
        public const byte Add_DirectStackRegister_Register = 0x0004;

        public const byte Sub_DirectStack = 0x0005;

        public const byte Sub_IndirectRegister_stack = 0x0006;

        public const byte Sub_IndirectRegister_Register = 0x0007;

        public const byte Sub_DirectStackRegister_Stack = 0x0008;

        public const byte Sub_DirectStackRegister_Register = 0x0009;

        /// <summary>
        /// Increase given registers value by one.
        /// 
        /// increg [register address]
        /// </summary>
        public const byte Inc_Reg = 0x000A;
        
        /// <summary>
        /// Increase top value of the stack by one.
        /// 
        /// incstack [size (word)]
        /// </summary>
        public const byte Inc_Stack = 0x000B;

        /// <summary>
        /// Decrease given registers value by one.
        /// 
        /// decreg [register address]
        /// </summary>
        public const byte Dec_Reg = 0x000C;
        
        /// <summary>
        /// Decrease top value of the stack by one.
        /// 
        /// decstack [size (word)]
        /// </summary>
        public const byte Dec_Stack = 0x000D;
        #endregion

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsStackOperation(byte bytecode)
        {
            return ((bytecode >> 7) & 1) == 1;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsFlowOperation(byte bytecode)
        {
            return ((bytecode >> 6) & 1) == 1;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsMemoryOperation(byte bytecode)
        {
            return ((bytecode >> 5) & 1) == 1;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsRegisterOperation(byte bytecode)
        {
            return ((bytecode >> 4) & 1) == 1;
        }
    }
}
