using System.Text;
using JsonParser.App.Lexer.Exceptions;

namespace JsonParser.App.Lexer;

public sealed class JsonLexer
{
    private sealed class LexerContext
    {
        public string InputBuffer { get; init; } = string.Empty;
        public int CurrentPosition { get; set; } = -1;
        public char? CurrentCharacter { get; set; }
        public StringBuilder LexemeBuffer { get; set; } = new();
        public List<Token> GeneratedTokens { get; set; } = [];
    }

    public IReadOnlyList<Token> Tokenize(in string content)
    {
        var context = new LexerContext()
        {
            InputBuffer = content,
        };

        if (!MoveNext(context))
        {
            Emit(context, TokenType.EndOfStream, string.Empty);
            return context.GeneratedTokens;
        }

        while (context.CurrentCharacter != null)
        {
            ScanNextToken(context);
        }

        ValidateEndState(context);
        Emit(context, TokenType.EndOfStream, string.Empty);

        return context.GeneratedTokens;
    }

    private void ScanNextToken(LexerContext context)
    {
        if (context.CurrentCharacter is null)
        {
            return;
        }

        if (context.CurrentCharacter is char c && char.IsWhiteSpace(c))
        {
            MoveNext(context);
            return;
        }

        if (IsNumberStart(context.CurrentCharacter))
        {
            ReadNumber(context);
            return;
        }

        switch (context.CurrentCharacter)
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

    private void ReadString(LexerContext context)
    {
        if (!MoveNext(context))
        {
            throw new JsonLexerException($"Lexer input abruptly terminated at position: {context.CurrentPosition + 1} in the input JSON. No character encountered in the input after double quote!");
        }

        while (true)
        {
            var character = context.CurrentCharacter;
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
                context.LexemeBuffer.Append(character);
            }

            if (!MoveNext(context))
            {
                // if there are no more characters in the input buffer, then stop reading and exit immediately
                // and throw exception this has ended without a matching " character
                throw new JsonLexerException($"Lexer input abruptly terminated at position: {context.CurrentPosition + 1} in the input JSON. No closing double quote encountered!");
            }
        }
    }

    private void ReadEscapeCharacter(LexerContext context)
    {
        if (!MoveNext(context))
        {
            throw new JsonLexerException($"Lexer input abruptly terminated at position: {context.CurrentPosition + 1} in the input JSON. No character encountered in the input after the escape sequence!");
        }

        switch (context.CurrentCharacter)
        {
            case '"' or '\\' or '/' or 'b' or 'f' or 'n' or 'r' or 't':
                context.LexemeBuffer.Append(context.CurrentCharacter);
                break;

            default:
                throw new JsonLexerException($"Unknown escape character encountered at position: {context.CurrentPosition + 1} in the input JSON. Character: {context.CurrentCharacter}");
        }
    }

    private void ReadIdentifier(LexerContext context)
        => ReadToken(context, TokenType.Identifier, character => !IsIdentifierTerminator(character));

    private void ReadNumber(LexerContext context)
        => ReadToken(context, TokenType.Number, character => !IsNumberTerminator(character));

    private void ReadInvalidToken(LexerContext context)
        => ReadToken(context, TokenType.Invalid, character => !IsSymbolOrWhitespace(character));

    private void ReadToken(LexerContext context, TokenType expectedTokenType, Func<char?, bool> isTokenCharacter)
    {
        while (isTokenCharacter(context.CurrentCharacter))
        {
            context.LexemeBuffer.Append(context.CurrentCharacter);

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

    private bool MoveNext(LexerContext context)
    {
        if (context.CurrentPosition >= context.InputBuffer.Length - 1)
        {
            context.CurrentCharacter = null;
            return false;
        }

        context.CurrentCharacter = context.InputBuffer[++context.CurrentPosition];
        return true;
    }

    private void EmitBufferedToken(LexerContext context, TokenType tokenType)
    {
        var value = context.LexemeBuffer.ToString();
        Emit(context, tokenType, value);

        context.LexemeBuffer.Clear();
    }

    private void Emit(LexerContext context, TokenType tokenType, string value)
        => context.GeneratedTokens.Add(new Token(tokenType, value));

    private void EmitAndAdvance(LexerContext context, TokenType tokenType, string value)
    {
        Emit(context, tokenType, value);
        MoveNext(context);
    }

    private void ValidateEndState(LexerContext context)
    {
        if (context.LexemeBuffer.Length > 0)
        {
            throw new JsonLexerException($"Lexer abruptly ended in at position: {context.CurrentPosition + 1} in the JSON input. Last read lexeme: {context.LexemeBuffer}");
        }
    }
}