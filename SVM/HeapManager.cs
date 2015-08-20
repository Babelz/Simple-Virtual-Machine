using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SVM
{
    /// <summary>
    /// TODO: initial implementation. This version is fucking slow. 
    ///       please fix it.
    ///       
    /// TODO: fix lookup speed.
    /// </summary>
    internal sealed class HeapManager
    {
        #region Fields
        #endregion

        public HeapManager(int initialChunkSize)
        {
        }

        private void EnsureHeapCapacity(int bytes)
        {
        }
        private void EnsureRecordCapacity()
        {
        }

        public void ReAllocate(int lowAddress, int bytes)
        {

        }
        public void Allocate(int bytes)
        {
        }
        public void Delete(int lowAddress)
        {
        }
        
        public void ReadBytes(int lowAddress, int highAddress, byte[] buffer)
        {
        }
        public void WriteBytes(int lowAddress, int highAddress, byte[] buffer)
        {
        }

        public void Clear()
        {
        }
    }
}
