using System.Text;
using JsonParser.App.TokenModel;

namespace JsonParser.App.Lexer;

public sealed class Output
{
    public StringBuilder LexemeBuffer { get; private set; }
    public List<Token> GeneratedTokens { get; private set; }

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
}