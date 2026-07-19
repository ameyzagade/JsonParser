using JsonParser.App.TokenModel;
using JsonParser.App.Lexer.Exceptions;
using System.Globalization;
using System.Collections.Immutable;
using System.Diagnostics;

namespace JsonParser.App.Lexer;

public sealed class JsonInputScanner
{
    private const int WhiteSpaceAsciiHexCode = 0x20;
    private const int UnicodeHexDigitCount = 4;

    private readonly Cursor _cursor;
    private readonly LexerOutput _output;

    private JsonInputScanner(in string content)
    {
        ArgumentException.ThrowIfNullOrEmpty(content);

        _cursor = new Cursor(content);
        _output = new LexerOutput();
    }

    /// <summary>
    /// Scans the input JSON string and returns an immutable array of tokens.
    /// </summary>
    /// <param name="content">
    /// The input JSON string to be scanned. It must not be null or empty.
    /// </param>
    /// <returns>
    /// An immutable array of tokens representing the scanned JSON input.
    /// </returns>
    public static ImmutableArray<Token> Scan(in string content)
        => new JsonInputScanner(content).Tokenize();

    private ImmutableArray<Token> Tokenize()
    {
        // each helper function will read the token according to the rule
        // and advance the index to next unread token
        while (_cursor.Character is not null)
        {
            // ScanNextToken
            ScanNextToken();
        }

        _output.AssertInvariant();

        _output.Emit(TokenType.EndOfStream, string.Empty, _cursor.OneBasedPosition);

        return [.. _output.GeneratedTokens];
    }

    private void ScanNextToken()
    {
        var currentCharacter = _cursor.Character;
        if (currentCharacter is char c && char.IsWhiteSpace(c))
        {
            _cursor.Advance();
            return;
        }

        // Every Read*() below identifies and emits a token and stops the cursor at next unconsumed character
        switch (currentCharacter)
        {
            case '{':
                _output.Emit(TokenType.LeftBrace, "{", _cursor.OneBasedPosition);
                _cursor.Advance();
                return;

            case '}':
                _output.Emit(TokenType.RightBrace, "}", _cursor.OneBasedPosition);
                _cursor.Advance();
                return;

            case '[':
                _output.Emit(TokenType.LeftBracket, "[", _cursor.OneBasedPosition);
                _cursor.Advance();
                return;

            case ']':
                _output.Emit(TokenType.RightBracket, "]", _cursor.OneBasedPosition);
                _cursor.Advance();
                return;

            case ',':
                _output.Emit(TokenType.Comma, ",", _cursor.OneBasedPosition);
                _cursor.Advance();
                return;

            case ':':
                _output.Emit(TokenType.Colon, ":", _cursor.OneBasedPosition);
                _cursor.Advance();
                return;

            case '"':
                ReadString();
                return;

            case 't':
            case 'f':
            case 'n':
                ReadIdentifier();
                return;

            case '-' or '0' or '1' or '2' or '3' or '4' or '5' or '6' or '7' or '8' or '9':
                ReadNumber();
                break;
        }

        ReadInvalidToken();
    }

    private void ReadNumber()
    {
        Debug.Assert(_cursor.Character is null ||
                    IsNumberStart(_cursor.Character.Value));

        var tokenStartIndex = _cursor.OneBasedPosition;

        // JSON number: -? integer fraction? exponent?
        if (_cursor.Character == '-')
        {
            if (!AppendCurrentAndAdvance())
            {
                throw new JsonInputScannerException($"Expected [0-9] (digit) after - (minus) at position {_cursor.OneBasedPosition}, but reached the end of input.");
            }
        }

        ReadUnsignedInteger();

        if (_cursor.Character is char commaChar &&
            commaChar == '.')
        {
            ReadFraction();
        }

        if (_cursor.Character is char exponentSignChar &&
            (exponentSignChar == 'e' || exponentSignChar == 'E'))
        {
            ReadExponent();
        }

        _output.EmitBufferedToken(TokenType.Number, tokenStartIndex);

        Debug.Assert(_cursor.Character is null ||
                    !IsNumberStart(_cursor.Character.Value));
    }

    private void ReadUnsignedInteger()
    {
        Debug.Assert(_cursor.Character is null || char.IsDigit(_cursor.Character.Value));

        var tokenStartIndex = _cursor.OneBasedPosition;

        if (_cursor.Character == '0')
        {
            AppendCurrentAndAdvance();
            if (_cursor.Character is char c &&
                char.IsDigit(c))
            {
                throw new JsonInputScannerException($"Leading zeros are not permitted. Received '0{c}' at position {tokenStartIndex}.");
            }
        }
        else
        {
            ReadDigits();
        }

        Debug.Assert(_cursor.Character is null || !char.IsDigit(_cursor.Character.Value));
    }

