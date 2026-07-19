using JsonParser.App.TokenModel;
using JsonParser.App.Lexer.Exceptions;
using System.Globalization;

namespace JsonParser.App.Lexer;

public sealed class JsonInputScanner
{
    private const int WhiteSpaceAsciiHexCode = 0x20;
    private const int UnicodeHexDigitCount = 4;

    private sealed class Context
    {
        public Cursor Cursor { get; init; }
        public LexerOutput Output { get; init; }
    }

    public IReadOnlyList<Token> Tokenize(in string content)
    {
        var context = new Context
        {
            Cursor = new(content),
            Output = new(),
        };

        // Prime the input buffer
        context.Cursor.Advance();

        while (context.Cursor.Character != null)
        {
            ScanNextToken(context);
        }

        ValidateLexerInvariant(context);
        context.Output.Emit(TokenType.EndOfStream, string.Empty, context.Cursor.InputBuffer.Length + 1);

        return context.Output.GeneratedTokens;
    }

    private void ScanNextToken(Context context)
    {
        var currentCharacter = context.Cursor.Character;
        if (currentCharacter is char c && char.IsWhiteSpace(c))
        {
            context.Cursor.Advance();
            return;
        }

        // Every Read*() below identifies and emits a token and stops the cursor at next unconsumed character
        switch (context.Cursor.Character)
        {
            case '{':
                context.Output.Emit(TokenType.LeftBrace, "{", context.Cursor.OneBasedPosition);
                context.Cursor.Advance();
                return;

            case '}':
                context.Output.Emit(TokenType.RightBrace, "}", context.Cursor.OneBasedPosition);
                context.Cursor.Advance();
                return;

            case '[':
                context.Output.Emit(TokenType.LeftBracket, "[", context.Cursor.OneBasedPosition);
                context.Cursor.Advance();
                return;

            case ']':
                context.Output.Emit(TokenType.RightBracket, "]", context.Cursor.OneBasedPosition);
                context.Cursor.Advance();
                return;

            case ',':
                context.Output.Emit(TokenType.Comma, ",", context.Cursor.OneBasedPosition);
                context.Cursor.Advance();
                return;

            case ':':
                context.Output.Emit(TokenType.Colon, ":", context.Cursor.OneBasedPosition);
                context.Cursor.Advance();
                return;

            case '"':
                ReadString(context);
                return;

            case 't':
            case 'f':
            case 'n':
                ReadIdentifier(context);
                return;

            case '-' or '0' or '1' or '2' or '3' or '4' or '5' or '6' or '7' or '8' or '9':
                ReadNumber(context);
                break;
        }

        ReadInvalidToken(context);
    }

    private void ReadNumber(Context context)
    {
        var tokenStartIndex = context.Cursor.OneBasedPosition;

        // JSON number: -? integer fraction? exponent?
        if (context.Cursor.Character == '-')
        {
            AppendCurrentAndAdvance(context);
        }

        ReadUnsignedInteger(context);
        if (context.Cursor.Character is char commaChar &&
            commaChar == '.')
        {
            ReadFraction(context);
        }

        if (context.Cursor.Character is char exponentSignChar &&
            (exponentSignChar == 'e' || exponentSignChar == 'E'))
        {
            ReadExponent(context);
        }

        context.Output.EmitBufferedToken(TokenType.Number, tokenStartIndex);
    }

    private void ReadUnsignedInteger(Context context)
    {
        if (context.Cursor.Character == '0')
        {
            var tokenStartIndex = context.Cursor.OneBasedPosition;

            AppendCurrentAndAdvance(context);
            if (context.Cursor.Character is char c &&
                char.IsDigit(c))
            {
                throw new JsonInputScannerException($"Leading zeros are not permitted. Received '0{c}' at position {tokenStartIndex}.");
            }
        }
        else
        {
            ReadDigits(context);
        }
    }

    private void ReadFraction(Context context)
    {
        AppendCurrentAndAdvance(context);
        ReadDigits(context);
    }

