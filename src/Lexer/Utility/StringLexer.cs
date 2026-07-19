using System.Diagnostics;
using System.Globalization;
using JsonParser.App.Lexer.Exceptions;
using JsonParser.App.TokenModel;

namespace JsonParser.App.Lexer.Utility;

internal sealed class StringLexer
{
    private const int WhiteSpaceAsciiHexCode = 0x20;
    private const int UnicodeHexDigitCount = 4;

    private readonly LexerContext _context;

    public StringLexer(LexerContext context) => _context = context;

    internal void Read()
    {
        Debug.Assert(_context.Cursor.Character == '"');

        var tokenStartIndex = _context.Cursor.OneBasedPosition;
        if (!_context.Cursor.Advance())
        {
            throw new JsonInputScannerException($"Input terminated after position: {tokenStartIndex} without an ending double quote!");
        }

        while (true)
        {
            var character = _context.Cursor.Character;
            if (character == '"')
            {
                // since the string started with a double quote ", this is the matching double quote to end the string
                _context.Output.EmitBufferedToken(TokenType.String, tokenStartIndex);
                _context.Cursor.Advance();
                break;
            }

            if (character < WhiteSpaceAsciiHexCode)
            {
                throw new JsonInputScannerException($"Control characters encountered at position: {_context.Cursor.OneBasedPosition}.");
            }
            else if (character == '\\')
            {
                ReadEscapeCharacter();
            }
            else
            {
                _context.Output.Append(character!.Value);

                if (!_context.Cursor.Advance())
                {
                    // if there are no more characters in the input buffer, then stop reading further
                    throw new JsonInputScannerException($"Input terminated after position: {_context.Cursor.OneBasedPosition} without an ending double quote!");
                }
            }
        }
    }

    private void ReadEscapeCharacter()
    {
        if (!_context.Cursor.Advance())
        {
            throw new JsonInputScannerException($"Input terminated after position: {_context.Cursor.OneBasedPosition} in middle of an escape sequence!");
        }

        switch (_context.Cursor.Character)
        {
            case '"' or '\\' or '/' or 'b' or 'f' or 'n' or 'r' or 't':
                ReadNormalEscapeCharacter();
                break;

            case 'u':
                ReadUnicodeEscapeSequence();
                break;

            default:
                throw new JsonInputScannerException($"Unknown escape character encountered at position: {_context.Cursor.OneBasedPosition}. Received character: {_context.Cursor.Character}");
        }
    }

    private void ReadNormalEscapeCharacter()
    {
        var decodedCharacter = _context.Cursor.Character switch
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

        _context.Output.Append(decodedCharacter);

        if (!_context.Cursor.Advance())
        {
            // if there are no more characters in the input buffer, then stop reading further
            throw new JsonInputScannerException($"Input terminated after position: {_context.Cursor.OneBasedPosition} without an ending double quote!");
        }
    }

    private void ReadUnicodeEscapeSequence()
    {
        // Entered with cursor on 'u'.
        if (!_context.Cursor.Advance())
        {
            throw new JsonInputScannerException($"Input terminated after position: {_context.Cursor.OneBasedPosition} in middle of a Unicode escape sequence!");
        }

        var firstCodeUnitStartPosition = _context.Cursor.OneBasedPosition;
        var firstCodeUnit = ReadUnicodeCodeUnit();
        if (char.IsLowSurrogate(firstCodeUnit))
        {
            throw new JsonInputScannerException($"The Unicode code unit \\u{(int)firstCodeUnit:X4} at position {firstCodeUnitStartPosition} is a low surrogate and cannot appear without a preceding high surrogate.");
        }

        if (!char.IsHighSurrogate(firstCodeUnit))
        {
            // the Unicode codeunit is neither a low or high surrogate but is a normal Unicode codeunit
            _context.Output.Append(firstCodeUnit);
            return;
        }

        // at this point, the surrogate is a high surrogate and must be followed by a low surrogate
        ExpectAndConsume('\\');
        ExpectAndConsume('u');

        var secondCodeUnitStartPosition = _context.Cursor.OneBasedPosition;
        var secondCodeUnit = ReadUnicodeCodeUnit();
        if (!char.IsLowSurrogate(secondCodeUnit))
        {
            throw new JsonInputScannerException($"The Unicode code unit \\u{(int)secondCodeUnit:X4} at position {secondCodeUnitStartPosition} is not a low surrogate. A high surrogate must be followed by a low surrogate.");
        }

        _context.Output.Append(firstCodeUnit);
        _context.Output.Append(secondCodeUnit);
    }

    private char ReadUnicodeCodeUnit()
    {
        if (!_context.Cursor.TryPeek(UnicodeHexDigitCount, out var codeUnitSequence))
        {
            throw new JsonInputScannerException($"Input terminated after position: {_context.Cursor.OneBasedPosition} in middle of a Unicode code unit!");
        }

        if (!int.TryParse(codeUnitSequence, NumberStyles.HexNumber, CultureInfo.InvariantCulture, out int codeunit))
        {
            throw new JsonInputScannerException($"Invalid Unicode code unit at position {_context.Cursor.OneBasedPosition}. Expected four hexadecimal digits but received '\\u{codeUnitSequence}'.");
        }

        _context.Cursor.Advance(UnicodeHexDigitCount);

        return (char)codeunit;
    }

    private void ExpectAndConsume(char expected)
    {
        if (_context.Cursor.Character is null)
        {
            throw new JsonInputScannerException($"Expected '{expected}' at position {_context.Cursor.OneBasedPosition}, but reached the end of input.");
        }

        if (_context.Cursor.Character != expected)
        {
            throw new JsonInputScannerException($"Expected '{expected}' at position {_context.Cursor.OneBasedPosition}, but received '{_context.Cursor.Character}'.");
        }

        _context.Cursor.Advance();
    }
}
