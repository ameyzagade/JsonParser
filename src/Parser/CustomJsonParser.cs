using JsonParser.App.Lexer;

namespace JsonParser.App.Parser;

public sealed class CustomJsonParser
{
    private enum ParserState
    {
        ExpectLeftBrace,
        ExpectRightBrace,
        Complete
    }

    private sealed class ParserContext
    {
        public Token? CurrentToken { get; set; }
        public ParserState CurrentState { get; set; } = ParserState.ExpectLeftBrace;
    }

    public bool Parse(IReadOnlyList<Token> tokens)
    {
        var context = new ParserContext();
        foreach (var token in tokens)
        {
            context.CurrentToken = token;
            Process(context);
        }

        ValidateEndState(context);

        return true;
    }

    private void Process(ParserContext context)
    {
        switch (context.CurrentState)
        {
            case ParserState.ExpectLeftBrace:
                ProcessExpectLeftBrace(context);
                break;
            case ParserState.ExpectRightBrace:
                ProcessExpectRightBrace(context);
                break;
        }
    }

    private void ProcessExpectLeftBrace(ParserContext context)
        => context.CurrentState = ParserState.ExpectRightBrace;

    private void ProcessExpectRightBrace(ParserContext context)
        => context.CurrentState = ParserState.Complete;

    private void ValidateEndState(ParserContext context)
    {
        if (context.CurrentState != ParserState.Complete)
        {
            throw new CustomJsonParserException($"Parser not in the expected {Enum.GetName(ParserState.Complete)} state after consuming all tokens!");
        }
    }
}