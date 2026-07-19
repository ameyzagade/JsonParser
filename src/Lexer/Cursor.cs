namespace JsonParser.App.Lexer;

public sealed class Cursor
{
    private readonly string _inputBuffer;

    /// <summary>
    /// Gets the current character at the cursor position in the input buffer.
    /// </summary>
    public char? Character { get; private set; }

    /// <summary>
    /// Gets the current position of the cursor in the input buffer, starting from 0.
    /// </summary>
    public int Position { get; private set; }

    /// <summary>
    /// Gets the current position of the cursor in the input buffer, starting from 1.
    /// </summary>
    public int OneBasedPosition => Position + 1;

    public Cursor(string content)
    {
        ArgumentNullException.ThrowIfNull(content);

        _inputBuffer = content;
        Character = null;
        Position = -1;

        // Prime the cursor so it starts on the first character
        // (or EOF for an empty input).
        Advance();
    }

    /// <summary>
    /// Advances the cursor by up to <paramref name="count"/> characters.
    /// </summary>
    /// <param name="count">
    /// Number of characters to advance.
    /// Must be greater than zero.
    /// </param>
    /// <returns>
    /// <c>true</c> if all requested advances succeeded.
    /// <c>false</c> if EOF was encountered before all advances completed.
    /// </returns>
    public bool Advance(int count = 1)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(count, 1);

        for (var i = 0; i < count; i++)
        {
            AdvanceCore();
            if (Character is null)
            {
                return false;
            }
        }

        return true;
    }

    /// <summary>
    /// Returns up to <paramref name="count"/> characters
    /// starting at the current cursor position
    /// without advancing the cursor.
    /// </summary>
    /// <param name="value">The string when peek operation is successful.</param>
    /// <param name="count">The number of next characters to be peeked.</param>
    /// <returns>A boolean representating success of failure status of the peek operation.</returns>
    public bool TryPeek(int count, out string value)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(count, 1);

        var endIndex = Position + count;
        if (endIndex > _inputBuffer.Length)
        {
            value = string.Empty;
            return false;
        }

        value = _inputBuffer[Position..endIndex];
        return true;
    }

    private void AdvanceCore()
    {
        Position++;

        if (Position >= _inputBuffer.Length)
        {
            Character = null;
            return;
        }

        Character = _inputBuffer[Position];
    }
}