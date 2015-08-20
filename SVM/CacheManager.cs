using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace SVM
{
    /*
     * Reusing the buffers is 50% faster than creating a
     * new temp buffer in every call.
     */

    /// <summary>
    /// Memory region inside the machine that contains
    /// reusable byte buffers. Buffers are used to read values
    /// from the main memory and from the program memory.
    /// These buffers are meant to store temporary
    /// information. They are usually used to store
    /// temporary data from the stack or from the program memory.
    /// Reason to wrap such small things inside a class is to avoid 
    /// calling the new word and thus reducing the call overhead of 
    /// any opcode that needs to work with memory. Don't call new,
    /// use us.
    /// </summary>
    public sealed class CacheManager
    {
        #region Constants
        /// <summary>
        /// Static 1-byte cache.
        /// </summary>
        public const byte A_CACHE = 0;

        /// <summary>
        /// Static 1-byte cache.
        /// </summary>
        public const byte B_CACHE = 2;

        /// <summary>
        /// Static 2-byte cache.
        /// </summary>
        public const byte C_CACHE = 4;

        /// <summary>
        /// Static 2-byte cache.
        /// </summary>
        public const byte D_CACHE = 6;

        /// <summary>
        /// Static 4-byte cache.
        /// </summary>
        public const byte E_CACHE = 8;

        /// <summary>
        /// Static 4-byte cache.
        /// </summary>
        public const byte F_CACHE = 10;

        /// <summary>
        /// Address of the main cache. This 
        /// cache is dynamic.
        /// </summary>
        public const byte M_CACHE = 12;
        #endregion

        #region Fields
        // Table containing all the caches.
        // Used for fast addressing.
        private readonly byte[][] caches;

        // 1-byte caches.
        private readonly byte[] c8a;
        private readonly byte[] c8b;
        private readonly byte[] c8c;
        private readonly byte[] c8d;

        // 2-byte caches.
        private readonly byte[] c16a;
        private readonly byte[] c16b;
        private readonly byte[] c16c;
        private readonly byte[] c16d;

        // 4-byte caches.
        private readonly byte[] c32a;
        private readonly byte[] c32b;
        private readonly byte[] c32c;
        private readonly byte[] c32d;

        /// <summary>
        /// Main, dynamic cache chunk.
        /// TODO: create allocation methods if needed.
        ///       Otherwise, turn this cache to static
        ///       as well.
        /// </summary>
        private byte[] cm;
        #endregion

        #region Properties
        public int MainCacheSize
        {
            get
            {
                return cm.Length;
            }
        }
        #endregion

        public CacheManager(int initialChunkSize)
        {
            c8a = new byte[1];
            c8b = new byte[1];
            c8c = new byte[1];
            c8d = new byte[1];

            c16a = new byte[2];
            c16b = new byte[2];
            c16c = new byte[2];
            c16d = new byte[2];

            c32a = new byte[4];
            c32b = new byte[4];
            c32c = new byte[4];
            c32d = new byte[4];

            cm = new byte[initialChunkSize];

            caches = new byte[13][]
            {
                c8a,
                c8b,
                c8c,
                c8d,
                
                c16a,
                c16b,
                c16c,
                c16d,
                
                c32a,
                c32b,
                c32c,
                c32d,

                cm
            };
        }

        /// <summary>
        /// Clears all caches.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void ClearCaches()
        {
            for (int i = 0; i < caches.Length; i++) ClearCache(i, 0);
        }
        /// <summary>
        /// Clears given size cache at given offset.
        /// </summary>
        /// <param name="size">cache to clear</param>
        /// <param name="offset">address offset</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void ClearCache(int size, int offset)
        {
            byte[] cache = GetCacheOfSize(size, offset);

            for (int i = 0; i < cache.Length; i++) cache[i] = 0;
        }

        /// <summary>
        /// Returns the given cache to the user.
        /// There is no memory locking included, so 
        /// some other part of the code might be using the 
        /// cache required.
        /// </summary>
        /// <param name="address">address of the cache</param>
        /// <returns>cache at given address</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte[] GetCache(int address)
        {
            return caches[address];
        }

        /// <summary>
        /// Returns a cache of given size. 
        /// </summary>
        /// <param name="size">size of the wanted cache</param>
        /// <param name="offset">address offset</param>
        /// <returns>cache of given size</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte[] GetCacheOfSize(int size, int offset)
        {
            if      (size == 1)  return caches[0 + offset];
            else if (size == 2)  return caches[4 + offset];
            else if (size == 4)  return caches[8 + offset];
            else                 return caches[M_CACHE];
         }
    }
}
