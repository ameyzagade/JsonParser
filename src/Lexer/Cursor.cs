namespace JsonParser.App.Lexer;

public sealed class Cursor
{
    public string InputBuffer { get; }
    public char? CurrentCharacter { get; private set; }
    public int CurrentPosition { get; private set; }

    public Cursor(string content)
    {
        ArgumentNullException.ThrowIfNull(content);

        InputBuffer = content;
        CurrentCharacter = null;
        CurrentPosition = -1;
    }

    public bool Advance()
    {
        if (CurrentPosition >= InputBuffer.Length - 1)
        {
            CurrentCharacter = null;
            ++CurrentPosition;
        }
        else
        {
            CurrentCharacter = InputBuffer[++CurrentPosition];
        }

        return CurrentCharacter is not null;
    }

    public bool AdvanceBy(int count)
    {
        for (var i = 0; i < count; i++)
        {
            if (CurrentCharacter is null)
            {
                return false;
            }

            Advance();
        }

        return true;
    }

    public bool TryPeek(
        int startIndex,
        int count,
        out string value)
    {
        var inputBufferLength = InputBuffer.Length;
        var endIndex = InputBuffer.Length - startIndex;

        if (startIndex < 0 ||
            count < 0 ||
            startIndex > inputBufferLength ||
            count > endIndex)
        {
            value = string.Empty;
            return false;
        }

        value = InputBuffer[startIndex..endIndex];
        return true;
    }

    public int CurrentOneBasedPosition => CurrentPosition + 1;
}
