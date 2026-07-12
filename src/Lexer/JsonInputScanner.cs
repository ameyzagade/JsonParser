using System.Text;
using JsonParser.App.TokenModel;
using JsonParser.App.Lexer.Exceptions;
using System.Globalization;

namespace JsonParser.App.Lexer;

public sealed class JsonInputScanner
{
    private sealed class Cursor
    {
        public string? InputBuffer { get; init; }
        public char? CurrentCharacter { get; set; } = null;
        public int CurrentPosition { get; set; } = -1;
    }

    private sealed class Context
    {
        public Cursor Cursor { get; init; }
        public StringBuilder LexemeBuffer { get; set; } = new();
        public List<Token> GeneratedTokens { get; set; } = [];
    }

    public IReadOnlyList<Token> Tokenize(in string content)
    {
        Context context = IntializeContext(content);

        // Start reading the input buffer
        MoveNext(context);

        while (context.Cursor!.CurrentCharacter != null)
        {
            ScanNextToken(context);
        }

        ValidateLexerInvariant(context);
        Emit(context, TokenType.EndOfStream, string.Empty, context.Cursor.InputBuffer?.Length + 1 ?? 1);

        return context.GeneratedTokens;
    }

    private static Context IntializeContext(string content)
        => new()
        {
            Cursor = new Cursor
            {
                InputBuffer = content,
            },
        };

