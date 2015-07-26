using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace SVM
{
    [Obsolete("Not used")]
    internal interface IRegister
    {
        /// <summary>
        /// Returns the size of the register in
        /// bytes.
        /// </summary>
        byte Size
        {
            get;
        }

        /// <summary>
        /// Clears the register.
        /// </summary>
        void Clear();

        /// <summary>
        /// Returns the current state of the register.
        /// </summary>
        /// <returns>current state of the register</returns>
        int Read();

        /// <summary>
        /// Sets the registers value.
        /// </summary>
        /// <param name="value">value to set</param>
        void Write(int value);
    }

    [DebuggerDisplay("Size = {size}, Value = {value}")]
    [Obsolete("Not used")]
    internal sealed class Register<T> : IRegister where T : struct
    {
        #region Fields
        private readonly byte size;
        private T value;
        #endregion

        #region Properties
        public byte Size
        {
            get
            {
                return size;
            }
        }
        #endregion

        public Register()
        {
            value = default(T);

            // TODO: hacks?
            object o = value;
            size = (byte)Marshal.SizeOf(o);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Clear()
        {
            value = default(T);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int Read()
        {
            object o = Convert.ChangeType(value, typeof(int));

            return (int)o;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Write(int value)
        {
            object o = Convert.ChangeType(value, typeof(T));

            this.value = (T)o;
        }
    }
}
