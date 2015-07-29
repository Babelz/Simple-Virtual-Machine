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
        // 29.7.2015: masking failure. Redo.

        /*
         * Stack operations
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
         * Control flow
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
         * Memory operations
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
         * Register operations
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
         */
        #region

        /// <summary>
        /// Math two top values from the stack and store result on the stack.
        /// 
        /// Math [size (word)] [size (word)]
        /// </summary>
        public const byte Math_DirectStack = 0x0000;

        /// <summary>
        /// Math two register values together and store result on the stack.
        /// 
        /// Math [register_a] [register_b]
        /// </summary>
        public const byte Math_IndirectRegister_Stack = 0x0001;
        
        /// <summary>
        /// Math two register values together and store result to given register.
        /// 
        /// Math [register_a] [register_b] [result register]
        /// </summary>
        public const byte Math_IndirectRegister_Register = 0x0002;

        /// <summary>
        /// Math top of the stack and a register value together. Stores result to the stack.
        /// 
        /// Math [size (word)] [register]
        /// </summary>
        public const byte Math_DirectStackRegister_Stack = 0x0003;

        /// <summary>
        /// Math top of the stack and a register value together. Stores result to the given register.
        /// 
        /// Math [size (word)] [register_a] [result register]
        /// </summary>
        public const byte Math_DirectStackRegister_Register = 0x0004;

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

        /*
         * Flag and misc operations
         */ 
        #region

        /// <summary>
        /// Sets the flag register to given value. Value must be a word.
        /// 
        /// flag [value (size of word)]
        /// </summary>
        public const byte Set_Flag_Direct = 0x007F;

        /// <summary>
        /// Sets the flag register to given value contained inside a register.
        /// Only first byte is used.
        /// 
        /// flag [register_address]
        /// </summary>
        /// 
        public const byte Set_Flag_IndirectRegister = 0x007E;
        /// <summary>
        /// Sets the flag register to given value contained at the top
        /// of the stack. Only one byte is used to set the flags.
        /// 
        /// flag
        /// </summary>
        public const byte Set_Flag_IndirectStack = 0x007D;

        #endregion
    }
}
