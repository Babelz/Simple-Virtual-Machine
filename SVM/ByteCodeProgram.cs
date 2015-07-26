using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SVM
{
    public sealed class ByteCodeProgram
    {
        #region Fields
        private readonly List<byte> bytes;
        #endregion

        public ByteCodeProgram()
        {
            bytes = new List<byte>();
        }

        public ByteCodeProgram InsertBytes(int startIndex, params byte[] bytes)
        {
            this.bytes.InsertRange(startIndex, bytes);
            
            return this;
        }
        public ByteCodeProgram AddBytes(params byte[] bytes)
        {
            this.bytes.AddRange(bytes);

            return this;
        }
        public ByteCodeProgram AddValue(int value, int bytes)
        {
            this.bytes.AddRange(ByteHelper.ToBytes(value, bytes));

            return this;
        }

        public byte[] GetBytes()
        {
            return bytes.ToArray();
        }

        public static implicit operator byte[](ByteCodeProgram program)
        {
            return program.bytes.ToArray();
        }
    }
}
