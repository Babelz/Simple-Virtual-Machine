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

        private bool RequireArgCount(Token token, int count)
        {
            return token.ArgumentsCount == count;
        }
        private bool RequireArgTypes(Token token, params ArgFlags[] flags)
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
            if (token.Header == Mnemonics.Push)
            {
                if (!RequireArgCount(token, 2)) Logger.Instance.LogError(ErrorHelper.TooFewArguments(token));
                if (!RequireArgTypes(token, ArgFlags.Word, ArgFlags.Number)) Logger.Instance.LogError(ErrorHelper.InvalidArgument(token));
            }
            else if (token.Header == Mnemonics.Push8)
            {
                if (!RequireArgCount(token, 1)) Logger.Instance.LogError(ErrorHelper.TooFewArguments(token));
                if (!RequireArgTypes(token, ArgFlags.Number)) Logger.Instance.LogError(ErrorHelper.InvalidArgument(token));
            }
            else if (token.Header == Mnemonics.Push16)
            {
                if (!RequireArgCount(token, 1)) Logger.Instance.LogError(ErrorHelper.TooFewArguments(token));
                if (!RequireArgTypes(token, ArgFlags.Number)) Logger.Instance.LogError(ErrorHelper.InvalidArgument(token));
            }
            else if (token.Header == Mnemonics.Push32)
            {
                if (!RequireArgCount(token, 1)) Logger.Instance.LogError(ErrorHelper.TooFewArguments(token));
                if (!RequireArgTypes(token, ArgFlags.Number)) Logger.Instance.LogError(ErrorHelper.InvalidArgument(token));
            }
            else if (token.Header == Mnemonics.PushReg)
            {
                if (!RequireArgCount(token, 1)) Logger.Instance.LogError(ErrorHelper.TooFewArguments(token));
                if (!RequireArgTypes(token, ArgFlags.Register)) Logger.Instance.LogError(ErrorHelper.InvalidArgument(token));
            }
        }
        private void AnalyzePop(Token token)
        {
            if (token.Header == Mnemonics.Pop)
            {
                if (!RequireArgCount(token, 1)) Logger.Instance.LogError(ErrorHelper.TooFewArguments(token));
                if (!RequireArgTypes(token, ArgFlags.Number)) Logger.Instance.LogError(ErrorHelper.InvalidArgument(token));
            }
            else if (token.Header == Mnemonics.PopReg)
            {
                if (!RequireArgCount(token, 1)) Logger.Instance.LogError(ErrorHelper.TooFewArguments(token));
                if (!RequireArgTypes(token, ArgFlags.Register)) Logger.Instance.LogError(ErrorHelper.InvalidArgument(token));
            }
            else if (token.Header == Mnemonics.Pop8)
            {
                if (!RequireArgCount(token, 0)) Logger.Instance.LogError(ErrorHelper.TooFewArguments(token));
            }
            else if (token.Header == Mnemonics.Pop16)
            {
                if (!RequireArgCount(token, 0)) Logger.Instance.LogError(ErrorHelper.TooFewArguments(token));
            }
            else if (token.Header == Mnemonics.Pop32)
            {
                if (!RequireArgCount(token, 0)) Logger.Instance.LogError(ErrorHelper.TooFewArguments(token));
            }
        }
        private void AnalyzeLdCh(Token token)
        {
            throw new NotImplementedException();
        }
        private void AnalyzeLdStr(Token token)
        {
            throw new NotImplementedException();
        }
        private void AnalyzePushb(Token token)
        {
            throw new NotImplementedException();
        }
        private void AnalyzeSp(Token token)
        {
            throw new NotImplementedException();
        }
        private void AnalyzeTop(Token token)
        {
            if (token.Header == Mnemonics.Top)
            {
                if (!RequireArgCount(token, 2)) Logger.Instance.LogError(ErrorHelper.TooFewArguments(token));
                if (!RequireArgTypes(token, ArgFlags.Number, ArgFlags.Register)) Logger.Instance.LogError(ErrorHelper.InvalidArgument(token));
            
                // Validate sizes.
            }
        }

        public void Analyze(IEnumerable<Token> tokens)
        {
            Logger.Instance.LogMessage("Analyzing...");

            foreach (Token token in tokens)
            {
                if (Logger.Instance.HasErrors) break;

                if (token.Type == TokenType.Opcode)
                {
                    if (Statements.IsPush(token.Header)) AnalyzePush(token);
                    else if (Statements.IsPop(token.Header)) AnalyzePop(token);
                    else if (Statements.IsTop(token.Header)) AnalyzeTop(token);
                    else if (Statements.IsSp(token.Header)) AnalyzeSp(token);
                    else if (Statements.IsPushb(token.Header)) AnalyzePushb(token);
                    else if (Statements.IsLdStr(token.Header)) AnalyzeLdStr(token);
                    else if (Statements.IsLdCh(token.Header)) AnalyzeLdCh(token);
                
                }
                else if (token.Type == TokenType.Declaration)
                {
                    // TODO: parse declarations.
                }
            }
        }
    }
}