    private void ScanNextToken(Context context)
    {
        var currentCharacter = context.Cursor.CurrentCharacter;
        var tokenStartIndex = context.Cursor.CurrentPosition + 1;

        if (currentCharacter is char c && char.IsWhiteSpace(c))
        {
            MoveNext(context);
            return;
        }

        if (IsNumberStart(currentCharacter))
        {
            ReadNumber(context);
            return;
        }

        switch (context.Cursor.CurrentCharacter)
        {
            case '{':
                EmitAndAdvance(context, TokenType.LeftBrace, "{", tokenStartIndex);
                return;

            case '}':
                EmitAndAdvance(context, TokenType.RightBrace, "}", tokenStartIndex);
                return;

            case '[':
                EmitAndAdvance(context, TokenType.LeftBracket, "[", tokenStartIndex);
                return;

            case ']':
                EmitAndAdvance(context, TokenType.RightBracket, "]", tokenStartIndex);
                return;

            case ',':
                EmitAndAdvance(context, TokenType.Comma, ",", tokenStartIndex);
                return;

            case ':':
                EmitAndAdvance(context, TokenType.Colon, ":", tokenStartIndex);
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
        var tokenStartIndex = context.Cursor.CurrentPosition + 1;

        if (!MoveNext(context))
        {
            throw new JsonInputScannerException($"Input terminated after position: {tokenStartIndex} without an ending double quote!");
        }

        while (true)
        {
            var character = context.Cursor.CurrentCharacter;
            if (character == '"')
            {
                // since the string started with a double quote ",
                // this is the matching double quote to end the string
                EmitBufferedToken(context, TokenType.String, tokenStartIndex);
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
                // if there are no more characters in the input buffer, then stop reading further
                throw new JsonInputScannerException($"Input terminated after position: {tokenStartIndex} without an ending double quote!");
            }
        }
    }

    private void ReadEscapeCharacter(Context context)
    {
        var tokenStartIndex = context.Cursor.CurrentPosition + 1;

        if (!MoveNext(context))
        {
            throw new JsonInputScannerException($"Input terminated after position: {tokenStartIndex} in middle of an escape sequence!");
        }

        char? currentCharacter = context.Cursor.CurrentCharacter;
        switch (currentCharacter)
        {
            case '"' or '\\' or '/' or 'b' or 'f' or 'n' or 'r' or 't':
                context.LexemeBuffer.Append(currentCharacter);
                break;

            case 'u':
                ReadUnicodeEscapeSequence(context);
                break;

            default:
                throw new JsonInputScannerException($"Unknown escape character encountered at position: {tokenStartIndex + 1}. Received character: {currentCharacter}");
        }
    }

    private void ReadUnicodeEscapeSequence(Context context)
    {
        var tokenStartIndex = context.Cursor.CurrentPosition + 1;

        if (!MoveNext(context))
        {
            throw new JsonInputScannerException($"Input terminated after position: {tokenStartIndex} in middle of an unicode escape sequence!");
        }

        string unicodeSequence = GetUnicodeSequence(context);
        ValidateUnicodeSequence(context, unicodeSequence);
        int codepoint = GetUnicodeCodepoint(context, unicodeSequence);

        context.LexemeBuffer.Append((char)codepoint);

        SkipCharacter(context, 3);
    }

    private string GetUnicodeSequence(Context context)
    {
        var unicodeSequenceEndIndex = context.Cursor.CurrentPosition + 4;        // the unicode sequence has a length of 4
        if (unicodeSequenceEndIndex > context.Cursor.InputBuffer!.Length)
        {
            throw new JsonInputScannerException($"Input terminated after position: {context.Cursor.CurrentPosition + 1} in middle of an unicode escape sequence!");
        }

        return context.Cursor.InputBuffer[context.Cursor.CurrentPosition..unicodeSequenceEndIndex];
    }

    private void ValidateUnicodeSequence(Context context, string unicodeSequence)
    {
        if (!HasAllHexCharacters(unicodeSequence))
        {
            throw new JsonInputScannerException($"Unknown unicode escape sequence encountered at position: {context.Cursor.CurrentPosition + 1}. Received unicode sequence: {unicodeSequence}");
        }
    }

    private int GetUnicodeCodepoint(Context context, string unicodeSequence)
    {
        if (!int.TryParse(unicodeSequence, NumberStyles.HexNumber, CultureInfo.InvariantCulture, out int codePoint))
        {
            throw new JsonInputScannerException($"Couldn't determine the unicode escape sequence at position: {context.Cursor.CurrentPosition + 1}. Received unicode sequence: {unicodeSequence}");
        }

        return codePoint;
    }

    private void ReadIdentifier(Context context)
        => ReadToken(context, TokenType.Identifier, character => !IsIdentifierTerminator(character));

    private void ReadNumber(Context context)
        => ReadToken(context, TokenType.Number, character => !IsNumberTerminator(character));

    private void ReadInvalidToken(Context context)
        => ReadToken(context, TokenType.Invalid, character => !IsSymbolOrWhitespace(character));

    private void ReadToken(Context context, TokenType expectedTokenType, Func<char?, bool> isTokenCharacter)
    {
        var tokenStartIndex = context.Cursor.CurrentPosition + 1;

        // keep adding characters in lexeme buffer and moving forward till token boundary
        while (isTokenCharacter(context.Cursor.CurrentCharacter))
        {
            context.LexemeBuffer.Append(context.Cursor.CurrentCharacter);

            if (!MoveNext(context))
            {
                // if there are no more characters in the input buffer, then stop reading further and flush the lexeme buffer
                EmitBufferedToken(context, expectedTokenType, tokenStartIndex);
                return;
            }
        }

        // flush the lexeme buffer when token boundary is encountered
        EmitBufferedToken(context, expectedTokenType, tokenStartIndex);
    }

    private bool IsSymbol(char? value) => value is '{' or '}' or '[' or ']' or ',' or ':' or '"';

    private bool IsNumberStart(char? value) => value.HasValue && (value == '-' || char.IsDigit(value.Value));

    private bool IsIdentifierTerminator(char? character) => IsSymbol(character) || (character is char c && char.IsWhiteSpace(c)) || IsNumberStart(character);

    private bool IsSymbolOrWhitespace(char? character) => IsSymbol(character) || (character is char c && char.IsWhiteSpace(c));

    private bool HasAllHexCharacters(string slice)
    {
        foreach (var c in slice)
        {
            if (!((c >= '0' && c <= '9') ||
                (c >= 'a' && c <= 'z') ||
                (c >= 'A' && c <= 'Z'))
            )
            {
                return false;
            }
        }

        return true;
    }

    private bool IsNumberTerminator(char? character)
        => IsSymbol(character)
            || (character is char c && char.IsWhiteSpace(c))
            || (character.HasValue && char.IsAsciiLetter(character.Value));

    private void EmitBufferedToken(Context context, TokenType tokenType, int tokenStartIndex)
    {
        var value = context.LexemeBuffer.ToString();
        Emit(context, tokenType, value, tokenStartIndex);

        context.LexemeBuffer.Clear();
    }

    private void Emit(Context context, TokenType tokenType, string value, int tokenStartIndex)
        => context.GeneratedTokens.Add(new Token(tokenType, value, tokenStartIndex));

    private void EmitAndAdvance(Context context, TokenType tokenType, string value, int tokenStartIndex)
    {
        Emit(context, tokenType, value, tokenStartIndex);
        MoveNext(context);
    }

    private bool MoveNext(Context context)
    {
        context.Cursor.CurrentCharacter = context.Cursor.CurrentPosition >= context.Cursor.InputBuffer?.Length - 1
            ? null
            : context.Cursor.InputBuffer?[++context.Cursor.CurrentPosition];

        return context.Cursor.CurrentCharacter is not null;
    }

    private bool SkipCharacter(Context context, int total = 1)
    {
        for (int i = 0; i < total; i++)
        {
            if (!MoveNext(context))
            {
                return false;
            }
        }

        return true;
    }

    private void ValidateLexerInvariant(Context context)
    {
        if (context.LexemeBuffer.Length > 0)
        {
            throw new JsonInputScannerException($"Ended of input at position: {context.Cursor.CurrentPosition + 1} while reading token: {context.LexemeBuffer}");
        }
    }
}