    private void ReadExponent(Context context)
    {
        AppendCurrentAndAdvance(context);
        if (context.Cursor.Character is null)
        {
            throw new JsonInputScannerException($"Expected either [0-9] digits or + (plus) or - (minus) after e or E (exponential sign) at position {context.Cursor.OneBasedPosition}, but reached the end of input.");
        }

        if (context.Cursor.Character == '-' ||
            context.Cursor.Character == '+')
        {
            AppendCurrentAndAdvance(context);
        }

        ReadDigits(context);
    }

    private void ReadDigits(Context context)
    {
        if (context.Cursor.Character is not char c)
        {
            throw new JsonInputScannerException($"Expected [0-9] (digit) at position {context.Cursor.OneBasedPosition}, but reached the end of input.");
        }

        if (!char.IsDigit(c))
        {
            throw new JsonInputScannerException($"Expected [0-9] (digit) at position {context.Cursor.OneBasedPosition}, but received '{c}'.");
        }

        while (context.Cursor.Character is char ch &&
                char.IsDigit(ch))
        {
            context.Output.LexemeBuffer.Append(ch);

            if (!context.Cursor.Advance())
            {
                break;
            }
        }
    }

    private void ReadString(Context context)
    {
        var tokenStartIndex = context.Cursor.OneBasedPosition;
        if (!context.Cursor.Advance())
        {
            throw new JsonInputScannerException($"Input terminated after position: {tokenStartIndex} without an ending double quote!");
        }

        while (true)
        {
            var character = context.Cursor.Character;
            if (character == '"')
            {
                // since the string started with a double quote ", this is the matching double quote to end the string
                context.Output.EmitBufferedToken(TokenType.String, tokenStartIndex);
                context.Cursor.Advance();
                break;
            }

            if (character < WhiteSpaceAsciiHexCode)
            {
                throw new JsonInputScannerException($"Control characters encountered at position: {context.Cursor.OneBasedPosition}.");
            }
            else if (character == '\\')
            {
                ReadEscapeCharacter(context);
            }
            else
            {
                context.Output.LexemeBuffer.Append(character);

                if (!context.Cursor.Advance())
                {
                    // if there are no more characters in the input buffer, then stop reading further
                    throw new JsonInputScannerException($"Input terminated after position: {context.Cursor.OneBasedPosition} without an ending double quote!");
                }
            }
        }
    }

    private void ReadEscapeCharacter(Context context)
    {
        if (!context.Cursor.Advance())
        {
            throw new JsonInputScannerException($"Input terminated after position: {context.Cursor.OneBasedPosition} in middle of an escape sequence!");
        }

        switch (context.Cursor.Character)
        {
            case '"' or '\\' or '/' or 'b' or 'f' or 'n' or 'r' or 't':
                ReadNormalEscapeCharacter(context);
                break;

            case 'u':
                ReadUnicodeEscapeSequence(context);
                break;

            default:
                throw new JsonInputScannerException($"Unknown escape character encountered at position: {context.Cursor.OneBasedPosition}. Received character: {context.Cursor.Character}");
        }
    }

    private void ReadNormalEscapeCharacter(Context context)
    {
        var decodedCharacter = context.Cursor.Character switch
        {
            '"' => '"',
            '\\' => '\\',
            '/' => '/',
            'b' => '\b',
            'f' => '\f',
            'n' => '\n',
            'r' => '\r',
            't' => '\t',
        };

        context.Output.LexemeBuffer.Append(decodedCharacter);

        if (!context.Cursor.Advance())
        {
            // if there are no more characters in the input buffer, then stop reading further
            throw new JsonInputScannerException($"Input terminated after position: {context.Cursor.OneBasedPosition} without an ending double quote!");
        }
    }

