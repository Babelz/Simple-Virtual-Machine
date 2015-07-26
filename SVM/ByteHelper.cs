using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace SVM
{
    /*
     * Not sure about the speed of conversion and math operators, but
     * some other implementations have been tried and the current ones
     * are the fastest.
     */
    public static class ByteHelper
    {
        public static readonly byte[] HWORDONE = new byte[1] { 1 };
        public static readonly byte[] WORDONE = new byte[2] { 1, 0 };
        public static readonly byte[] LWORDONE = new byte[4] { 1, 0, 0, 0 };
        public static readonly byte[] DWORDONE = new byte[8] { 1, 0, 0, 0, 0, 0, 0, 0 };

        /// <summary>
        /// Converts given bytes to integer. Currently supports
        /// max 4-bytes.
        /// </summary>
        /// <param name="bytes">bytes to convert</param>
        /// <returns>converted value</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int ToInt(params byte[] bytes)
        {
            int result = 0;

            if (bytes.Length >= 1)
            {
                int bit = (bytes[0] >> 0) & 1;
                result |= (bit << 0);

                bit = (bytes[0] >> 1) & 1;
                result |= (bit << 1);

                bit = (bytes[0] >> 2) & 1;
                result |= (bit << 2);

                bit = (bytes[0] >> 3) & 1;
                result |= (bit << 3);

                bit = (bytes[0] >> 4) & 1;
                result |= (bit << 4);

                bit = (bytes[0] >> 5) & 1;
                result |= (bit << 5);

                bit = (bytes[0] >> 6) & 1;
                result |= (bit << 6);

                bit = (bytes[0] >> 7) & 1;
                result |= (bit << 7);

                if (bytes.Length >= 2)
                {
                    bit = (bytes[1] >> 0) & 1;
                    result |= (bit << 8);

                    bit = (bytes[1] >> 1) & 1;
                    result |= (bit << 9);

                    bit = (bytes[1] >> 2) & 1;
                    result |= (bit << 10);

                    bit = (bytes[1] >> 3) & 1;
                    result |= (bit << 11);

                    bit = (bytes[1] >> 4) & 1;
                    result |= (bit << 12);

                    bit = (bytes[1] >> 5) & 1;
                    result |= (bit << 13);

                    bit = (bytes[1] >> 6) & 1;
                    result |= (bit << 14);

                    bit = (bytes[1] >> 7) & 1;
                    result |= (bit << 15);

                    if (bytes.Length >= 3)
                    {
                        bit = (bytes[2] >> 0) & 1;
                        result |= (bit << 16);

                        bit = (bytes[2] >> 1) & 1;
                        result |= (bit << 17);

                        bit = (bytes[2] >> 2) & 1;
                        result |= (bit << 18);

                        bit = (bytes[2] >> 3) & 1;
                        result |= (bit << 19);

                        bit = (bytes[2] >> 4) & 1;
                        result |= (bit << 20);

                        bit = (bytes[2] >> 5) & 1;
                        result |= (bit << 21);

                        bit = (bytes[2] >> 6) & 1;
                        result |= (bit << 22);

                        bit = (bytes[2] >> 7) & 1;
                        result |= (bit << 23);

                        if (bytes.Length >= 4)
                        {
                            bit = (bytes[3] >> 0) & 1;
                            result |= (bit << 24);

                            bit = (bytes[3] >> 1) & 1;
                            result |= (bit << 25);

                            bit = (bytes[3] >> 2) & 1;
                            result |= (bit << 26);

                            bit = (bytes[3] >> 3) & 1;
                            result |= (bit << 27);

                            bit = (bytes[3] >> 4) & 1;
                            result |= (bit << 28);

                            bit = (bytes[3] >> 5) & 1;
                            result |= (bit << 29);

                            bit = (bytes[3] >> 6) & 1;
                            result |= (bit << 30);

                            bit = (bytes[3] >> 7) & 1;
                            result |= (bit << 31);
                        }
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Converts given value to bytes. Currently supports
        /// max 4-bytes.
        /// </summary>
        /// <param name="value">value to convert</param>
        /// <param name="bytes">bytes to convert</param>
        /// <returns>converted bytes</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte[] ToBytes(int value, int bytes)
        {
            byte[] results = new byte[bytes];

            if (bytes >= 1)
            {
                results[0] |= (byte)(((value >> 0) & 1) << 0);
                results[0] |= (byte)(((value >> 1) & 1) << 1);
                results[0] |= (byte)(((value >> 2) & 1) << 2);
                results[0] |= (byte)(((value >> 3) & 1) << 3);
                results[0] |= (byte)(((value >> 4) & 1) << 4);
                results[0] |= (byte)(((value >> 5) & 1) << 5);
                results[0] |= (byte)(((value >> 6) & 1) << 6);
                results[0] |= (byte)(((value >> 7) & 1) << 7);

                if (bytes >= 2)
                {
                    results[1] |= (byte)(((value >> 8) & 1) << 0);
                    results[1] |= (byte)(((value >> 9) & 1) << 1);
                    results[1] |= (byte)(((value >> 10) & 1) << 2);
                    results[1] |= (byte)(((value >> 11) & 1) << 3);
                    results[1] |= (byte)(((value >> 12) & 1) << 4);
                    results[1] |= (byte)(((value >> 13) & 1) << 5);
                    results[1] |= (byte)(((value >> 14) & 1) << 6);
                    results[1] |= (byte)(((value >> 15) & 1) << 7);

                    if (bytes >= 3)
                    {
                        results[2] |= (byte)(((value >> 16) & 1) << 0);
                        results[2] |= (byte)(((value >> 17) & 1) << 1);
                        results[2] |= (byte)(((value >> 18) & 1) << 2);
                        results[2] |= (byte)(((value >> 19) & 1) << 3);
                        results[2] |= (byte)(((value >> 20) & 1) << 4);
                        results[2] |= (byte)(((value >> 21) & 1) << 5);
                        results[2] |= (byte)(((value >> 22) & 1) << 6);
                        results[2] |= (byte)(((value >> 23) & 1) << 7);

                        if (bytes >= 4)
                        {
                            results[3] |= (byte)(((value >> 24) & 1) << 0);
                            results[3] |= (byte)(((value >> 25) & 1) << 1);
                            results[3] |= (byte)(((value >> 26) & 1) << 2);
                            results[3] |= (byte)(((value >> 27) & 1) << 3);
                            results[3] |= (byte)(((value >> 28) & 1) << 4);
                            results[3] |= (byte)(((value >> 29) & 1) << 5);
                            results[3] |= (byte)(((value >> 30) & 1) << 6);
                            results[3] |= (byte)(((value >> 31) & 1) << 7);
                        }
                    }
                }
            }

            return results;
        }

        /// <summary>
        /// Adds two variables presented as bytes together 
        /// and returns the result to the caller. This function supports
        /// max of 4-bytes per variable currently.
        /// </summary>
        /// <param name="lhs">left hand side bytes</param>
        /// <param name="rhs">right hand side bytes</param>
        /// <returns>lhs + rhs</returns>
        public static int AddInt(byte[] lhs, byte[] rhs)
        {
            int result = 0;
            bool carry = false;

            // This way of doing this is around 25 times faster than 
            // "normal way" (using loops etc). It is ugly as fuck but hey!
            // its illegally fast compared to the last implementation.

            if (lhs[0] == 0 && rhs[0] == 0) goto b_8;

            int bit = ((lhs[0] >> 0) & 1) + ((rhs[0] >> 0) & 1);
            carry = bit == 2;
            if (bit == 1) result |= 1 << 0;

            bit = ((lhs[0] >> 1) & 1) + ((rhs[0] >> 1) & 1);
            if (bit == 2) { if (carry) result |= 1 << 1; else carry = true; }
            else if (bit == 1 && !carry) result |= 1 << 1;
            else if (bit == 0 && carry) { carry = false; result |= 1 << 1; }

            bit = ((lhs[0] >> 2) & 1) + ((rhs[0] >> 2) & 1);
            if (bit == 2) { if (carry) result |= 1 << 2; else carry = true; }
            else if (bit == 1 && !carry) result |= 1 << 2;
            else if (bit == 0 && carry) { carry = false; result |= 1 << 2; }

            bit = ((lhs[0] >> 3) & 1) + ((rhs[0] >> 3) & 1);
            if (bit == 2) { if (carry) result |= 1 << 3; else carry = true; }
            else if (bit == 1 && !carry) result |= 1 << 3;
            else if (bit == 0 && carry) { carry = false; result |= 1 << 3; }

            bit = ((lhs[0] >> 4) & 1) + ((rhs[0] >> 4) & 1);
            if (bit == 2) { if (carry) result |= 1 << 4; else carry = true; }
            else if (bit == 1 && !carry) result |= 1 << 4;
            else if (bit == 0 && carry) { carry = false; result |= 1 << 4; }

            bit = ((lhs[0] >> 5) & 1) + ((rhs[0] >> 5) & 1);
            if (bit == 2) { if (carry) result |= 1 << 5; else carry = true; }
            else if (bit == 1 && !carry) result |= 1 << 5;
            else if (bit == 0 && carry) { carry = false; result |= 1 << 5; }

            bit = ((lhs[0] >> 6) & 1) + ((rhs[0] >> 6) & 1);
            if (bit == 2) { if (carry) result |= 1 << 6; else carry = true; }
            else if (bit == 1 && !carry) result |= 1 << 6;
            else if (bit == 0 && carry) { carry = false; result |= 1 << 6; }

            bit = ((lhs[0] >> 7) & 1) + ((rhs[0] >> 7) & 1);
            if (bit == 2) { if (carry) result |= 1 << 7; else carry = true; }
            else if (bit == 1 && !carry) result |= 1 << 7;
            else if (bit == 0 && carry) { carry = false; result |= 1 << 7; }

        b_8:
            if (lhs.Length >= 2)
            {
                if (lhs[1] == 0 && rhs[1] == 0)
                {
                    if (carry) { carry = false; result |= 1 << 8; } goto b_16;
                }
            }
            else return result;

            bit = ((lhs[1] >> 0) & 1) + ((rhs[1] >> 0) & 1);
            if (bit == 2) { if (carry) result |= 1 << 8; else carry = true; }
            else if (bit == 1 && !carry) result |= 1 << 8;
            else if (bit == 0 && carry) { carry = false; result |= 1 << 8; }

            bit = ((lhs[1] >> 1) & 1) + ((rhs[1] >> 1) & 1);
            if (bit == 2) { if (carry) result |= 1 << 9; else carry = true; }
            else if (bit == 1 && !carry) result |= 1 << 9;
            else if (bit == 0 && carry) { carry = false; result |= 1 << 9; }

            bit = ((lhs[1] >> 2) & 1) + ((rhs[1] >> 2) & 1);
            if (bit == 2) { if (carry) result |= 1 << 10; else carry = true; }
            else if (bit == 1 && !carry) result |= 1 << 10;
            else if (bit == 0 && carry) { carry = false; result |= 1 << 10; }

            bit = ((lhs[1] >> 3) & 1) + ((rhs[1] >> 3) & 1);
            if (bit == 2) { if (carry) result |= 1 << 11; else carry = true; }
            else if (bit == 1 && !carry) result |= 1 << 11;
            else if (bit == 0 && carry) { carry = false; result |= 1 << 11; }

            bit = ((lhs[1] >> 4) & 1) + ((rhs[1] >> 4) & 1);
            if (bit == 2) { if (carry) result |= 1 << 12; else carry = true; }
            else if (bit == 1 && !carry) result |= 1 << 12;
            else if (bit == 0 && carry) { carry = false; result |= 1 << 12; }

            bit = ((lhs[1] >> 5) & 1) + ((rhs[1] >> 5) & 1);
            if (bit == 2) { if (carry) result |= 1 << 13; else carry = true; }
            else if (bit == 1 && !carry) result |= 1 << 13;
            else if (bit == 0 && carry) { carry = false; result |= 1 << 13; }

            bit = ((lhs[1] >> 6) & 1) + ((rhs[1] >> 6) & 1);
            if (bit == 2) { if (carry) result |= 1 << 14; else carry = true; }
            else if (bit == 1 && !carry) result |= 1 << 14;
            else if (bit == 0 && carry) { carry = false; result |= 1 << 14; }

            bit = ((lhs[1] >> 7) & 1) + ((rhs[1] >> 7) & 1);
            if (bit == 2) { if (carry) result |= 1 << 15; else carry = true; }
            else if (bit == 1 && !carry) result |= 1 << 15;
            else if (bit == 0 && carry) { carry = false; result |= 1 << 15; }

        b_16:
            if (lhs.Length >= 3)
            {
                if (lhs[2] == 0 && rhs[2] == 0)
                {
                    if (carry) { carry = false; result |= 1 << 16; } goto b_24;
                }
            }
            else return result;

            bit = ((lhs[2] >> 0) & 1) + ((rhs[2] >> 0) & 1);
            if (bit == 2) { if (carry) result |= 1 << 16; else carry = true; }
            else if (bit == 1 && !carry) result |= 1 << 16;
            else if (bit == 0 && carry) { carry = false; result |= 1 << 16; }

            bit = ((lhs[2] >> 1) & 1) + ((rhs[2] >> 1) & 1);
            if (bit == 2) { if (carry) result |= 1 << 17; else carry = true; }
            else if (bit == 1 && !carry) result |= 1 << 17;
            else if (bit == 0 && carry) { carry = false; result |= 1 << 17; }

            bit = ((lhs[2] >> 2) & 1) + ((rhs[2] >> 2) & 1);
            if (bit == 2) { if (carry) result |= 1 << 18; else carry = true; }
            else if (bit == 1 && !carry) result |= 1 << 18;
            else if (bit == 0 && carry) { carry = false; result |= 1 << 18; }

            bit = ((lhs[2] >> 3) & 1) + ((rhs[2] >> 3) & 1);
            if (bit == 2) { if (carry) result |= 1 << 19; else carry = true; }
            else if (bit == 1 && !carry) result |= 1 << 19;
            else if (bit == 0 && carry) { carry = false; result |= 1 << 19; }

            bit = ((lhs[2] >> 4) & 1) + ((rhs[2] >> 4) & 1);
            if (bit == 2) { if (carry) result |= 1 << 20; else carry = true; }
            else if (bit == 1 && !carry) result |= 1 << 20;
            else if (bit == 0 && carry) { carry = false; result |= 1 << 20; }

            bit = ((lhs[2] >> 5) & 1) + ((rhs[2] >> 5) & 1);
            if (bit == 2) { if (carry) result |= 1 << 21; else carry = true; }
            else if (bit == 1 && !carry) result |= 1 << 21;
            else if (bit == 0 && carry) { carry = false; result |= 1 << 21; }

            bit = ((lhs[2] >> 6) & 1) + ((rhs[2] >> 6) & 1);
            if (bit == 2) { if (carry) result |= 1 << 22; else carry = true; }
            else if (bit == 1 && !carry) result |= 1 << 22;
            else if (bit == 0 && carry) { carry = false; result |= 1 << 22; }

            bit = ((lhs[2] >> 7) & 1) + ((rhs[2] >> 7) & 1);
            if (bit == 2) { if (carry) result |= 1 << 23; else carry = true; }
            else if (bit == 1 && !carry) result |= 1 << 23;
            else if (bit == 0 && carry) { carry = false; result |= 1 << 23; }

        b_24:
            if (lhs.Length >= 4)
            {
                if (lhs[3] == 0 && rhs[3] == 0)
                {
                    if (carry) { carry = false; result |= 1 << 24; } return result;
                }
            }

            bit = ((lhs[3] >> 0) & 1) + ((rhs[3] >> 0) & 1);
            if (bit == 2) { if (carry) result |= 1 << 24; else carry = true; }
            else if (bit == 1 && !carry) result |= 1 << 24;
            else if (bit == 0 && carry) { carry = false; result |= 1 << 24; }

            bit = ((lhs[3] >> 1) & 1) + ((rhs[3] >> 1) & 1);
            if (bit == 2) { if (carry) result |= 1 << 25; else carry = true; }
            else if (bit == 1 && !carry) result |= 1 << 25;
            else if (bit == 0 && carry) { carry = false; result |= 1 << 25; }

            bit = ((lhs[3] >> 2) & 1) + ((rhs[3] >> 2) & 1);
            if (bit == 2) { if (carry) result |= 1 << 26; else carry = true; }
            else if (bit == 1 && !carry) result |= 1 << 26;
            else if (bit == 0 && carry) { carry = false; result |= 1 << 26; }

            bit = ((lhs[3] >> 3) & 1) + ((rhs[3] >> 3) & 1);
            if (bit == 2) { if (carry) result |= 1 << 27; else carry = true; }
            else if (bit == 1 && !carry) result |= 1 << 27;
            else if (bit == 0 && carry) { carry = false; result |= 1 << 27; }

            bit = ((lhs[3] >> 4) & 1) + ((rhs[3] >> 4) & 1);
            if (bit == 2) { if (carry) result |= 1 << 28; else carry = true; }
            else if (bit == 1 && !carry) result |= 1 << 28;
            else if (bit == 0 && carry) { carry = false; result |= 1 << 28; }

            bit = ((lhs[3] >> 5) & 1) + ((rhs[3] >> 5) & 1);
            if (bit == 2) { if (carry) result |= 1 << 29; else carry = true; }
            else if (bit == 1 && !carry) result |= 1 << 29;
            else if (bit == 0 && carry) { carry = false; result |= 1 << 29; }

            bit = ((lhs[3] >> 6) & 1) + ((rhs[3] >> 6) & 1);
            if (bit == 2) { if (carry) result |= 1 << 30; else carry = true; }
            else if (bit == 1 && !carry) result |= 1 << 30;
            else if (bit == 0 && carry) { carry = false; result |= 1 << 30; }

            bit = ((lhs[3] >> 7) & 1) + ((rhs[3] >> 7) & 1);
            if (bit == 2) { if (carry) result |= 1 << 31; else carry = true; }
            else if (bit == 1 && !carry) result |= 1 << 31;
            else if (bit == 0 && carry) { carry = false; result |= 1 << 31; }

            return result;
        }

        /// <summary>
        /// Adds two variables presented as bytes together 
        /// and returns the result to the caller. This function supports
        /// max of 4-bytes per variable currently.
        /// </summary>
        /// <param name="lhs">left hand side bytes</param>
        /// <param name="rhs">right hand side bytes</param>
        /// <returns>lhs + rhs</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte[] AddBytes(byte[] lhs, byte[] rhs)
        {
            byte[] result = new byte[lhs.Length];

            bool carry = false;

            int bit = 0;

            if (lhs[0] != 0 && rhs[0] != 0)
            {
                bit = ((lhs[0] >> 0) & 1) + ((rhs[0] >> 0) & 1);
                carry = bit == 2;
                if (bit == 1 && !carry) result[0] |= 1 << 0;

                bit = ((lhs[0] >> 1) & 1) + ((rhs[0] >> 1) & 1);
                if (bit == 2) { if (carry) result[0] |= 1 << 1; else carry = true; }
                else if (bit == 1 && !carry) result[0] |= 1 << 1;
                else if (bit == 0 && carry) { carry = false; result[0] |= 1 << 1; }

                bit = ((lhs[0] >> 2) & 1) + ((rhs[0] >> 2) & 1);
                if (bit == 2) { if (carry) result[0] |= 1 << 2; else carry = true; }
                else if (bit == 1 && !carry) result[0] |= 1 << 2;
                else if (bit == 0 && carry) { carry = false; result[0] |= 1 << 2; }

                bit = ((lhs[0] >> 3) & 1) + ((rhs[0] >> 3) & 1);
                if (bit == 2) { if (carry) result[0] |= 1 << 3; else carry = true; }
                else if (bit == 1 && !carry) result[0] |= 1 << 3;
                else if (bit == 0 && carry) { carry = false; result[0] |= 1 << 3; }

                bit = ((lhs[0] >> 4) & 1) + ((rhs[0] >> 4) & 1);
                if (bit == 2) { if (carry) result[0] |= 1 << 4; else carry = true; }
                else if (bit == 1 && !carry) result[0] |= 1 << 4;
                else if (bit == 0 && carry) { carry = false; result[0] |= 1 << 4; }

                bit = ((lhs[0] >> 5) & 1) + ((rhs[0] >> 5) & 1);
                if (bit == 2) { if (carry) result[0] |= 1 << 5; else carry = true; }
                else if (bit == 1 && !carry) result[0] |= 1 << 5;
                else if (bit == 0 && carry) { carry = false; result[0] |= 1 << 5; }

                bit = ((lhs[0] >> 6) & 1) + ((rhs[0] >> 6) & 1);
                if (bit == 2) { if (carry) result[0] |= 1 << 6; else carry = true; }
                else if (bit == 1 && !carry) result[0] |= 1 << 6;
                else if (bit == 0 && carry) { carry = false; result[0] |= 1 << 6; }

                bit = ((lhs[0] >> 7) & 1) + ((rhs[0] >> 7) & 1);
                if (bit == 2) { if (carry) result[0] |= 1 << 7; else carry = true; }
                else if (bit == 1 && !carry) result[0] |= 1 << 7;
                else if (bit == 0 && carry) { carry = false; result[0] |= 1 << 7; }
            }

            if (lhs.Length >= 2)
            {
                if (lhs[1] == 0 && rhs[1] == 0)
                {
                    if (carry)
                    {
                        carry = false; 
                        result[1] |= 1 << 0;
                    }
                }
                else
                {
                    bit = ((lhs[1] >> 0) & 1) + ((rhs[1] >> 0) & 1);
                    if (bit == 2) { if (carry) result[1] |= 1 << 0; else carry = true; }
                    else if (bit == 1 && !carry) result[1] |= 1 << 0;
                    else if (bit == 0 && carry) { carry = false; result[1] |= 1 << 0; }

                    bit = ((lhs[1] >> 1) & 1) + ((rhs[1] >> 1) & 1);
                    if (bit == 2) { if (carry) result[1] |= 1 << 1; else carry = true; }
                    else if (bit == 1 && !carry) result[1] |= 1 << 1;
                    else if (bit == 0 && carry) { carry = false; result[1] |= 1 << 1; }

                    bit = ((lhs[1] >> 2) & 1) + ((rhs[1] >> 2) & 1);
                    if (bit == 2) { if (carry) result[1] |= 1 << 2; else carry = true; }
                    else if (bit == 1 && !carry) result[1] |= 1 << 2;
                    else if (bit == 0 && carry) { carry = false; result[1] |= 1 << 2; }

                    bit = ((lhs[1] >> 3) & 1) + ((rhs[1] >> 3) & 1);
                    if (bit == 2) { if (carry) result[1] |= 1 << 3; else carry = true; }
                    else if (bit == 1 && !carry) result[1] |= 1 << 3;
                    else if (bit == 0 && carry) { carry = false; result[1] |= 1 << 3; }

                    bit = ((lhs[1] >> 4) & 1) + ((rhs[1] >> 4) & 1);
                    if (bit == 2) { if (carry) result[1] |= 1 << 4; else carry = true; }
                    else if (bit == 1 && !carry) result[1] |= 1 << 4;
                    else if (bit == 0 && carry) { carry = false; result[1] |= 1 << 4; }

                    bit = ((lhs[1] >> 5) & 1) + ((rhs[1] >> 5) & 1);
                    if (bit == 2) { if (carry) result[1] |= 1 << 5; else carry = true; }
                    else if (bit == 1 && !carry) result[1] |= 1 << 5;
                    else if (bit == 0 && carry) { carry = false; result[1] |= 1 << 5; }

                    bit = ((lhs[1] >> 6) & 1) + ((rhs[1] >> 6) & 1);
                    if (bit == 2) { if (carry) result[1] |= 1 << 6; else carry = true; }
                    else if (bit == 1 && !carry) result[1] |= 1 << 6;
                    else if (bit == 0 && carry) { carry = false; result[1] |= 1 << 6; }

                    bit = ((lhs[1] >> 7) & 1) + ((rhs[1] >> 7) & 1);
                    if (bit == 2) { if (carry) result[1] |= 1 << 7; else carry = true; }
                    else if (bit == 1 && !carry) result[1] |= 1 << 7;
                    else if (bit == 0 && carry) { carry = false; result[1] |= 1 << 7; }
                }
            }
            else return result;

            if (lhs.Length >= 3)
            {
                if (lhs[2] == 0 && rhs[2] == 0)
                {
                    if (carry)
                    {
                        carry = false; 
                        result[2] |= 1 << 0;
                    }
                }
                else
                {

                    bit = ((lhs[2] >> 0) & 1) + ((rhs[2] >> 0) & 1);
                    if (bit == 2) { if (carry) result[2] |= 1 << 0; else carry = true; }
                    else if (bit == 1 && !carry) result[2] |= 1 << 0;
                    else if (bit == 0 && carry) { carry = false; result[2] |= 1 << 0; }

                    bit = ((lhs[2] >> 1) & 1) + ((rhs[2] >> 1) & 1);
                    if (bit == 2) { if (carry) result[2] |= 1 << 1; else carry = true; }
                    else if (bit == 1 && !carry) result[2] |= 1 << 1;
                    else if (bit == 0 && carry) { carry = false; result[2] |= 1 << 1; }

                    bit = ((lhs[2] >> 2) & 1) + ((rhs[2] >> 2) & 1);
                    if (bit == 2) { if (carry) result[2] |= 1 << 2; else carry = true; }
                    else if (bit == 1 && !carry) result[2] |= 1 << 2;
                    else if (bit == 0 && carry) { carry = false; result[2] |= 1 << 2; }

                    bit = ((lhs[2] >> 3) & 1) + ((rhs[2] >> 3) & 1);
                    if (bit == 2) { if (carry) result[2] |= 1 << 3; else carry = true; }
                    else if (bit == 1 && !carry) result[2] |= 1 << 3;
                    else if (bit == 0 && carry) { carry = false; result[2] |= 1 << 3; }

                    bit = ((lhs[2] >> 4) & 1) + ((rhs[2] >> 4) & 1);
                    if (bit == 2) { if (carry) result[2] |= 1 << 4; else carry = true; }
                    else if (bit == 1 && !carry) result[2] |= 1 << 4;
                    else if (bit == 0 && carry) { carry = false; result[2] |= 1 << 4; }

                    bit = ((lhs[2] >> 5) & 1) + ((rhs[2] >> 5) & 1);
                    if (bit == 2) { if (carry) result[2] |= 1 << 5; else carry = true; }
                    else if (bit == 1 && !carry) result[2] |= 1 << 5;
                    else if (bit == 0 && carry) { carry = false; result[2] |= 1 << 5; }

                    bit = ((lhs[2] >> 6) & 1) + ((rhs[2] >> 6) & 1);
                    if (bit == 2) { if (carry) result[2] |= 1 << 6; else carry = true; }
                    else if (bit == 1 && !carry) result[2] |= 1 << 6;
                    else if (bit == 0 && carry) { carry = false; result[2] |= 1 << 6; }

                    bit = ((lhs[2] >> 7) & 1) + ((rhs[2] >> 7) & 1);
                    if (bit == 2) { if (carry) result[2] |= 1 << 7; else carry = true; }
                    else if (bit == 1 && !carry) result[2] |= 1 << 7;
                    else if (bit == 0 && carry) { carry = false; result[2] |= 1 << 7; }
                }
            }
            else return result;

            if (lhs.Length >= 4)
            {
                if (lhs[3] == 0 && rhs[3] == 0)
                {
                    if (carry)
                    {
                        carry = false; 
                        result[3] |= 1 << 0;
                    }
                }
                else
                {
                    bit = ((lhs[3] >> 0) & 1) + ((rhs[3] >> 0) & 1);
                    if (bit == 2) { if (carry) result[3] |= 1 << 0; else carry = true; }
                    else if (bit == 1 && !carry) result[3] |= 1 << 0;
                    else if (bit == 0 && carry) { carry = false; result[3] |= 1 << 0; }

                    bit = ((lhs[3] >> 1) & 1) + ((rhs[3] >> 1) & 1);
                    if (bit == 2) { if (carry) result[3] |= 1 << 1; else carry = true; }
                    else if (bit == 1 && !carry) result[3] |= 1 << 1;
                    else if (bit == 0 && carry) { carry = false; result[3] |= 1 << 1; }

                    bit = ((lhs[3] >> 2) & 1) + ((rhs[3] >> 2) & 1);
                    if (bit == 2) { if (carry) result[3] |= 1 << 2; else carry = true; }
                    else if (bit == 1 && !carry) result[3] |= 1 << 2;
                    else if (bit == 0 && carry) { carry = false; result[3] |= 1 << 2; }

                    bit = ((lhs[3] >> 3) & 1) + ((rhs[3] >> 3) & 1);
                    if (bit == 2) { if (carry) result[3] |= 1 << 3; else carry = true; }
                    else if (bit == 1 && !carry) result[3] |= 1 << 3;
                    else if (bit == 0 && carry) { carry = false; result[3] |= 1 << 3; }

                    bit = ((lhs[3] >> 4) & 1) + ((rhs[3] >> 4) & 1);
                    if (bit == 2) { if (carry) result[3] |= 1 << 4; else carry = true; }
                    else if (bit == 1 && !carry) result[3] |= 1 << 4;
                    else if (bit == 0 && carry) { carry = false; result[3] |= 1 << 4; }

                    bit = ((lhs[3] >> 5) & 1) + ((rhs[3] >> 5) & 1);
                    if (bit == 2) { if (carry) result[3] |= 1 << 5; else carry = true; }
                    else if (bit == 1 && !carry) result[3] |= 1 << 5;
                    else if (bit == 0 && carry) { carry = false; result[3] |= 1 << 5; }

                    bit = ((lhs[3] >> 6) & 1) + ((rhs[3] >> 6) & 1);
                    if (bit == 2) { if (carry) result[3] |= 1 << 6; else carry = true; }
                    else if (bit == 1 && !carry) result[3] |= 1 << 6;
                    else if (bit == 0 && carry) { carry = false; result[3] |= 1 << 6; }

                    bit = ((lhs[3] >> 7) & 1) + ((rhs[3] >> 7) & 1);
                    if (bit == 2) { if (carry) result[3] |= 1 << 7; else carry = true; }
                    else if (bit == 1 && !carry) result[3] |= 1 << 7;
                    else if (bit == 0 && carry) { carry = false; result[3] |= 1 << 7; }
                }
            }
            
            return result;
        }

        public static int SubtractInt(byte[] lhs, byte[] rhs)
        {
            rhs = ByteHelper.Negate(rhs);

            return ByteHelper.AddInt(lhs, rhs);
        }

        public static byte[] SubtractBytes(byte[] lhs, byte[] rhs)
        {
            rhs = ByteHelper.Negate(rhs);

            return ByteHelper.AddBytes(lhs, rhs);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte[] Negate(byte[] bytes)
        {
            for (int i = 0; i < bytes.Length; i++) bytes[i] = (byte)~bytes[i];

            return ByteHelper.AddBytes(bytes, ByteHelper.GetOneByteArray((byte)bytes.Length));
        }

        public static string ToBinaryString(byte[] bytes)
        {
            string result = string.Empty;

            for (int i = bytes.Length - 1; i >= 0; i--)
            {
                result += Convert.ToString(bytes[i], 2).PadLeft(8, '0') + " ";
			}

            return result;
        }

        public static byte[] GetOneByteArray(byte size)
        {
            if (size == Sizes.HWORD)     return HWORDONE;
            if (size == Sizes.WORD)      return WORDONE;
            if (size == Sizes.LWORD)     return LWORDONE;
            if (size == Sizes.DWORD)     return DWORDONE;

            // TODO: implement the damn error handling system...
            throw new InvalidOperationException("Invalid size");
        }
    }
}
