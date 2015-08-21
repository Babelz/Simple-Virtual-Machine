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
        #region HeapChunk class
        private sealed class HeapChunk
        {
            public byte[] Bytes;
            public int Key;
        }
        #endregion

        #region Fields
        /*
         * alloc person, 32                  ; allocate 32-bytes and store the heap address to a
         * realloc person, 64                ; resize chunk pointed by a to 64-bytes
         * 
         * name word 0                       ; address offset of name
         * age word 4                        ; address offset of age
         * 
         * memset person, name, "Niko"
         * memset person, age, 22
         * 
         * genframe                         ; generate stack frame with 4-bytes args
         * push person                      ; store address of person to stack
         * 
         * call person_hello
         * 
         * del person
         *                                  ; macro version
         * fnc person_hello                 ; prints "Niko - 22"
         *      flags STRING
         *      push person::name
         *      print
         *      
         *      flag INT
         *      push person::age
         *      print
         *      
         *      ret
         *                                  ; raw version
         *fnc person_hello                  
         *      memsz person, 0, w32a            ; bytes from name low address to null terminator
         *      push w32a
         *      memcpy person, 0, w32a           ; copy name to stack
         *      
         *      flag STRING
         *      print w32                        ; print name
         *      pop w32                          ; pop name
         *      
         *      memcpy person, w32a, word        ; copy age to stack
         * 
         *      flag INT
         *      print word                      ; print age
         *      pop word                        ; pop age
         *      
         *      res 0
         * 
         *      ret
         */
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
