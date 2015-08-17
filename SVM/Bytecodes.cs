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
        /// push [bytes count] [bytes]
        /// </summary>
        public const byte Push_Direct = 0x0080;

        /// <summary>
        /// push [register address] 
        /// 
        /// Size of the value depends on the size of the register.
        /// </summary>
        public const byte Push_Register = 0x0081;

        /// <summary>
        /// pop [bytes count]
        /// </summary>
        public const byte Pop = 0x0082;

        /// <summary>
        /// top [bytes count] [register address] 
        ///
        /// Copies the given amount of bytes from the top of the stack to the given register.
        /// </summary>
        public const byte Top = 0x0083;

        /// <summary>
        /// sp [register address]
        /// 
        /// Saves stack pointers current address to given register.
        /// </summary>
        public const byte Sp = 0x0084;

        /// <summary>
        /// pushb [element word size] [elements count size] [elements count] [elements] ... 
        /// 
        /// Push given bytes to the stack.
        /// </summary>
        public const byte Push_Bytes = 0x0085;
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
        /// jez [stack word] [address word] [address]
        /// 
        /// Jumps to given address if top value on the stack is zero.
        /// </summary>
        public const byte Jez = 0x0025;

        /// <summary>
        /// jlz [stack word] [address word]  [address]
        /// 
        /// Jumps to given address if top value on the stack is less than zero.
        /// </summary>
        public const byte Jlz = 0x0035;

        /// <summary>
        /// jgz [stack word] [address word] [address]
        /// 
        /// Jumps to given address if top value on the stack is greater than zero.
        /// </summary>
        public const byte Jgz = 0x0045;

        /// <summary>
        /// eq [a word] [b word] [address word] [address]
        /// 
        /// Jumps to given address if two top value on the stack are equal.
        /// </summary>
        public const byte Eq = 0x0055;

        /// <summary>
        /// neq [a word] [b word] [address word] [address]
        /// 
        /// Jumps to given address if two top value on the stack are not equal.
        /// </summary>
        public const byte Neq = 0x0065;

        /// <summary>
        /// jmp [address word] [address]
        /// 
        /// Jumps to given address.
        /// </summary>
        public const byte Jump = 0x0075;

        /// <summary>
        /// jmp [stack word]
        /// 
        /// Jumps to given address located on the stack.
        /// </summary>
        public const byte Jump_Stack = 0x0085;

        /// <summary>
        /// nop
        /// 
        /// No operation.
        /// </summary>
        public const byte Nop = 0x0095;
        #endregion

        /*
         * Memory operations
         */
        #region

        // TODO: is this shit needed?
        /// <summary>
        /// Clears given amount of bytes from the stack.
        /// 
        /// zeromemory [size (word)] [bytes]
        /// </summary>
        public const byte ZeroMemory = 0x0021;

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
        public const byte PtrStack_Register = 0x0025;
        
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
        /// Loads given value to the given register.
        /// 
        /// load [register] [value]
        /// </summary>
        public const byte Load_Direct = 0x0011;
        
        /// <summary>
        /// Copies value from given address from the stack to given register
        /// 
        /// copystack [register] [value_size (word)] [address_size (word)] [address]
        /// </summary>
        public const byte CopyStack = 0x0012;

        /// <summary>
        /// Copies value from given address, contained inside given register, from the stack, to given register
        /// 
        /// copystack [size] [address_register] [target_register]
        /// </summary>
        public const byte CopyStack_Register = 0x0013;

        /// <summary>
        /// Clears given register.
        /// 
        /// clear [register]
        /// </summary>
        public const byte Clear = 0x00014;

        #endregion

        /*
         * Arithmetic operations
         */
        #region

        /// <summary>
        /// Executes math operation with two top of the stack and store result on the stack.
        /// 
        /// Math [size (word)] [size (word)]
        /// </summary>
        public const byte Arithmetic_Stack = 0x0000;

        /// <summary>
        /// Executes math operation with two register values and stores result on the stack.
        /// 
        /// Math [register_a] [register_b]
        /// </summary>
        public const byte Arithmetic_Register = 0x0001;
        
        /// <summary>
        /// Executes math operation with two register values and stores result to given register.
        /// 
        /// Math [register_a] [register_b] [result register]
        /// </summary>
        public const byte Arithmetic_Register_Register = 0x0002;

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
        public const byte Set_Flag_Register = 0x007E;
       
        /// <summary>
        /// Sets the flag register to given value contained at the top
        /// of the stack. Only one byte is used to set the flags.
        /// 
        /// flag
        /// </summary>
        public const byte Set_Flag_Stack = 0x007D;

        /// <summary>
        /// Moves given amount of bytes to the standard output stream.
        /// 
        /// print [bytes count word size] [bytes count]
        /// </summary>
        public const byte Print = 0x007E;

        /// <summary>
        /// Moves given amount of bytes to the standard output stream.
        /// 
        /// print [address size] [low address] [high address] [bytes count word size] [bytes count]
        /// </summary>
        public const byte Print_Offset = 0x007D;

        #endregion
    }
}
