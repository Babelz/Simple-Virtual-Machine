using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SVM
{
    public sealed class BytecodeBuffer
    {
        #region Fields
        private readonly List<byte> bytes;
        #endregion

        public BytecodeBuffer()
        {
            bytes = new List<byte>();
        }

        public BytecodeBuffer InsertBytes(int startIndex, params byte[] bytes)
        {
            this.bytes.InsertRange(startIndex, bytes);
            
            return this;
        }
        public BytecodeBuffer AddBytes(params byte[] bytes)
        {
            this.bytes.AddRange(bytes);

            return this;
        }
        public BytecodeBuffer AddValue(int value, int bytes)
        {
            byte[] buffer = new byte[bytes];

            ByteHelper.ToBytes(value, buffer);

            this.bytes.AddRange(buffer);

            return this;
        }

        public byte[] GetBytes()
        {
            return bytes.ToArray();
        }

        public void Clear()
        {
            bytes.Clear();
        }

        public static implicit operator byte[](BytecodeBuffer program)
        {
            return program.bytes.ToArray();
        }
    }
}
