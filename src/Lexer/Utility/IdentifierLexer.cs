using System.Diagnostics;
using JsonParser.App.TokenModel;

namespace JsonParser.App.Lexer.Utility;

internal sealed class IdentifierLexer
{
    private readonly LexerContext _context;

    internal IdentifierLexer(LexerContext context) => _context = context;

    internal void Read()
        => ReadUntilBoundary(TokenType.Identifier, character => !IsIdentifierTerminator(character!.Value));

    internal void ReadInvalid()
        => ReadUntilBoundary(TokenType.Invalid, character => !IsSymbolOrWhitespace(character));

    private void ReadUntilBoundary(TokenType expectedTokenType, Func<char?, bool> isTokenCharacter)
    {
        Debug.Assert(isTokenCharacter(_context.Cursor.Character));

        var tokenStartIndex = _context.Cursor.OneBasedPosition;

        // keep adding characters in lexeme buffer and moving forward till token boundary
        while (isTokenCharacter(_context.Cursor.Character))
        {
            _context.Output.Append(_context.Cursor.Character!.Value);

            if (!_context.Cursor.Advance())
            {
                // if there are no more characters in the input buffer, then stop reading further and flush the lexeme buffer
                _context.Output.EmitBufferedToken(expectedTokenType, tokenStartIndex);
                return;
            }
        }

        // flush the lexeme buffer when token boundary is encountered
        _context.Output.EmitBufferedToken(expectedTokenType, tokenStartIndex);

        Debug.Assert(!isTokenCharacter(_context.Cursor.Character));
    }

    private bool IsIdentifierTerminator(char character) => CharacterClassifier.IsPunctuation(character) ||
                                                           char.IsWhiteSpace(character) ||
                                                           CharacterClassifier.IsNumberStart(character);

    private bool IsSymbolOrWhitespace(char? character) => CharacterClassifier.IsPunctuation(character) ||
                                                          (character is char c && char.IsWhiteSpace(c));
}