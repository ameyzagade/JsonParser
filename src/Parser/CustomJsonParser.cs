using JsonParser.App.Lexer;

namespace JsonParser.App.Parser;

public sealed class CustomJsonParser
{
    private enum ParserState
    {
        ExpectLeftBrace,
        ExpectRightBraceOrString,
        ExpectString,
        ExpectColon,
        ExpectStringOrPrimitiveValueOrObject,
        ExpectCommaOrRightBrace,
        ExpectRightBrace,
        Complete
    }

    private sealed class ParserContext
    {
        public IReadOnlyList<Token> InputTokens { get; init; } = [];
        public Token? CurrentToken { get; set; }
        public int CurrentPosition { get; set; }
        public ParserState CurrentState { get; set; } = ParserState.ExpectLeftBrace;
    }

    public void Parse(IReadOnlyList<Token> tokens)
    {
        var context = new ParserContext
        {
            InputTokens = tokens,
        };

        for (context.CurrentPosition = 0; context.CurrentPosition < tokens.Count; context.CurrentPosition++)
        {
            Token? token = tokens[context.CurrentPosition];
            context.CurrentToken = token;
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

            case ParserState.ExpectStringOrPrimitiveValueOrObject:
                ProcessExpectStringOrPrimitiveValueOrObject(context);
                break;

            case ParserState.ExpectCommaOrRightBrace:
                ProcessExpectCommaOrRightBrace(context);
                break;

            case ParserState.ExpectString:
                ProcessExpectString(context);
                break;

            case ParserState.ExpectRightBrace:
                ProcessExpectRightBrace(context);
                break;

            default:
                throw new CustomJsonParserException($"Parser is in an unknown state: {context.CurrentState}");
        }
    }

    private void ProcessExpectLeftBrace(ParserContext context)
        => context.CurrentState = context.CurrentToken!.TokenType switch
        {
            TokenType.LeftBrace => ParserState.ExpectRightBraceOrString,
            _ => throw new CustomJsonParserException($"Unexpected token received at position {context.CurrentPosition}. Token: {context.CurrentToken.Value}. Expected Token: Left Brace."),
        };

    private void ProcessExpectRightBraceOrString(ParserContext context)
        => context.CurrentState = context.CurrentToken!.TokenType switch
        {
            TokenType.String => ParserState.ExpectColon,
            TokenType.RightBrace => ParserState.Complete,
            _ => throw new CustomJsonParserException($"Unexpected token received at position {context.CurrentPosition}. Token: {context.CurrentToken.Value}. Expected Token: String or Right Brace."),
        };

    private void ProcessExpectColon(ParserContext context)
        => context.CurrentState = context.CurrentToken!.TokenType switch
        {
            TokenType.Colon => ParserState.ExpectStringOrPrimitiveValueOrObject,
            _ => throw new CustomJsonParserException($"Unexpected token received at position {context.CurrentPosition}. Token: {context.CurrentToken.Value}. Expected Token: Colon."),
        };

    private void ProcessExpectStringOrPrimitiveValueOrObject(ParserContext context)
    {
        if (context.CurrentToken is { TokenType: TokenType.LeftBrace })
        {
            ParseObject(context);
            return;
        }

        context.CurrentState = context.CurrentToken switch
        {
            { TokenType: TokenType.String } => ParserState.ExpectCommaOrRightBrace,
            { TokenType: TokenType.PrimitiveValue, Value: "true" } => ParserState.ExpectCommaOrRightBrace,
            { TokenType: TokenType.PrimitiveValue, Value: "false" } => ParserState.ExpectCommaOrRightBrace,
            { TokenType: TokenType.PrimitiveValue, Value: "null" } => ParserState.ExpectCommaOrRightBrace,
            { TokenType: TokenType.PrimitiveValue } when double.TryParse(context.CurrentToken.Value, out var _) => ParserState.ExpectCommaOrRightBrace,
            _ => throw new CustomJsonParserException($"Unexpected token received at position {context.CurrentPosition}. Token: {context.CurrentToken!.Value}."),
        };
    }

    private void ProcessExpectCommaOrRightBrace(ParserContext context)
        => context.CurrentState = context.CurrentToken!.TokenType switch
        {
            TokenType.Comma => ParserState.ExpectString,
            TokenType.RightBrace => ParserState.Complete,
            _ => throw new CustomJsonParserException($"Unexpected token received at position {context.CurrentPosition}. Token: {context.CurrentToken.Value}. Expected Token: Comma or Right Brace."),
        };

    private void ProcessExpectString(ParserContext context)
        => context.CurrentState = context.CurrentToken!.TokenType switch
        {
            TokenType.String => ParserState.ExpectColon,
            _ => throw new CustomJsonParserException($"Unexpected token received at position {context.CurrentPosition}. Token: {context.CurrentToken.Value}. Expected Token: String."),
        };

    private void ProcessExpectRightBrace(ParserContext context)
        => context.CurrentState = context.CurrentToken!.TokenType switch
        {
            TokenType.RightBrace => ParserState.Complete,
            _ => throw new CustomJsonParserException($"Unexpected token received at position {context.CurrentPosition}. Token: {context.CurrentToken.Value}. Expected Token: Right Brace."),
        };

    private void ParseObject(ParserContext context)
    {
        while (true)
        {
            if (context.CurrentToken!.TokenType == TokenType.RightBrace)
            {
                break;
            }

            MoveNextToken(context);
            ProcessExpectString(context);

            MoveNextToken(context);
            ProcessExpectColon(context);

            MoveNextToken(context);
            ProcessExpectStringOrPrimitiveValueOrObject(context);

            MoveNextToken(context);
            ProcessExpectCommaOrRightBrace(context);
            
            context.CurrentState = ParserState.ExpectCommaOrRightBrace;
        }
    }

    private void MoveNextToken(ParserContext context)
        => context.CurrentToken = context.InputTokens[++context.CurrentPosition];

    private void ValidateEndState(ParserContext context)
    {
        if (context.CurrentState != ParserState.Complete)
        {
            throw new CustomJsonParserException($"Parser not in the expected {Enum.GetName(ParserState.Complete)} state after consuming all tokens!");
        }
    }
}