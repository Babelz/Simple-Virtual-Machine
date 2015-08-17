using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SVM
{
    public sealed class BytecodeProgram
    {
        #region Fields
        private readonly List<byte> bytes;
        #endregion

        public BytecodeProgram()
        {
            bytes = new List<byte>();
        }

        public BytecodeProgram InsertBytes(int startIndex, params byte[] bytes)
        {
            this.bytes.InsertRange(startIndex, bytes);
            
            return this;
        }
        public BytecodeProgram AddBytes(params byte[] bytes)
        {
            this.bytes.AddRange(bytes);

            return this;
        }
        public BytecodeProgram AddValue(int value, int bytes)
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

        public static implicit operator byte[](BytecodeProgram program)
        {
            return program.bytes.ToArray();
        }
    }
}
