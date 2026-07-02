namespace JsonParser.App.Parser;

public sealed class CustomJsonParserException : Exception
{
    public CustomJsonParserException() : base() { }
    public CustomJsonParserException(string message) : base(message) { }
    public CustomJsonParserException(string message, Exception innerException) : base(message, innerException) { }
}