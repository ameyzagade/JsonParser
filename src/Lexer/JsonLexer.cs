using System.Text;
using JsonParser.App.Lexer.Exceptions;

namespace JsonParser.App.Lexer;

public sealed class JsonLexer
{
    private enum LexerState
    {
        Normal,
        ValueStart,
        StringValue,
        PrimitiveValue,
        AfterPrimitive
    }

    private sealed class LexerContext
    {
        public string InputBuffer { get; init; } = string.Empty;
        public int CurrentPosition { get; set; } = 0;
        public char CurrentCharacter { get; set; }
        public bool EscapeNext { get; set; } = false;
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

            case LexerState.ValueStart:
                ProcessValueStartState(context);
                break;

            case LexerState.StringValue:
                ProcessStringValueState(context);
                break;

            case LexerState.PrimitiveValue:
                ProcessPrimitiveValueState(context);
                break;

            case LexerState.AfterPrimitive:
                ProcessAfterPrimitiveState(context);
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
                Emit(context, TokenType.LeftBrace, "{");
                break;

            case '}':
                Emit(context, TokenType.RightBrace, "}");
                break;

            case '"':
                context.CurrentState = LexerState.StringValue;
                break;

            case ',':
                Emit(context, TokenType.Comma, ",");
                break;

            case ':':
                Emit(context, TokenType.Colon, ":");
                context.CurrentState = LexerState.ValueStart;
                break;

            default:
                throw new JsonLexerException($"Unexpected character received at position {context.CurrentPosition + 1}. Current Character: {context.CurrentCharacter}. Expected Character(s): Any whitespace, right brace, left brace, double quote, comma or colon.");
        }
    }

    private void ProcessValueStartState(LexerContext context)
    {
        if (char.IsWhiteSpace(context.CurrentCharacter))
        {
            return;
        }

        switch (context.CurrentCharacter)
        {
            case '"':
                context.CurrentState = LexerState.StringValue;
                break;

            default:
                context.LexemeBuffer.Append(context.CurrentCharacter);
                context.CurrentState = LexerState.PrimitiveValue;
                break;
        }
    }

    private void ProcessStringValueState(LexerContext context)
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
                FlushToken(context, TokenType.String);
                context.CurrentState = LexerState.Normal;
                break;

            case '\\':
                ProcessEscapeSequence(context, context.CurrentCharacter);
                break;

            default:
                context.LexemeBuffer.Append(context.CurrentCharacter);
                break;
        }
    }

    private void ProcessPrimitiveValueState(LexerContext context)
    {
        if (char.IsWhiteSpace(context.CurrentCharacter))
        {
            FlushToken(context, TokenType.PrimitiveValue);
            context.CurrentState = LexerState.AfterPrimitive;
            return;
        }

        switch (context.CurrentCharacter)
        {
            case ',':
                FlushToken(context, TokenType.PrimitiveValue);
                Emit(context, TokenType.Comma, ",");
                context.CurrentState = LexerState.Normal;
                break;

            case '}':
                FlushToken(context, TokenType.PrimitiveValue);
                Emit(context, TokenType.RightBrace, "}");
                context.CurrentState = LexerState.Normal;
                break;

            case '"':
            case '\\':
                throw new JsonLexerException($"Unexpected character '{context.CurrentCharacter}' inside a primitive value at position {context.CurrentPosition + 1}.");

            default:
                context.LexemeBuffer.Append(context.CurrentCharacter);
                break;
        }
    }

    private void ProcessAfterPrimitiveState(LexerContext context)
    {
        if (char.IsWhiteSpace(context.CurrentCharacter))
        {
            return;
        }

        switch (context.CurrentCharacter)
        {
            case ',':
                Emit(context, TokenType.Comma, ",");
                context.CurrentState = LexerState.Normal;
                break;

            case '}':
                Emit(context, TokenType.RightBrace, "}");
                context.CurrentState = LexerState.Normal;
                break;

            default:
                throw new JsonLexerException($"Expected ',' or '}}' after primitive value at position {context.CurrentPosition + 1}.");
        }
    }

    private void Emit(LexerContext context, TokenType tokenType, string value)
        => context.GeneratedTokens.Add(new Token(tokenType, value));

    private void FlushToken(LexerContext context, TokenType type)
    {
        var value = context.LexemeBuffer.ToString();
        Emit(context, type, value);

        context.LexemeBuffer.Clear();
    }

    private void ProcessEscapeSequence(LexerContext context, char currentCharacter)
    {
        var nextChar = Peek(context) ?? throw new JsonLexerException($"Input abruptly ends after position: {context.CurrentPosition + 1} having character: {context.CurrentCharacter}. Expected character(s): Any escape character.");
        context.EscapeNext = nextChar switch
        {
            '"' or '\\' or '/' or 'b' or 'f' or 'n' or 'r' or 't' => true,
            _ => throw new JsonLexerException($"Invalid escape sequence '\\{nextChar}' at position {context.CurrentPosition + 1}"),
        };
    }

    private char? Peek(LexerContext context, int offset = 1)
    {
        int position = context.CurrentPosition + offset;
        return position >= context.InputBuffer.Length
                ? null
                : context.InputBuffer[position];
    }

    private void ValidateEndState(LexerContext context)
    {
        if (context.EscapeNext)
        {
            throw new JsonLexerException("Input ended in the middle of an escape sequence.");
        }

        if (context.CurrentState != LexerState.Normal)
        {
            throw new JsonLexerException($"Lexer not in the expected {Enum.GetName(LexerState.Normal)} state after consuming all characters!");
        }

        if (context.LexemeBuffer.Length > 0)
        {
            throw new JsonLexerException("Input ended with an unflushed token.");
        }
    }
}