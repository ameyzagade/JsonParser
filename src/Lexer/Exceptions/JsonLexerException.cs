namespace JsonParser.App.Lexer.Exceptions;

public sealed class JsonLexerException : Exception
{
    public JsonLexerException() : base() { }
    public JsonLexerException(string message) : base(message) { }
    public JsonLexerException(string message, Exception innerException) : base(message, innerException) { }
}