    private void ReadFraction()
    {
        Debug.Assert(_cursor.Character == '.');

        AppendCurrentAndAdvance();
        ReadDigits();

        Debug.Assert(_cursor.Character is null || !char.IsDigit(_cursor.Character.Value));
    }

    private void ReadExponent()
    {
        Debug.Assert(_cursor.Character is 'e' or 'E');

        if (!AppendCurrentAndAdvance())
        {
            throw new JsonInputScannerException($"Expected either [0-9] digits or + (plus) or - (minus) after e or E (exponential sign) at position {_cursor.OneBasedPosition}, but reached the end of input.");
        }

        if (_cursor.Character == '-' ||
            _cursor.Character == '+')
        {
            if (!AppendCurrentAndAdvance())
            {
                throw new JsonInputScannerException($"Expected [0-9] digits after + (plus) or - (minus) at position {_cursor.OneBasedPosition}, but reached the end of input.");
            }
        }

        ReadDigits();

        Debug.Assert(_cursor.Character is null || !char.IsDigit(_cursor.Character.Value));
    }

    private void ReadDigits()
    {
        Debug.Assert(_cursor.Character is null || char.IsDigit(_cursor.Character.Value));

        if (_cursor.Character is not char c)
        {
            throw new JsonInputScannerException($"Expected [0-9] (digit) at position {_cursor.OneBasedPosition}, but reached the end of input.");
        }

        if (!char.IsDigit(c))
        {
            throw new JsonInputScannerException($"Expected [0-9] (digit) at position {_cursor.OneBasedPosition}, but received '{c}'.");
        }

        while (_cursor.Character is char ch &&
                char.IsDigit(ch))
        {
            _output.Append(ch);

            if (!_cursor.Advance())
            {
                break;
            }
        }

        Debug.Assert(_cursor.Character is null || !char.IsDigit(_cursor.Character.Value));
    }

    private void ReadString()
    {
        Debug.Assert(_cursor.Character == '"');

        var tokenStartIndex = _cursor.OneBasedPosition;
        if (!_cursor.Advance())
        {
            throw new JsonInputScannerException($"Input terminated after position: {tokenStartIndex} without an ending double quote!");
        }

        while (true)
        {
            var character = _cursor.Character;
            if (character == '"')
            {
                // since the string started with a double quote ", this is the matching double quote to end the string
                _output.EmitBufferedToken(TokenType.String, tokenStartIndex);
                _cursor.Advance();
                break;
            }

            if (character < WhiteSpaceAsciiHexCode)
            {
                throw new JsonInputScannerException($"Control characters encountered at position: {_cursor.OneBasedPosition}.");
            }
            else if (character == '\\')
            {
                ReadEscapeCharacter();
            }
            else
            {
                _output.Append(character!.Value);

                if (!_cursor.Advance())
                {
                    // if there are no more characters in the input buffer, then stop reading further
                    throw new JsonInputScannerException($"Input terminated after position: {_cursor.OneBasedPosition} without an ending double quote!");
                }
            }
        }
    }

    private void ReadEscapeCharacter()
    {
        if (!_cursor.Advance())
        {
            throw new JsonInputScannerException($"Input terminated after position: {_cursor.OneBasedPosition} in middle of an escape sequence!");
        }

        switch (_cursor.Character)
        {
            case '"' or '\\' or '/' or 'b' or 'f' or 'n' or 'r' or 't':
                ReadNormalEscapeCharacter();
                break;

            case 'u':
                ReadUnicodeEscapeSequence();
                break;

            default:
                throw new JsonInputScannerException($"Unknown escape character encountered at position: {_cursor.OneBasedPosition}. Received character: {_cursor.Character}");
        }
    }

    private void ReadNormalEscapeCharacter()
    {
        var decodedCharacter = _cursor.Character switch
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

        _output.Append(decodedCharacter);

        if (!_cursor.Advance())
        {
            // if there are no more characters in the input buffer, then stop reading further
            throw new JsonInputScannerException($"Input terminated after position: {_cursor.OneBasedPosition} without an ending double quote!");
        }
    }

