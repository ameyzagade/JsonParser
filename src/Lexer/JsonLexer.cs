using System.Text;
using JsonParser.App.Lexer.Exceptions;

namespace JsonParser.App.Lexer;

public sealed class JsonLexer
{
    private enum LexerState
    {
        Normal,
        Key,
        Value,
    }

    private sealed class LexerContext
    {
        public string InputBuffer { get; init; } = string.Empty;
        public int CurrentPosition { get; set; } = 0;
        public char CurrentCharacter { get; set; }
        public bool EscapeNext { get; set; } = false;
        public bool InQuote { get; set; } = false;
        public LexerState CurrentState { get; set; } = LexerState.Normal;
        public StringBuilder LexemeBuffer { get; set; } = new();
        public List<Token> GeneratedTokens { get; set; } = [];
    }

    public IReadOnlyList<Token> Tokenize(in string content)
    {
        var context = new LexerContext()
        {
            InputBuffer = content,
        };

        for (context.CurrentPosition = 0; context.CurrentPosition < content.Length; context.CurrentPosition++)
        {
            context.CurrentCharacter = content[context.CurrentPosition];
            Process(context);
        }

        ValidateEndState(context);

        return context.GeneratedTokens;
    }

    private void Process(LexerContext context)
    {
        switch (context.CurrentState)
        {
            case LexerState.Normal:
                ProcessNormalState(context);
                break;

            case LexerState.Key:
                ProcessKeyState(context);
                break;

            case LexerState.Value:
                ProcessValueState(context);
                break;

            default:
                throw new JsonLexerException($"Lexer is in an unknown state: {context.CurrentState}");
        }
    }

    private void ProcessNormalState(LexerContext context)
    {
        if (char.IsWhiteSpace(context.CurrentCharacter))
        {
            return;
        }

        switch (context.CurrentCharacter)
        {
            case '{':
                context.GeneratedTokens.Add(new Token(TokenType.LeftBrace, "{"));
                break;

            case '}':
                context.GeneratedTokens.Add(new Token(TokenType.RightBrace, "}"));
                break;

            case '"':
                context.CurrentState = LexerState.Key;
                break;

            case ':':
                context.GeneratedTokens.Add(new Token(TokenType.Colon, ":"));
                context.CurrentState = LexerState.Value;
                break;

            default:
                throw new JsonLexerException($"Unexpected character received at position {context.CurrentPosition}. Character: {context.CurrentCharacter}");
        }
    }

    private void ProcessKeyState(LexerContext context)
    {
        if (context.EscapeNext)
        {
            context.LexemeBuffer.Append(context.CurrentCharacter);
            context.EscapeNext = false;
            return;
        }

        switch (context.CurrentCharacter)
        {
            case '"':
                context.CurrentState = LexerState.Normal;
                FlushToken(context, TokenType.String);
                break;

            case '\\':
                ProcessEscapeSequence(context, context.CurrentCharacter);
                break;

            default:
                context.LexemeBuffer.Append(context.CurrentCharacter);
                break;
        }
    }

    private void ProcessValueState(LexerContext context)
    {
        if (!context.InQuote && char.IsWhiteSpace(context.CurrentCharacter))
        {
            return;
        }

        if (context.EscapeNext)
        {
            context.LexemeBuffer.Append(context.CurrentCharacter);
            context.EscapeNext = false;
            return;
        }

        switch (context.CurrentCharacter)
        {
            case '}':
                context.CurrentState = LexerState.Normal;
                FlushToken(context, TokenType.Value);
                break;

            case '"':
                if (!context.InQuote)
                {
                    context.InQuote = true;
                }
                else
                {
                    context.InQuote = false;
                    context.CurrentState = LexerState.Normal;
                    FlushToken(context, TokenType.Value);
                }
                break;

            case '\\':
                ProcessEscapeSequence(context, context.CurrentCharacter);
                break;

            default:
                context.LexemeBuffer.Append(context.CurrentCharacter);
                break;
        }
    }

    private void FlushToken(LexerContext context, TokenType type)
    {
        if (context.LexemeBuffer.Length == 0)
        {
            return;
        }

        var value = context.LexemeBuffer.ToString();
        context.LexemeBuffer.Clear();

        context.GeneratedTokens.Add(new Token(type, value));
    }

    private void ProcessEscapeSequence(LexerContext context, char currentCharacter)
    {
        var nextChar = Peek(context) ?? throw new JsonLexerException($"Input abruptly ends after character: {context.CurrentCharacter}, position: {context.CurrentPosition}");
        switch (nextChar)
        {
            case '"' or '\\' or '/' or 'b' or 'f' or 'n' or 'r' or 't':
                context.EscapeNext = true;
                break;

            default:
                context.LexemeBuffer.Append(currentCharacter);
                break;
        }
    }

    private char? Peek(LexerContext context, int offset = 1)
    {
        int position = context.CurrentPosition + offset;
        return position > context.InputBuffer.Length
                ? null
                : context.InputBuffer[position];
    }

    private void ValidateEndState(LexerContext context)
    {
        if (context.CurrentState != LexerState.Normal)
        {
            throw new JsonLexerException($"Lexer not in the expected {Enum.GetName(LexerState.Normal)} state after consuming all characters!");
        }
    }
}