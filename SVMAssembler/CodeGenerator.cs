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
        #region Private Generator class 
        private sealed class Generator
        {
            #region Fields
            private readonly BytecodeBuffer buffer;

            private readonly Func<string, bool> predicate;
            private readonly Action<Token> generator;
            #endregion

            public Generator(BytecodeBuffer buffer, Func<string, bool> predicate, Action<Token> generator)
            {
                this.buffer = buffer;
                this.predicate = predicate;
                this.generator = generator;
            }

            public bool Matches(Token token)
            {
                return predicate(token.Header);
            }
            public void Generate(Token token)
            {
                generator(token);
            }
        }
        #endregion

        #region Fields
        private readonly List<Generator> generators;

        private readonly BytecodeBuffer buffer;
        #endregion

        public CodeGenerator()
        {
            buffer = new BytecodeBuffer();

            generators = new List<Generator>()
            {
                new Generator(buffer, Statements.IsPush, GeneratePush),
                new Generator(buffer, Statements.IsPop, GeneratePop)
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
                    byte bytesCount = StringHelper.WordToByte(token.ArgumentAtIndex(0));
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
                case "top":
                    register = StringHelper.RegisterToByte(token.ArgumentAtIndex(1));
                    bytesCount = ByteHelper.Sizeof(long.Parse(token.ArgumentAtIndex(0)));

                    buffer.AddBytes(Bytecodes.Top, bytesCount, register);
                    break;
                case "ldch":
                    string str = token.ArgumentAtIndex(0);
                    str = str.Substring(str.IndexOf('\''), 1);

                    byteValue = (byte)str.First();

                    buffer.AddBytes(Bytecodes.Push_Direct, Sizes.HWORD, byteValue);
                    break;
                case "ldstr":
                    str = token.ArgumentAtIndex(0);
                    str = str.Replace("\"", "");
                    str = str.Trim();

                    bytes = Encoding.ASCII.GetBytes(str);

                    byte elementSize = 1;
                    int elementsCount = bytes.Length;
                    byte elementsCountSize = ByteHelper.Sizeof(elementsCount);

                    byte[] elementsCountBytes = new byte[elementsCountSize];
                    ByteHelper.ToBytes(elementsCount, elementsCountBytes);

                    buffer.AddBytes(Bytecodes.Push_Bytes, elementSize, elementsCountSize).AddBytes(elementsCountBytes).AddBytes(bytes);
                    break;
                default:
                    break;
            }
        }
        private void GeneratePop(Token token)
        {
            switch (token.Header)
            {
                case "pop":
                    string arg = token.ArgumentAtIndex(0);

                    if (Statements.IsRegister(arg))
                    {
                        byte register = StringHelper.RegisterToByte(arg);

                        buffer.AddBytes(Bytecodes.Pop_Register, register);
                    }
                    else
                    {
                        byte value = byte.Parse(arg);

                        buffer.AddBytes(Bytecodes.Pop, value);
                    }
                    break;
                case "pop8":
                    buffer.AddBytes(Bytecodes.Pop, Sizes.HWORD);
                    break;
                case "pop16":
                    buffer.AddBytes(Bytecodes.Pop, Sizes.WORD);
                    break;
                case "pop32":
                    buffer.AddBytes(Bytecodes.Pop, Sizes.LWORD);
                    break;
                default:
                    break;
            }
        }

        public BytecodeBuffer GenerateCode(IEnumerable<Token> tokens)
        {
            Logger.Instance.LogMessage("Generating code...");

            buffer.Clear();

            foreach (Token token in tokens)
            {
                Generator generator = generators.Find(g => g.Matches(token));
                generator.Generate(token);
            }

            return buffer;
        }
    }
}
