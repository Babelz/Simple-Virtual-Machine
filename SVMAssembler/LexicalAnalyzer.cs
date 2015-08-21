using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SVMAssembler
{
    [Flags()]
    public enum ArgFlags : byte
	{
        Address     = 0,
        Identifier  = 1,
        Word        = 2,
        Number      = 4,
        Character   = 8,
        String      = 16,
        Register    = 32
	}
    
    public sealed class LexicalAnalyzer
    {
        public LexicalAnalyzer()
        {
        }

        private bool RequireArgs(Token token, int count)
        {
            return token.ArgumentsCount == count;
        }
        private bool RequireArgs(Token token, params ArgFlags[] flags)
        {
            for (int i = 0; i < flags.Length; i++)
            {
                string argument = token.ArgumentAtIndex(i);
                ArgFlags flag = flags[i];

                if ((flag & ArgFlags.Address) == ArgFlags.Address)
                {
                    // TODO: fix.
                }
                if ((flag & ArgFlags.Identifier) == ArgFlags.Identifier)
                {
                    // TODO: fix.
                }
                if ((flag & ArgFlags.Word) == ArgFlags.Word)
                {
                    if (!Statements.IsWord(argument)) return false;
                }
                if ((flag & ArgFlags.Number) == ArgFlags.Number)
                {
                    if (!Statements.IsNumber(argument)) return false;
                }
                if ((flag & ArgFlags.Character) == ArgFlags.Character)
                {
                    // TODO: fix.
                }
                if ((flag & ArgFlags.String) == ArgFlags.String)
                {
                    // TODO: fix.
                }
                if ((flag & ArgFlags.Register) == ArgFlags.Register)
                {
                    if (!Statements.IsRegister(argument)) return false;
                }
            }

            return true;
        }

        private void AnalyzePush(Token token)
        {
            if (token.Header == Instructions.Push)
            {
                if (!RequireArgs(token, 2)) Logger.Instance.LogError(ErrorHelper.TooFewArguments(token));
                if (!RequireArgs(token, ArgFlags.Word, ArgFlags.Number)) Logger.Instance.LogError(ErrorHelper.InvalidArgument(token));
            }
            else if (token.Header == Instructions.Push8)
            {
                if (!RequireArgs(token, 1)) Logger.Instance.LogError(ErrorHelper.TooFewArguments(token));
                if (!RequireArgs(token, ArgFlags.Number)) Logger.Instance.LogError(ErrorHelper.InvalidArgument(token));
            }
            else if (token.Header == Instructions.Push16)
            {
                if (!RequireArgs(token, 1)) Logger.Instance.LogError(ErrorHelper.TooFewArguments(token));
                if (!RequireArgs(token, ArgFlags.Number)) Logger.Instance.LogError(ErrorHelper.InvalidArgument(token));
            }
            else if (token.Header == Instructions.Push32)
            {
                if (!RequireArgs(token, 1)) Logger.Instance.LogError(ErrorHelper.TooFewArguments(token));
                if (!RequireArgs(token, ArgFlags.Number)) Logger.Instance.LogError(ErrorHelper.InvalidArgument(token));
            }
            else if (token.Header == Instructions.PushReg)
            {
                if (!RequireArgs(token, 1)) Logger.Instance.LogError(ErrorHelper.TooFewArguments(token));
                if (!RequireArgs(token, ArgFlags.Register)) Logger.Instance.LogError(ErrorHelper.InvalidArgument(token));
            }
        }

        public void Analyze(IEnumerable<Token> tokens)
        {
            foreach (Token token in tokens)
            {
                if (Logger.Instance.HasErrors) break;

                if (token.Type == TokenType.Opcode)
                {
                    if (Statements.IsPush(token.Header)) AnalyzePush(token);
                }
                else if (token.Type == TokenType.Declaration)
                {
                    // TODO: parse declarations.
                }
            }
        }
    }
}
