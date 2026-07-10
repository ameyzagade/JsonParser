using System.Text;
using JsonParser.App.Lexer.Exceptions;

namespace JsonParser.App.Lexer;

public sealed class JsonLexer
{
    private struct CursorState
    {
        public string InputBuffer;
        public int CurrentPosition;
        public char? CurrentCharacter;

        public CursorState(string input, int position, char? character)
        {
            InputBuffer = input;
            CurrentPosition = position;
            CurrentCharacter = character;
        }
    }

    private struct Output
    {
        public StringBuilder LexemeBuffer;
        public List<Token> GeneratedTokens;

        public Output()
        {
            LexemeBuffer = new();
            GeneratedTokens = [];
        }
    }

    private sealed class Context
    {
        public CursorState CursorState;
        public Output Output;
    }

    public IReadOnlyList<Token> Tokenize(in string content)
    {
        var context = new Context()
        {
            CursorState = new(content, -1, '\0'),
            Output = new()
        };

        // Start reading the input buffer
        if (!MoveNext(context))
        {
            Emit(context, TokenType.EndOfStream, string.Empty);
            return context.Output.GeneratedTokens;
        }

        while (context.CursorState.CurrentCharacter != null)
        {
            ScanNextToken(context);
        }

        ValidateLexerInvariant(context);
        Emit(context, TokenType.EndOfStream, string.Empty);

        return context.Output.GeneratedTokens;
    }

    private void ScanNextToken(Context context)
    {
        if (context.CursorState.CurrentCharacter is null)
        {
            return;
        }

        if (context.CursorState.CurrentCharacter is char c && char.IsWhiteSpace(c))
        {
            MoveNext(context);
            return;
        }

        if (IsNumberStart(context.CursorState.CurrentCharacter))
        {
            ReadNumber(context);
            return;
        }

        switch (context.CursorState.CurrentCharacter)
        {
            case '{':
                EmitAndAdvance(context, TokenType.LeftBrace, "{");
                return;

            case '}':
                EmitAndAdvance(context, TokenType.RightBrace, "}");
                return;

            case '[':
                EmitAndAdvance(context, TokenType.LeftBracket, "[");
                return;

            case ']':
                EmitAndAdvance(context, TokenType.RightBracket, "]");
                return;

            case ',':
                EmitAndAdvance(context, TokenType.Comma, ",");
                return;

            case ':':
                EmitAndAdvance(context, TokenType.Colon, ":");
                return;

            case '"':
                ReadString(context);
                return;

            case 't':
            case 'f':
            case 'n':
                ReadIdentifier(context);
                return;
        }

        ReadInvalidToken(context);
    }

    private void ReadString(Context context)
    {
        if (!MoveNext(context))
        {
            throw new JsonLexerException($"Lexer input abruptly terminated at position: {context.CursorState.CurrentPosition + 1} in the input JSON. No character encountered in the input after double quote!");
        }

        while (true)
        {
            var character = context.CursorState.CurrentCharacter;
            if (character == '"')
            {
                // since the string started with a double quote ",
                // this is the matching double quote to end the string
                EmitBufferedToken(context, TokenType.String);
                MoveNext(context);
                break;
            }

            if (character == '\\')
            {
                ReadEscapeCharacter(context);
            }
            else
            {
                context.Output.LexemeBuffer.Append(character);
            }

            if (!MoveNext(context))
            {
                // if there are no more characters in the input buffer, then stop reading and exit immediately
                // and throw exception this has ended without a matching " character
                throw new JsonLexerException($"Lexer input abruptly terminated at position: {context.CursorState.CurrentPosition + 1} in the input JSON. No closing double quote encountered!");
            }
        }
    }

    private void ReadEscapeCharacter(Context context)
    {
        if (!MoveNext(context))
        {
            throw new JsonLexerException($"Lexer input abruptly terminated at position: {context.CursorState.CurrentPosition + 1} in the input JSON. No character encountered in the input after the escape sequence!");
        }

        switch (context.CursorState.CurrentCharacter)
        {
            case '"' or '\\' or '/' or 'b' or 'f' or 'n' or 'r' or 't':
                context.Output.LexemeBuffer.Append(context.CursorState.CurrentCharacter);
                break;

            default:
                throw new JsonLexerException($"Unknown escape character encountered at position: {context.CursorState.CurrentPosition + 1} in the input JSON. Character: {context.CursorState.CurrentCharacter}");
        }
    }

    private void ReadIdentifier(Context context)
        => ReadToken(context, TokenType.Identifier, character => !IsIdentifierTerminator(character));

    private void ReadNumber(Context context)
        => ReadToken(context, TokenType.Number, character => !IsNumberTerminator(character));

    private void ReadInvalidToken(Context context)
        => ReadToken(context, TokenType.Invalid, character => !IsSymbolOrWhitespace(character));

    private void ReadToken(Context context, TokenType expectedTokenType, Func<char?, bool> isTokenCharacter)
    {
        while (isTokenCharacter(context.CursorState.CurrentCharacter))
        {
            context.Output.LexemeBuffer.Append(context.CursorState.CurrentCharacter);

            if (!MoveNext(context))
            {
                // if there are no more characters in the input buffer, then stop reading and exit immediately
                // and flush the current lexeme buffer
                EmitBufferedToken(context, expectedTokenType);
                return;
            }
        }

        EmitBufferedToken(context, expectedTokenType);
    }

    private bool IsSymbol(char? value)
        => value is '{' or '}' or '[' or ']' or ',' or ':' or '"';

    private bool IsNumberStart(char? value)
        => value.HasValue && (value == '-' || char.IsDigit(value.Value));

    private bool IsIdentifierTerminator(char? character)
        => IsSymbol(character)
            || (character is char c && char.IsWhiteSpace(c))
            || IsNumberStart(character);

    private bool IsSymbolOrWhitespace(char? character)
        => IsSymbol(character) || (character is char c && char.IsWhiteSpace(c));

    private bool IsNumberTerminator(char? character)
        => IsSymbol(character)
            || (character is char c && char.IsWhiteSpace(c))
            || (character.HasValue && char.IsAsciiLetter(character.Value));

    private bool MoveNext(Context context)
    {
        if (context.CursorState.CurrentPosition >= context.CursorState.InputBuffer.Length - 1)
        {
            context.CursorState.CurrentCharacter = null;
            return false;
        }

        context.CursorState.CurrentCharacter = context.CursorState.InputBuffer[++context.CursorState.CurrentPosition];
        return true;
    }

    private void EmitBufferedToken(Context context, TokenType tokenType)
    {
        var value = context.Output.LexemeBuffer.ToString();
        Emit(context, tokenType, value);

        context.Output.LexemeBuffer.Clear();
    }

    private void Emit(Context context, TokenType tokenType, string value)
        => context.Output.GeneratedTokens.Add(new Token(tokenType, value));

    private void EmitAndAdvance(Context context, TokenType tokenType, string value)
    {
        Emit(context, tokenType, value);
        MoveNext(context);
    }

    private void ValidateLexerInvariant(Context context)
    {
        if (context.Output.LexemeBuffer.Length > 0)
        {
            throw new JsonLexerException($"Lexer abruptly ended in at position: {context.CursorState.CurrentPosition + 1} in the JSON input. Last read lexeme: {context.Output.LexemeBuffer}");
        }
    }
}