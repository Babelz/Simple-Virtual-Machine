using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SVM
{
    /// <summary>
    /// Contains all bytecodes supported by the virtual machine.
    public static class Bytecodes
    {
        // TODO: reorder bytecodes by value...

        /*
         * Stack operations.
         */
        #region

        /// <summary>
        /// push [bytes count] [bytes]
        /// </summary>
        public const byte Push_Direct = 0x0001;

        /// <summary>
        /// push [register address] 
        /// 
        /// Size of the value depends on the size of the register.
        /// </summary>
        public const byte Push_Register = 0x0011;

        /// <summary>
        /// pop [bytes count]
        /// </summary>
        public const byte Pop = 0x0002;

        /// <summary>
        /// top [bytes count] [register address] 
        ///
        /// Copies the given amount of bytes from the top of the stack to the given register.
        /// </summary>
        public const byte Top = 0x0003;

        /// <summary>
        /// sp [register address]
        /// 
        /// Saves stack pointers current address to given register.
        /// </summary>
        public const byte Sp = 0x0004;

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
         */
        #region

        /// <summary>
        /// Allocates more memory on the stack.
        /// 
        /// stackalloc [size (word)] [bytes]
        /// </summary>
        public const byte StackAlloc = 0x0006;

        // TODO: is this shit needed?
        /// <summary>
        /// Clears given amount of bytes from the stack.
        /// 
        /// zeromemory [size (word)] [bytes]
        /// </summary>
        public const byte ZeroMemory = 0x00016;

        /// <summary>
        /// Allocates new array of given size. Stores begin and end address to given registers.
        /// 
        /// array [register (begin)] [register (end)] [register (elements count)] [elements size (word)]
        /// </summary>
        public const byte GenerateArray_IndirectRegister = 0x0026;

        /// <summary>
        /// Allocates new array of given size. Stores begin and end address to the stack.
        /// 
        /// array [register (elements count)] [elements size (word)]
        /// 
        /// Values store are 32-bit addresses.
        /// </summary>
        public const byte GenerateArray_IndirectStack = 0x0036;
        
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
        public const byte Load = 0x0007;
        
        /// <summary>
        /// Copies value from given address from the stack to given register
        /// 
        /// copystack [register] [value_size (word)] [address_size (word)] [address]
        /// </summary>
        public const byte CopyStack_Direct = 0x0017;

        /// <summary>
        /// Copies value from given address contained inside given register from the stack to given register
        /// 
        /// copystack [size] [address_register] [target_register]
        /// </summary>
        public const byte CopyStack_IndirectRegister = 0x0027;


        /// <summary>
        /// Sets given value at given location on the stack
        /// 
        /// ptrstack [address_size] [address] [value_size] [value]
        /// </summary>
        public const byte PtrStack = 0x0037;

        /// <summary>
        /// Sets given value at given location on the stack
        /// 
        /// ptrstack [address_register] [size] [value]
        /// </summary>
        public const byte PtrStack_IndirectRegister = 0x0047;

        /// <summary>
        /// Clears given register.
        /// 
        /// clear [register]
        /// </summary>
        public const byte Clear = 0x0008;

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
        public const byte Add_DirectStack = 0x0009;

        /// <summary>
        /// Add two register values together and store result on the stack.
        /// 
        /// add [register_a] [register_b]
        /// </summary>
        public const byte Add_IndirectRegister_Stack = 0x0019;
        
        /// <summary>
        /// Add two register values together and store result to given register.
        /// 
        /// add [register_a] [register_b] [result register]
        /// </summary>
        public const byte Add_IndirectRegister_Register = 0x0029;

        /// <summary>
        /// Add top of the stack and a register value together. Stores result to the stack.
        /// 
        /// add [size (word)] [register]
        /// </summary>
        public const byte Add_DirectStackRegister_Stack = 0x0039;

        /// <summary>
        /// Add top of the stack and a register value together. Stores result to the given register.
        /// 
        /// add [size (word)] [register_a] [result register]
        /// </summary>
        public const byte Add_DirectStackRegister_Register = 0x0059;

        /// <summary>
        /// Increase given registers value by one.
        /// 
        /// increg [register address]
        /// </summary>
        public const byte Inc_Reg = 0x001A;
        
        /// <summary>
        /// Increase top value of the stack by one.
        /// 
        /// incstack [size (word)]
        /// </summary>
        public const byte Inc_Stack = 0x002A;

        /// <summary>
        /// Decrease given registers value by one.
        /// 
        /// decreg [register address]
        /// </summary>
        public const byte Dec_Reg = 0x003A;
        
        /// <summary>
        /// Decrease top value of the stack by one.
        /// 
        /// decstack [size (word)]
        /// </summary>
        public const byte Dec_Stack = 0x004A;
        #endregion
    }
}