    private void ReadUnicodeEscapeSequence()
    {
        // Entered with cursor on 'u'.
        if (!_cursor.Advance())
        {
            throw new JsonInputScannerException($"Input terminated after position: {_cursor.OneBasedPosition} in middle of a Unicode escape sequence!");
        }

        var firstCodeUnitStartPosition = _cursor.OneBasedPosition;
        var firstCodeUnit = ReadUnicodeCodeUnit();
        if (char.IsLowSurrogate(firstCodeUnit))
        {
            throw new JsonInputScannerException($"The Unicode code unit \\u{(int)firstCodeUnit:X4} at position {firstCodeUnitStartPosition} is a low surrogate and cannot appear without a preceding high surrogate.");
        }

        if (!char.IsHighSurrogate(firstCodeUnit))
        {
            // the Unicode codeunit is neither a low or high surrogate but is a normal Unicode codeunit
            _output.Append(firstCodeUnit);
            return;
        }

        // at this point, the surrogate is a high surrogate and must be followed by a low surrogate
        ExpectAndConsume('\\');
        ExpectAndConsume('u');

        var secondCodeUnitStartPosition = _cursor.OneBasedPosition;
        var secondCodeUnit = ReadUnicodeCodeUnit();
        if (!char.IsLowSurrogate(secondCodeUnit))
        {
            throw new JsonInputScannerException($"The Unicode code unit \\u{(int)secondCodeUnit:X4} at position {secondCodeUnitStartPosition} is not a low surrogate. A high surrogate must be followed by a low surrogate.");
        }

        _output.Append(firstCodeUnit);
        _output.Append(secondCodeUnit);
    }

    private char ReadUnicodeCodeUnit()
    {
        if (!_cursor.TryPeek(UnicodeHexDigitCount, out var codeUnitSequence))
        {
            throw new JsonInputScannerException($"Input terminated after position: {_cursor.OneBasedPosition} in middle of a Unicode code unit!");
        }

        if (!int.TryParse(codeUnitSequence, NumberStyles.HexNumber, CultureInfo.InvariantCulture, out int codeunit))
        {
            throw new JsonInputScannerException($"Invalid Unicode code unit at position {_cursor.OneBasedPosition}. Expected four hexadecimal digits but received '\\u{codeUnitSequence}'.");
        }

        _cursor.Advance(UnicodeHexDigitCount);

        return (char)codeunit;
    }

    private void ReadIdentifier()
        => ReadUntilBoundary(TokenType.Identifier, character => !IsIdentifierTerminator(character!.Value));

    private void ReadInvalidToken()
        => ReadUntilBoundary(TokenType.Invalid, character => !IsSymbolOrWhitespace(character));

    private void ReadUntilBoundary(TokenType expectedTokenType, Func<char?, bool> isTokenCharacter)
    {
        Debug.Assert(isTokenCharacter(_cursor.Character));

        var tokenStartIndex = _cursor.OneBasedPosition;

        // keep adding characters in lexeme buffer and moving forward till token boundary
        while (isTokenCharacter(_cursor.Character))
        {
            _output.Append(_cursor.Character!.Value);

            if (!_cursor.Advance())
            {
                // if there are no more characters in the input buffer, then stop reading further and flush the lexeme buffer
                _output.EmitBufferedToken(expectedTokenType, tokenStartIndex);
                return;
            }
        }

        // flush the lexeme buffer when token boundary is encountered
        _output.EmitBufferedToken(expectedTokenType, tokenStartIndex);

        Debug.Assert(!isTokenCharacter(_cursor.Character));
    }

    private void ExpectAndConsume(char expected)
    {
        if (_cursor.Character is null)
        {
            throw new JsonInputScannerException($"Expected '{expected}' at position {_cursor.OneBasedPosition}, but reached the end of input.");
        }

        if (_cursor.Character != expected)
        {
            throw new JsonInputScannerException($"Expected '{expected}' at position {_cursor.OneBasedPosition}, but received '{_cursor.Character}'.");
        }

        _cursor.Advance();
    }

    /// Precondition:
    /// Cursor positioned on a valid character.
    private bool AppendCurrentAndAdvance()
    {
        _output.Append(_cursor.Character!.Value);
        return _cursor.Advance();
    }

    private bool IsSymbol(char? value) => value is '{' or '}' or '[' or ']' or ',' or ':' or '"';

    private bool IsNumberStart(char value) => value == '-' ||
                                              char.IsDigit(value);

    private bool IsIdentifierTerminator(char character) => IsSymbol(character) ||
                                                           (character is char c && char.IsWhiteSpace(c)) ||
                                                           IsNumberStart(character);

    private bool IsSymbolOrWhitespace(char? character) => IsSymbol(character) ||
                                                          (character is char c && char.IsWhiteSpace(c));
}