namespace JsonParser.App.Lexer.Exceptions;

public sealed class JsonInputScannerException : Exception
{
    public JsonInputScannerException() : base() { }
    public JsonInputScannerException(string message) : base(message) { }
    public JsonInputScannerException(string message, Exception innerException) : base(message, innerException) { }
}