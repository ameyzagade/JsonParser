using System.Text;
using JsonParser.App.Lexer.Exceptions;
using JsonParser.App.TokenModel;

namespace JsonParser.App.Lexer;

public sealed class Output
{
    public StringBuilder LexemeBuffer { get; }
    public List<Token> GeneratedTokens { get; }

    public Output()
    {
        LexemeBuffer = new();
        GeneratedTokens = [];
    }

    public void Emit(TokenType tokenType, string value, int tokenStartIndex)
        => GeneratedTokens.Add(new Token(tokenType, value, tokenStartIndex));

    public void EmitBufferedToken(TokenType tokenType, int tokenStartIndex)
    {
        var value = LexemeBuffer.ToString();
        Emit(tokenType, value, tokenStartIndex);

        LexemeBuffer.Clear();
    }

    public void AssertInvariant()
    {
        if (LexemeBuffer.Length > 0)
        {
            throw new InvalidOperationException($"End of input while reading token: {LexemeBuffer}");
        }
    }
}