using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace SVM
{
    /// <summary>
    /// Register used to select arguments for opcodes.
    /// </summary>
    [Obsolete("Not used")]
    public sealed class ArgumentRegister
    {
        #region Fields
        private int flags;
        #endregion

        public ArgumentRegister()
        {
            flags = 0;
        }

        public void Clear()
        {
            flags = 0;
        }

        /*
         * 0-15 are register flags.
         */

        public byte[] GetRegisterFlags()
        {
            byte[] registerFlags = new byte[16];

            for (int i = 0; i < 15; i++)
            {
                int bit = (flags >> i) & 1;

                registerFlags[i] = (byte)bit;
            }

            return registerFlags;
        }
        public byte[] GetStackFlags()
        {
            byte[] stackFlags = new byte[4];

            stackFlags[0] = (byte)((flags >> 28) & 1);
            stackFlags[1] = (byte)((flags >> 29) & 1);
            stackFlags[2] = (byte)((flags >> 30) & 1);
            stackFlags[3] = (byte)((flags >> 31) & 1);

            return stackFlags;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void ToggleA()
        {
            flags |= 1 << 0;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void ToggleB()
        {
            flags |= 1 << 1;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void ToggleC()
        {
            flags |= 1 << 2;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void ToggleD()
        {
            flags |= 1 << 3;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void ToggleAA()
        {
            flags |= 1 << 4;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void ToggleBA()
        {
            flags |= 1 << 5;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void ToggleCA()
        {
            flags |= 1 << 6;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void ToggleDA()
        {
            flags |= 1 << 7;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void ToggleAB()
        {
            flags |= 1 << 8;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void ToggleBB()
        {
            flags |= 1 << 9;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void ToggleCB()
        {
            flags |= 1 << 10;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void ToggleDB()
        {
            flags |= 1 << 11;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void ToggleAC()
        {
            flags |= 1 << 12;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void ToggleBC()
        {
            flags |= 1 << 13;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void ToggleCC()
        {
            flags |= 1 << 14;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void ToggleDC()
        {
            flags |= 1 << 15;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void DisableA()
        {
            flags &= ~(1 << 0);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void DisableB()
        {
            flags &= ~(1 << 1);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void DisableC()
        {
            flags &= ~(1 << 2);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void DisableD()
        {
            flags &= ~(1 << 3);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void DisableAA()
        {
            flags &= ~(1 << 4);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void DisableBA()
        {
            flags &= ~(1 << 5);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void DisableCA()
        {
            flags &= ~(1 << 6);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void DisableDA()
        {
            flags &= ~(1 << 7);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void DisableAB()
        {
            flags &= ~(1 << 8);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void DisableBB()
        {
            flags &= ~(1 << 9);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void DisableCB()
        {
            flags &= ~(1 << 10);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void DisableDB()
        {
            flags &= ~(1 << 11);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void DisableAC()
        {
            flags &= ~(1 << 12);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void DisableBC()
        {
            flags &= ~(1 << 13);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void DisableCC()
        {
            flags &= ~(1 << 14);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void DisableDC()
        {
            flags &= ~(1 << 15);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void ToggleStack1()
        {
            flags |= 1 << 31;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void ToggleStack2()
        {
            flags |= 1 << 30;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void ToggleStack3()
        {
            flags |= 1 << 29;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void ToggleStack4()
        {
            flags |= 1 << 28;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void DisableStack1()
        {
            flags &= ~(1 << 31);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void DisableStack2()
        {
            flags &= ~(1 << 30);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void DisableStack3()
        {
            flags &= ~(1 << 29);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void DisableStack4()
        {
            flags &= ~(1 << 28);
        }
    }
}
