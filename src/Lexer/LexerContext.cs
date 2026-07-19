namespace JsonParser.App.Lexer;

internal sealed class LexerContext
{
    public Cursor Cursor { get; }
    public LexerOutput Output { get; }

    internal LexerContext(Cursor cursor, LexerOutput output)
    {
        Cursor = cursor;
        Output = output;
    }
}
