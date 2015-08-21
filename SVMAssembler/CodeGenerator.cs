using SVM;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SVMAssembler
{
    /// <summary>
    /// Class that emits bytecode from input strings.
    /// </summary>
    public sealed class CodeGenerator
    {
        #region Private generator class 
        private sealed class Generator
        {
            #region Fields
            private readonly BytecodeBuffer buffer;

            private readonly Func<string, bool> predicate;
            private readonly Action<Token> generator;
            #endregion

        }
        #endregion

        #region Fields
        private readonly Dictionary<Func<string, bool>, Action<Token>> generators;

        private readonly BytecodeBuffer buffer;
        #endregion

        public CodeGenerator()
        {
            buffer = new BytecodeBuffer();

            generators = new Dictionary<Func<string, bool>, Action<Token>>()
            {
                { Statements.IsPush, GeneratePush }
            };
        }

        private void GeneratePush(Token token)
        {
            /*
             * push	    <word><value>
             * push8	<value> 
             * push16	<value>
             * push32	<value>
             * pushreg  <register_name>
             */

            switch (token.Header)
            {
                case "push":
                    byte bytesCount = byte.Parse(token.ArgumentAtIndex(0));
                    byte[] bytes = StringHelper.ToBytes(token.ArgumentAtIndex(1), bytesCount);

                    buffer.AddBytes(Bytecodes.Push_Direct, bytesCount).AddBytes(bytes);
                    break;
                case "push8":
                    long testValue = long.Parse(token.ArgumentAtIndex(0));

                    if (testValue < 0 || testValue > byte.MaxValue) Logger.Instance.LogWarning(ErrorHelper.SizeMisMatch(token, sizeof(byte)));

                    byte byteValue = (byte)testValue;

                    buffer.AddBytes(Bytecodes.Push_Direct, Sizes.HWORD).AddValue(byteValue, 1);
                    break;
                case "push16":
                    testValue = long.Parse(token.ArgumentAtIndex(0));

                    if (testValue > short.MaxValue) Logger.Instance.LogWarning(ErrorHelper.SizeMisMatch(token, sizeof(short)));

                    short shortValue = (short)testValue;

                    buffer.AddBytes(Bytecodes.Push_Direct, Sizes.WORD).AddValue(shortValue, 2);
                    break;
                case "push32":
                    testValue = long.Parse(token.ArgumentAtIndex(0));

                    if (testValue > int.MaxValue) Logger.Instance.LogWarning(ErrorHelper.SizeMisMatch(token, sizeof(int)));

                    int intValue = (int)testValue;

                    buffer.AddBytes(Bytecodes.Push_Direct, Sizes.LWORD).AddValue(intValue, 4);
                    break;
                case "pushreg":
                    byte register = StringHelper.RegisterToByte(token.ArgumentAtIndex(0));

                    buffer.AddBytes(Bytecodes.Push_Register, register);
                    break;
                default:
                    break;
            }
        }
    }
}