    private void ReadUnicodeEscapeSequence(Context context)
    {
        // Entered with cursor on 'u'.
        if (!context.Cursor.Advance())
        {
            throw new JsonInputScannerException($"Input terminated after position: {context.Cursor.OneBasedPosition} in middle of a Unicode escape sequence!");
        }

        var firstCodeUnitStartPosition = context.Cursor.OneBasedPosition;
        var firstCodeUnit = ReadUnicodeCodeUnit(context);
        if (char.IsLowSurrogate(firstCodeUnit))
        {
            throw new JsonInputScannerException($"The Unicode code unit \\u{(int)firstCodeUnit:X4} at position {firstCodeUnitStartPosition} is a low surrogate and cannot appear without a preceding high surrogate.");
        }

        if (!char.IsHighSurrogate(firstCodeUnit))
        {
            // the Unicode codeunit is neither a low or high surrogate but is a normal Unicode codeunit
            context.Output.LexemeBuffer.Append(firstCodeUnit);
            return;
        }

        // at this point, the surrogate is a high surrogate and must be followed by a low surrogate
        ReadExpectedCharacter(context, '\\');
        ReadExpectedCharacter(context, 'u');

        var secondCodeUnitStartPosition = context.Cursor.OneBasedPosition;
        var secondCodeUnit = ReadUnicodeCodeUnit(context);
        if (!char.IsLowSurrogate(secondCodeUnit))
        {
            throw new JsonInputScannerException($"The Unicode code unit \\u{(int)secondCodeUnit:X4} at position {secondCodeUnitStartPosition} is not a low surrogate. A high surrogate must be followed by a low surrogate.");
        }

        context.Output.LexemeBuffer.Append(firstCodeUnit);
        context.Output.LexemeBuffer.Append(secondCodeUnit);
    }

    private char ReadUnicodeCodeUnit(Context context)
    {
        if (!context.Cursor.TryPeek(context.Cursor.Position, UnicodeHexDigitCount, out var codeUnitSequence))
        {
            throw new JsonInputScannerException($"Input terminated after position: {context.Cursor.OneBasedPosition} in middle of a Unicode code unit!");
        }

        if (!int.TryParse(codeUnitSequence, NumberStyles.HexNumber, CultureInfo.InvariantCulture, out int codeunit))
        {
            throw new JsonInputScannerException($"Invalid Unicode code unit at position {context.Cursor.OneBasedPosition}. Expected four hexadecimal digits but received '\\u{codeUnitSequence}'.");
        }

        context.Cursor.AdvanceBy(UnicodeHexDigitCount);

        return (char)codeunit;
    }

    private void ReadIdentifier(Context context)
        => ReadToken(context, TokenType.Identifier, character => !IsIdentifierTerminator(character!.Value));

    private void ReadInvalidToken(Context context)
        => ReadToken(context, TokenType.Invalid, character => !IsSymbolOrWhitespace(character));

    private void ReadToken(Context context, TokenType expectedTokenType, Func<char?, bool> isTokenCharacter)
    {
        var tokenStartIndex = context.Cursor.Position + 1;

        // keep adding characters in lexeme buffer and moving forward till token boundary
        while (isTokenCharacter(context.Cursor.Character))
        {
            context.Output.LexemeBuffer.Append(context.Cursor.Character);

            if (!context.Cursor.Advance())
            {
                // if there are no more characters in the input buffer, then stop reading further and flush the lexeme buffer
                context.Output.EmitBufferedToken(expectedTokenType, tokenStartIndex);
                return;
            }
        }

        // flush the lexeme buffer when token boundary is encountered
        context.Output.EmitBufferedToken(expectedTokenType, tokenStartIndex);
    }

    private void ReadExpectedCharacter(Context context, char expected)
    {
        if (context.Cursor.Character is null)
        {
            throw new JsonInputScannerException($"Expected '{expected}' at position {context.Cursor.OneBasedPosition}, but reached the end of input.");
        }

        if (context.Cursor.Character != expected)
        {
            throw new JsonInputScannerException($"Expected '{expected}' at position {context.Cursor.OneBasedPosition}, but received '{context.Cursor.Character}'.");
        }

        context.Cursor.Advance();
    }

    private void AppendCurrentAndAdvance(Context context)
    {
        context.Output.LexemeBuffer.Append(context.Cursor.Character);
        context.Cursor.Advance();
    }

    private bool IsSymbol(char? value) => value is '{' or '}' or '[' or ']' or ',' or ':' or '"';

    private bool IsNumberStart(char value) => value == '-' ||
                                              char.IsDigit(value);

    private bool IsIdentifierTerminator(char character) => IsSymbol(character) ||
                                                           (character is char c && char.IsWhiteSpace(c)) ||
                                                           IsNumberStart(character);

    private bool IsSymbolOrWhitespace(char? character) => IsSymbol(character) ||
                                                          (character is char c && char.IsWhiteSpace(c));

    private void ValidateLexerInvariant(Context context) => context.Output.AssertInvariant();
}