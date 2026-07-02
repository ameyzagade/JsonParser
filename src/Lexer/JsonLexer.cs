using JsonParser.App.Lexer.Exceptions;

namespace JsonParser.App.Lexer;

public sealed class JsonLexer
{
    private enum LexerState
    {
        Normal
    }

    private sealed class LexerContext
    {
        public char CurrentCharacter { get; set; }
        public LexerState CurrentState { get; set; } = LexerState.Normal;
        public List<Token> GeneratedTokens { get; set; } = [];
    }

    public IReadOnlyList<Token> Tokenize(ReadOnlySpan<char> content)
    {
        var context = new LexerContext();

        for (int currentPosition = 0; currentPosition < content.Length; currentPosition++)
        {
            context.CurrentCharacter = content[currentPosition];
            Process(context);
        }

        ValidateEndState(context);

        return context.GeneratedTokens;
    }

    private void Process(LexerContext context)
    {
        switch (context.CurrentState)
        {
            case LexerState.Normal:
                ProcessNormalState(context);
                break;
            default:
                throw new JsonLexerException($"Lexer is in unknown state: {context.CurrentState}");
        }
    }

    private void ProcessNormalState(LexerContext context)
    {
        switch (context.CurrentCharacter)
        {
            case '{':
                context.GeneratedTokens.Add(new Token(TokenType.LeftBrace, context.CurrentCharacter.ToString()));
                break;
            case '}':
                context.GeneratedTokens.Add(new Token(TokenType.RightBrace, context.CurrentCharacter.ToString()));
                break;
            default:
                break;
        }
    }

    private void ValidateEndState(LexerContext context)
    {
        if (context.CurrentState != LexerState.Normal)
        {
            throw new JsonLexerException($"Lexer not in the expected {Enum.GetName(LexerState.Normal)} state after consuming all characters!");
        }
    }
}
