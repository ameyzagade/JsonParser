using JsonParser.App.Lexer;

namespace JsonParser.App.Parser;

public sealed class CustomJsonParser
{
    private enum ParserState
    {
        ExpectLeftBrace,
        ExpectRightBraceOrString,
        ExpectColon,
        ExpectValue,
        ExpectRightBrace,
        Complete
    }

    private sealed class ParserContext
    {
        public Token? CurrentToken { get; set; }
        public int CurrentPosition { get; set; } = 1;
        public ParserState CurrentState { get; set; } = ParserState.ExpectLeftBrace;
    }

    public void Parse(IReadOnlyList<Token> tokens)
    {
        var context = new ParserContext();
        foreach (var token in tokens)
        {
            context.CurrentToken = token;
            context.CurrentPosition += token.Value.Length;

            Process(context);
        }

        ValidateEndState(context);
    }

    private void Process(ParserContext context)
    {
        switch (context.CurrentState)
        {
            case ParserState.ExpectLeftBrace:
                ProcessExpectLeftBrace(context);
                break;

            case ParserState.ExpectRightBraceOrString:
                ProcessExpectRightBraceOrString(context);
                break;

            case ParserState.ExpectColon:
                ProcessExpectColon(context);
                break;

            case ParserState.ExpectValue:
                ProcessExpectValue(context);
                break;

            case ParserState.ExpectRightBrace:
                ProcessExpectRightBrace(context);
                break;

            default:
                throw new CustomJsonParserException($"Parser is in an unknown state: {context.CurrentState}");
        }
    }

    private void ProcessExpectLeftBrace(ParserContext context)
        => context.CurrentState = ParserState.ExpectRightBraceOrString;

    private void ProcessExpectRightBraceOrString(ParserContext context)
        => context.CurrentState = context.CurrentToken!.TokenType switch
        {
            TokenType.RightBrace => ParserState.Complete,
            TokenType.String => ParserState.ExpectColon,
            _ => throw new CustomJsonParserException($"Unexpected token received at position {context.CurrentPosition}. Token: {context.CurrentToken.Value}"),
        };

    private void ProcessExpectColon(ParserContext context)
        => context.CurrentState = ParserState.ExpectValue;

    private void ProcessExpectValue(ParserContext context)
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