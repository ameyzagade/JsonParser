using System.Diagnostics;
using JsonParser.App.Lexer.Exceptions;
using JsonParser.App.TokenModel;

namespace JsonParser.App.Lexer.Utility;

internal sealed class NumberLexer
{
    private readonly LexerContext _context;

    internal NumberLexer(LexerContext context) => _context = context;

    internal void Read()
    {
        Debug.Assert(_context.Cursor.Character is null ||
                    CharacterClassifier.IsNumberStart(_context.Cursor.Character.Value));

        var tokenStartIndex = _context.Cursor.OneBasedPosition;

        // JSON number: -? integer fraction? exponent?
        if (_context.Cursor.Character == '-')
        {
            if (!AppendCurrentAndAdvance())
            {
                throw new JsonInputScannerException($"Expected [0-9] (digit) after - (minus) at position {_context.Cursor.OneBasedPosition}, but reached the end of input.");
            }
        }

        ReadUnsignedInteger();

        if (_context.Cursor.Character is char commaChar &&
            commaChar == '.')
        {
            ReadFraction();
        }

        if (_context.Cursor.Character is char exponentSignChar &&
            (exponentSignChar == 'e' || exponentSignChar == 'E'))
        {
            ReadExponent();
        }

        _context.Output.EmitBufferedToken(TokenType.Number, tokenStartIndex);

        Debug.Assert(_context.Cursor.Character is null ||
                    !CharacterClassifier.IsNumberStart(_context.Cursor.Character.Value));
    }

    private void ReadUnsignedInteger()
    {
        Debug.Assert(_context.Cursor.Character is null || char.IsDigit(_context.Cursor.Character.Value));

        var tokenStartIndex = _context.Cursor.OneBasedPosition;

        if (_context.Cursor.Character == '0')
        {
            AppendCurrentAndAdvance();
            if (_context.Cursor.Character is char c &&
                char.IsDigit(c))
            {
                throw new JsonInputScannerException($"Leading zeros are not permitted. Received '0{c}' at position {tokenStartIndex}.");
            }
        }
        else
        {
            ReadDigits();
        }

        Debug.Assert(_context.Cursor.Character is null || !char.IsDigit(_context.Cursor.Character.Value));
    }

    private void ReadFraction()
    {
        Debug.Assert(_context.Cursor.Character == '.');

        AppendCurrentAndAdvance();
        ReadDigits();

        Debug.Assert(_context.Cursor.Character is null || !char.IsDigit(_context.Cursor.Character.Value));
    }

    private void ReadExponent()
    {
        Debug.Assert(_context.Cursor.Character is 'e' or 'E');

        if (!AppendCurrentAndAdvance())
        {
            throw new JsonInputScannerException($"Expected either [0-9] digits or + (plus) or - (minus) after e or E (exponential sign) at position {_context.Cursor.OneBasedPosition}, but reached the end of input.");
        }

        if (_context.Cursor.Character == '-' ||
            _context.Cursor.Character == '+')
        {
            if (!AppendCurrentAndAdvance())
            {
                throw new JsonInputScannerException($"Expected [0-9] digits after + (plus) or - (minus) at position {_context.Cursor.OneBasedPosition}, but reached the end of input.");
            }
        }

        ReadDigits();

        Debug.Assert(_context.Cursor.Character is null || !char.IsDigit(_context.Cursor.Character.Value));
    }

    private void ReadDigits()
    {
        Debug.Assert(_context.Cursor.Character is null || char.IsDigit(_context.Cursor.Character.Value));

        if (_context.Cursor.Character is not char c)
        {
            throw new JsonInputScannerException($"Expected [0-9] (digit) at position {_context.Cursor.OneBasedPosition}, but reached the end of input.");
        }

        if (!char.IsDigit(c))
        {
            throw new JsonInputScannerException($"Expected [0-9] (digit) at position {_context.Cursor.OneBasedPosition}, but received '{c}'.");
        }

        while (_context.Cursor.Character is char ch &&
                char.IsDigit(ch))
        {
            _context.Output.Append(ch);

            if (!_context.Cursor.Advance())
            {
                break;
            }
        }

        Debug.Assert(_context.Cursor.Character is null || !char.IsDigit(_context.Cursor.Character.Value));
    }

    private bool AppendCurrentAndAdvance()
    {
        Debug.Assert(_context.Cursor.Character is not null);

        _context.Output.Append(_context.Cursor.Character!.Value);
        return _context.Cursor.Advance();
    }
}