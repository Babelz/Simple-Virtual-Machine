using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SVM
{
    internal sealed class HeapManager
    {
        #region Fields
        private int[] offsets;
        private byte[] heap;

        /// <summary>
        /// Heap pointer.
        /// </summary>
        private int hp;

        /// <summary>
        /// Offset pointer.
        /// </summary>
        private int op;
        #endregion

        public HeapManager(int initialChunkSize)
        {
            heap = new byte[initialChunkSize];
            offsets = new int[128];
        }

        private void RecordSize(byte[] bytes)
        {
            offsets[op] = bytes.Length;
            op++;

            if (op >= offsets.Length) Array.Resize(ref offsets, offsets.Length * 2);
        }

        public void Allocate(byte[] bytes)
        {
            if (hp + bytes.Length >= heap.Length) Array.Resize(ref heap, heap.Length * 2);

            for (int i = 0; i < bytes.Length; i++) heap[hp + i] = bytes[i];
        }
        public void Release()
        {
        }
        public void Clear()
        {
        }
    }
}
