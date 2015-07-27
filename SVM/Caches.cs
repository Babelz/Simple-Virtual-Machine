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
    public sealed class Caches
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
        private readonly byte[][] transitionTable;

        // 1-byte caches.
        private readonly byte[] aCache;
        private readonly byte[] aaCache;
        private readonly byte[] bCache;
        private readonly byte[] bbCache;

        // 2-byte caches.
        private readonly byte[] cCache;
        private readonly byte[] ccCache;
        private readonly byte[] dCache;
        private readonly byte[] ddCache;

        // 4-byte caches.
        private readonly byte[] eCache;
        private readonly byte[] eeCache;
        private readonly byte[] fCache;
        private readonly byte[] ffCache;

        /// <summary>
        /// Main, dynamic cache chunk.
        /// TODO: create allocation methods if needed.
        ///       Otherwise, turn this cache to static
        ///       as well.
        /// </summary>
        private byte[] mCache;
        #endregion

        #region Properties
        public int MainCacheSize
        {
            get
            {
                return mCache.Length;
            }
        }
        #endregion

        public Caches(int initialChunkSize)
        {
            aCache = new byte[1];
            aaCache = new byte[1];
            bCache = new byte[1];
            bbCache = new byte[1];

            cCache = new byte[2];
            ccCache = new byte[2];
            dCache = new byte[2];
            ddCache = new byte[2];

            eCache = new byte[4];
            eeCache = new byte[4];
            fCache = new byte[4];
            ffCache = new byte[4];

            mCache = new byte[initialChunkSize];

            transitionTable = new byte[13][]
            {
                aCache,
                aaCache,
                bCache,
                bbCache,
                
                cCache,
                ccCache,
                dCache,
                ddCache,
                
                eCache,
                eeCache,
                fCache,
                ffCache,

                mCache
            };
        }

        /// <summary>
        /// Clears all caches.
        /// </summary>
        public void Clear()
        {
            for (int i = 0; i < transitionTable.Length; i++) transitionTable[i].Initialize();
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
            return transitionTable[address];
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
            if      (size == 1)  return transitionTable[0 + offset];
            else if (size == 2)  return transitionTable[4 + offset];
            else if (size == 4)  return transitionTable[8 + offset];
            else                 return transitionTable[M_CACHE];
         }
    }
}
