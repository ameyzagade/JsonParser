using System.Diagnostics;
using JsonParser.App.TokenModel;

namespace JsonParser.App.Lexer.Utility;

internal sealed class PunctuationReader
{
    private readonly LexerContext _context;

    internal PunctuationReader(LexerContext context) => _context = context;

    internal void Read()
    {
        Debug.Assert(_context.Cursor.Character is '{' or '}' or '[' or ']' or ',' or ':' or '"');

        switch (_context.Cursor.Character)
        {
            case '{':
                _context.Output.Emit(TokenType.LeftBrace, "{", _context.Cursor.OneBasedPosition);
                break;

            case '}':
                _context.Output.Emit(TokenType.RightBrace, "}", _context.Cursor.OneBasedPosition);
                break;

            case '[':
                _context.Output.Emit(TokenType.LeftBracket, "[", _context.Cursor.OneBasedPosition);
                break;

            case ']':
                _context.Output.Emit(TokenType.RightBracket, "]", _context.Cursor.OneBasedPosition);
                break;

            case ',':
                _context.Output.Emit(TokenType.Comma, ",", _context.Cursor.OneBasedPosition);
                break;

            case ':':
                _context.Output.Emit(TokenType.Colon, ":", _context.Cursor.OneBasedPosition);
                break;

            default:
                throw new InvalidOperationException($"Unexpected symbol '{_context.Cursor.Character}' encountered at position {_context.Cursor.OneBasedPosition}.");
        }

        _context.Cursor.Advance();
    }
}