using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace SVM
{
    // TODO: optimize for speed and memory usage, find a 
    //       balance between those two things.
    //       Currently memory usage is sacrificed 
    //       to get better performance.
    [DebuggerDisplay("Bytes = {HighAddress}")]
    internal sealed class MemoryManager
    {
        #region Fields
        private byte[] chunk;
        #endregion

        #region Properties
        /// <summary>
        /// Low address of the chunk this is
        /// the lowest addressable chunk.
        /// </summary>
        public int LowAddress
        {
            get
            {
                return 0;
            }
        }
        /// <summary>
        /// High address of the chunk, this is
        /// the last addressable chunk.
        /// </summary>
        public int HighAddress
        {
            get
            {
                return chunk.Length;
            }
        }
        #endregion

        public MemoryManager(int initialChunkSize)
        {
            chunk = new byte[initialChunkSize];
        }

        /// <summary>
        /// Resizes the memory to given size.
        /// </summary>
        /// <param name="bytes">bytes to allocate</param>
        public void Resize(int bytes)
        {
            Array.Resize(ref chunk, bytes);
        }

        /// <summary>
        /// Insert given byte to given memory location.
        /// </summary>
        /// <param name="value">value to insert</param>
        /// <param name="offset">address where the byte will be placed</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void WriteByte(int offset, byte value)
        {
            Debug.Assert(offset < HighAddress);

            chunk[offset] = value;
        }
        /// <summary>
        /// Inserts given chunk of bytes starting from given address.
        /// </summary>
        /// <param name="buffer">bytes to insert</param>
        /// <param name="offset">address from where placing the bytes will start</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void WriteBytes(int lowAddress, int highAddress, byte[] buffer)
        {
            int i = lowAddress;
            int j = 0;

            do 
            {
                chunk[i] = buffer[j];
                
                i++;
                j++;
            } while (i < highAddress);
        }
        
        /// <summary>
        /// Reads byte at given address.
        /// </summary>
        /// <param name="address">address</param>
        /// <returns>byte at address</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte ReadByte(int address)
        {
            return chunk[address];
        }
        /// <summary>
        /// Reads given amount of bytes to given buffer.
        /// </summary>
        /// <param name="lowAddress">low address (begin)</param>
        /// <param name="highAddress">bytes to read</param>
        /// <param name="buffer">buffer where the bytes are stored</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void ReadBytes(int lowAddress, int highAddress, byte[] buffer)
        {
            int i = lowAddress;
            int j = 0;

            do
            {
                buffer[j] = chunk[i];

                j++;
                i++;
            } while (i < highAddress);
        }

        /// <summary>
        /// Reserves given amount of bytes. If not enough space
        /// is not available, more will be allocated.
        /// </summary>
        /// <param name="bytes">bytes to add</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Reserve(int bytes, int offset)
        {
            if (bytes + offset >= HighAddress)
            {
                // TODO: could do the following things..
                //       1) optimize allocation size based on 
                //          the memory pressure
                //       2) use various chunk sizes to 
                //          allocate memory 
                Array.Resize(ref this.chunk, this.chunk.Length * 2);
            }
        }
        /// <summary>
        /// Clears given amount of bytes.
        /// </summary>
        /// <param name="lowAddress">begin</param>
        /// <param name="highAddress">end</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Clear(int lowAddress, int highAddress)
        {
            Debug.Assert(lowAddress >= 0 && highAddress < HighAddress, "Invalid offsets");
            
            for (int i = lowAddress; i < highAddress; i++) chunk[i] = 0;
        }

        /// <summary>
        /// Clears every byte.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Clear()
        {
            for (int i = 0; i < chunk.Length; i++) chunk[i] = 0;
        }
    }
}
