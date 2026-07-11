using System.Globalization;
using JsonParser.App.TokenModel;

namespace JsonParser.App.Parser;

public sealed class CustomJsonParser
{
    private struct CursorState
    {
        public int CurrentPosition;
        public Token? CurrentToken;
        public IReadOnlyList<Token> TokenStream;

        public CursorState(int position, IReadOnlyList<Token> tokenStream)
        {
            CurrentPosition = position;
            TokenStream = tokenStream;
        }
    }

    private class Context
    {
        public CursorState CursorState;
    }

    public void Parse(IReadOnlyList<Token> tokens)
    {
        Context context = IntializeContext(tokens);

        MoveNext(context);
        ParseValue(context);
        Consume(context, TokenType.EndOfStream);

        ValidateEndState(context);
    }

    private Context IntializeContext(IReadOnlyList<Token> tokens)
        => new()
        {
            CursorState = new CursorState(-1, tokens),
        };

    private void ParseValue(Context context)
    {
        ValidateTokenNotNull(context);

        switch (context.CursorState.CurrentToken!.TokenType)
        {
            case TokenType.String:
                Consume(context, TokenType.String);
                break;

            case TokenType.Number:
                ParseNumber(context);
                break;

            case TokenType.Identifier:
                ParseIdentifier(context);
                break;

            case TokenType.LeftBrace:
                MoveNext(context);
                ParseObject(context);
                break;

            case TokenType.LeftBracket:
                MoveNext(context);
                ParseArray(context);
                break;

            default:
                throw new CustomJsonParserException($"Unexpected token at position {context.CursorState.CurrentToken!.StartIndex}. Expected a string, number, identifier, object or array!");
        }
    }

    private void ParseNumber(Context context)
    {
        ValidateExpectedToken(context, TokenType.Number);

        if (!double.TryParse(context.CursorState.CurrentToken!.Value, NumberStyles.Float, CultureInfo.InvariantCulture, out var _))
        {
            throw new CustomJsonParserException($"Number is in a incorrect format at position: {context.CursorState.CurrentToken.StartIndex}. Received {context.CursorState.CurrentToken.Value}.");
        }

        MoveNext(context);
    }

    private void ParseIdentifier(Context context)
    {
        ValidateExpectedToken(context, TokenType.Identifier);

        switch (context.CursorState.CurrentToken!.Value)
        {
            case "true" or "false" or "null":
                MoveNext(context);
                break;

            default:
                throw new CustomJsonParserException($"Unexpected token at position {context.CursorState.CurrentToken!.StartIndex}. Expected true, false or null!");
        }
    }

    private void ParseObject(Context context)
    {
        ValidateTokenNotNull(context);

        if (context.CursorState.CurrentToken!.TokenType != TokenType.RightBrace)
        {
            ParsePair(context);
            ParseRemainingCommaSeparated(context, ParsePair);
        }

        Consume(context, TokenType.RightBrace);
    }

    private void ParseArray(Context context)
    {
        ValidateTokenNotNull(context);

        if (context.CursorState.CurrentToken!.TokenType != TokenType.RightBracket)
        {
            ParseValue(context);
            ParseRemainingCommaSeparated(context, ParseValue);
        }

        Consume(context, TokenType.RightBracket);
    }

    private void ParsePair(Context context)
    {
        Consume(context, TokenType.String);
        Consume(context, TokenType.Colon);
        ParseValue(context);
    }

    private void Consume(Context context, TokenType expectedTokenType)
    {
        ValidateExpectedToken(context, expectedTokenType);
        MoveNext(context);
    }

    private void ParseRemainingCommaSeparated(Context context, Action<Context> parseItem)
    {
        while (context.CursorState.CurrentToken?.TokenType == TokenType.Comma)
        {
            Consume(context, TokenType.Comma);
            parseItem(context);
        }
    }

    private void MoveNext(Context context)
        => context.CursorState.CurrentToken = context.CursorState.CurrentPosition >= context.CursorState.TokenStream.Count - 1
            ? null
            : context.CursorState.TokenStream[++context.CursorState.CurrentPosition];

    private void ValidateTokenNotNull(Context context)
    {
        if (context.CursorState.CurrentToken is null)
        {
            throw new CustomJsonParserException($"A token was expected but no token was found at position {context.CursorState.CurrentToken!.StartIndex}!");
        }
    }

    private void ValidateExpectedToken(Context context, TokenType expectedTokenType)
    {
        ValidateTokenNotNull(context);

        if (context.CursorState.CurrentToken!.TokenType != expectedTokenType)
        {
            throw new CustomJsonParserException($"A token of type {expectedTokenType} was expected but a token of type {context.CursorState.CurrentToken.TokenType} was received at position {context.CursorState.CurrentToken!.StartIndex}!");
        }
    }

    private void ValidateEndState(Context context)
    {
        if (context.CursorState.CurrentToken?.TokenType is not TokenType.EndOfStream)
        {
            throw new CustomJsonParserException();
        }
    }
}