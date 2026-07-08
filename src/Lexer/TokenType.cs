namespace JsonParser.App.Lexer;

public enum TokenType
{
    LeftBrace,
    RightBrace,
    LeftBracket,
    RightBracket,
    Colon,
    Comma,
    String,
    Number,
    Identifier,
    Invalid,
    EndOfStream
}
