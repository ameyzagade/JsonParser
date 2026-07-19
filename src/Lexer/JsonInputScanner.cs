using JsonParser.App.Lexer.Utility;
using JsonParser.App.TokenModel;
using System.Collections.Immutable;

namespace JsonParser.App.Lexer;

public sealed class JsonInputScanner
{
    private readonly LexerContext _context;
    private readonly PunctuationReader _punctuationLexer;
    private readonly NumberLexer _numberLexer;
    private readonly StringLexer _stringLexer;
    private readonly IdentifierLexer _identifierLexer;

    private JsonInputScanner(in string content)
    {
        ArgumentException.ThrowIfNullOrEmpty(content);

        _context = new LexerContext(new Cursor(content), new LexerOutput());
        _punctuationLexer = new PunctuationReader(_context);
        _numberLexer = new NumberLexer(_context);
        _stringLexer = new StringLexer(_context);
        _identifierLexer = new IdentifierLexer(_context);
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
        while (_context.Cursor.Character is not null)
        {
            ScanNextToken();
        }

        _context.Output.AssertInvariant();

        _context.Output.Emit(TokenType.EndOfStream, string.Empty, _context.Cursor.OneBasedPosition);

        return [.. _context.Output.GeneratedTokens];
    }

    private void ScanNextToken()
    {
        var currentCharacter = _context.Cursor.Character!.Value;
        if (char.IsWhiteSpace(currentCharacter))
        {
            _context.Cursor.Advance();
        }
        else if (CharacterClassifier.IsPunctuation(currentCharacter))
        {
            _punctuationLexer.Read();
        }
        else if (CharacterClassifier.IsNumberStart(currentCharacter))
        {
            _numberLexer.Read();
        }
        else if (CharacterClassifier.IsStringStart(currentCharacter))
        {
            _stringLexer.Read();
        }
        else if (CharacterClassifier.IsIdentifierStart(currentCharacter))
        {
            _identifierLexer.Read();
        }
        else
        {
            _identifierLexer.ReadInvalid();
        }
    }
}

internal static class CharacterClassifier
{
    internal static bool IsPunctuation(char? value) => value is '{' or '}' or '[' or ']' or ',' or ':' or '"';

    internal static bool IsNumberStart(char value) => value == '-' || char.IsDigit(value);

    internal static bool IsStringStart(char? value) => value is '"';

    internal static bool IsIdentifierStart(char value) => value is 't' or 'f' or 'n';
}