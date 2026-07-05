using System.Collections;
using JsonParser.App.Lexer;

namespace JsonParser.Tests.Parser.TestData;

public class InvalidCustomJsonParserTestData : IEnumerable<object[]>
{
    public IEnumerator<object[]> GetEnumerator()
    {
        yield return [
            new JsonParserTestData(
                [
                    new Token(TokenType.String, "value"),
                ],
                "Unexpected token received at position 1. Token: value. Expected Token: Left Brace."
            ),
        ];

        yield return [
            new JsonParserTestData(
                [
                    new Token(TokenType.PrimitiveValue, "1"),
                ],
                "Unexpected token received at position 1. Token: 1. Expected Token: Left Brace."
            ),
        ];

        yield return [
            new JsonParserTestData(
                [
                    new Token(TokenType.Comma, ","),
                ],
                "Unexpected token received at position 1. Token: ,. Expected Token: Left Brace."
            ),
        ];

        yield return [
            new JsonParserTestData(
                [
                    new Token(TokenType.Colon, ":"),
                ],
                "Unexpected token received at position 1. Token: :. Expected Token: Left Brace."
            ),
        ];

        yield return [
            new JsonParserTestData(
                [
                    new Token(TokenType.RightBrace, "}"),
                ],
                "Unexpected token received at position 1. Token: }. Expected Token: Left Brace."
            ),
        ];

        yield return [
            new JsonParserTestData(
                [
                    new Token(TokenType.LeftBrace, "{"),

                    new Token(TokenType.Colon, ":"),
                ],
                "Unexpected token received at position 2. Token: :. Expected Token: String or Right Brace."
            ),
        ];

        yield return [
            new JsonParserTestData(
                [
                    new Token(TokenType.LeftBrace, "{"),

                    new Token(TokenType.LeftBrace, "{"),
                ],
                "Unexpected token received at position 2. Token: {. Expected Token: String or Right Brace."
            ),
        ];

        yield return [
            new JsonParserTestData(
                [
                    new Token(TokenType.LeftBrace, "{"),

                    new Token(TokenType.Comma, ","),
                ],
                "Unexpected token received at position 2. Token: ,. Expected Token: String or Right Brace."
            ),
        ];

        yield return [
            new JsonParserTestData(
                [
                    new Token(TokenType.LeftBrace, "{"),

                    new Token(TokenType.PrimitiveValue, "1"),
                ],
                "Unexpected token received at position 2. Token: 1. Expected Token: String or Right Brace."
            ),
        ];

        yield return [
            new JsonParserTestData(
                [
                    new Token(TokenType.LeftBrace, "{"),

                    new Token(TokenType.String, "Name"),
                    new Token(TokenType.String, "John"),
                ],
                "Unexpected token received at position 6. Token: John. Expected Token: Colon."
            ),
        ];

        yield return [
            new JsonParserTestData(
                [
                    new Token(TokenType.LeftBrace, "{"),

                    new Token(TokenType.String, "Name"),
                    new Token(TokenType.PrimitiveValue, "2"),
                ],
                "Unexpected token received at position 6. Token: 2. Expected Token: Colon."
            ),
        ];

        yield return [
            new JsonParserTestData(
                [
                    new Token(TokenType.LeftBrace, "{"),

                    new Token(TokenType.String, "Name"),
                    new Token(TokenType.Comma, ","),
                ],
                "Unexpected token received at position 6. Token: ,. Expected Token: Colon."
            ),
        ];

        yield return [
            new JsonParserTestData(
                [
                    new Token(TokenType.LeftBrace, "{"),

                    new Token(TokenType.String, "Name"),
                    new Token(TokenType.LeftBrace, "{"),
                ],
                "Unexpected token received at position 6. Token: {. Expected Token: Colon."
            ),
        ];

        yield return [
            new JsonParserTestData(
                [
                    new Token(TokenType.LeftBrace, "{"),

                    new Token(TokenType.String, "Name"),
                    new Token(TokenType.RightBrace, "}"),
                ],
                "Unexpected token received at position 6. Token: }. Expected Token: Colon."
            ),
        ];

        yield return [
            new JsonParserTestData(
                [
                    new Token(TokenType.LeftBrace, "{"),

                    new Token(TokenType.PrimitiveValue, "true"),
                    new Token(TokenType.Colon, ":"),
                    new Token(TokenType.RightBrace, "}"),
                ],
                "Unexpected token received at position 2. Token: true. Expected Token: String or Right Brace."
            ),
        ];

        yield return [
            new JsonParserTestData(
                [
                    new Token(TokenType.LeftBrace, "{"),

                    new Token(TokenType.String, "Name"),
                    new Token(TokenType.Colon, ":"),
                    new Token(TokenType.LeftBrace, "{"),
                ],
                "Unexpected token received at position 7. Token: {."
            ),
        ];

        yield return [
            new JsonParserTestData(
                [
                    new Token(TokenType.LeftBrace, "{"),

                    new Token(TokenType.String, "Name"),
                    new Token(TokenType.Colon, ":"),
                    new Token(TokenType.RightBrace, "}"),
                ],
                "Unexpected token received at position 7. Token: }."
            ),
        ];

        yield return [
            new JsonParserTestData(
                [
                    new Token(TokenType.LeftBrace, "{"),

                    new Token(TokenType.String, "Name"),
                    new Token(TokenType.Colon, ":"),
                    new Token(TokenType.Colon, ":"),
                ],
                "Unexpected token received at position 7. Token: :."
            ),
        ];

        yield return [
            new JsonParserTestData(
                [
                    new Token(TokenType.LeftBrace, "{"),

                    new Token(TokenType.String, "Name"),
                    new Token(TokenType.Colon, ":"),
                    new Token(TokenType.Comma, ","),
                ],
                "Unexpected token received at position 7. Token: ,."
            ),
        ];

        yield return [
            new JsonParserTestData(
                [
                    new Token(TokenType.LeftBrace, "{"),

                    new Token(TokenType.String, "Name"),
                    new Token(TokenType.Colon, ":"),
                    new Token(TokenType.PrimitiveValue, "True"),
                ],
                "Unexpected token received at position 7. Token: True."
            ),
        ];

        yield return [
            new JsonParserTestData(
                [
                    new Token(TokenType.LeftBrace, "{"),

                    new Token(TokenType.String, "Name"),
                    new Token(TokenType.Colon, ":"),
                    new Token(TokenType.PrimitiveValue, "False"),
                ],
                "Unexpected token received at position 7. Token: False."
            ),
        ];

        yield return [
            new JsonParserTestData(
                [
                    new Token(TokenType.LeftBrace, "{"),

                    new Token(TokenType.String, "Name"),
                    new Token(TokenType.Colon, ":"),
                    new Token(TokenType.PrimitiveValue, "Null"),
                ],
                "Unexpected token received at position 7. Token: Null."
            ),
        ];

        yield return [
            new JsonParserTestData(
                [
                    new Token(TokenType.LeftBrace, "{"),

                    new Token(TokenType.String, "Name"),
                    new Token(TokenType.Colon, ":"),
                    new Token(TokenType.PrimitiveValue, "12.2.2"),
                ],
                "Unexpected token received at position 7. Token: 12.2.2."
            ),
        ];

        yield return [
            new JsonParserTestData(
                [
                    new Token(TokenType.LeftBrace, "{"),

                    new Token(TokenType.String, "Name"),
                    new Token(TokenType.Colon, ":"),
                    new Token(TokenType.String, "John Doe"),
                    new Token(TokenType.LeftBrace, "}"),
                ],
                "Unexpected token received at position 15. Token: }. Expected Token: Comma or Right Brace."
            ),
        ];

        yield return [
            new JsonParserTestData(
                [
                    new Token(TokenType.LeftBrace, "{"),

                    new Token(TokenType.String, "Name"),
                    new Token(TokenType.Colon, ":"),
                    new Token(TokenType.String, "John Doe"),
                    new Token(TokenType.LeftBrace, "{"),
                ],
                "Unexpected token received at position 15. Token: {. Expected Token: Comma or Right Brace."
            ),
        ];

        yield return [
            new JsonParserTestData(
                [
                    new Token(TokenType.LeftBrace, "{"),

                    new Token(TokenType.String, "Name"),
                    new Token(TokenType.Colon, ":"),
                    new Token(TokenType.String, "John Doe"),
                    new Token(TokenType.Colon, ":"),
                ],
                "Unexpected token received at position 15. Token: :. Expected Token: Comma or Right Brace."
            ),
        ];

        yield return [
            new JsonParserTestData(
                [
                    new Token(TokenType.LeftBrace, "{"),

                    new Token(TokenType.String, "Name"),
                    new Token(TokenType.Colon, ":"),
                    new Token(TokenType.String, "John Doe"),
                    new Token(TokenType.String, "value"),
                ],
                "Unexpected token received at position 15. Token: value. Expected Token: Comma or Right Brace."
            ),
        ];

        yield return [
            new JsonParserTestData(
                [
                    new Token(TokenType.LeftBrace, "{"),

                    new Token(TokenType.String, "Age"),
                    new Token(TokenType.Colon, ":"),
                    new Token(TokenType.PrimitiveValue, "30"),
                    new Token(TokenType.PrimitiveValue, "true"),
                ],
                "Unexpected token received at position 8. Token: true. Expected Token: Comma or Right Brace."
            ),
        ];

        yield return [
            new JsonParserTestData(
                [
                    new Token(TokenType.LeftBrace, "{"),

                    new Token(TokenType.String, "Age"),
                    new Token(TokenType.Colon, ":"),
                    new Token(TokenType.PrimitiveValue, "1"),
                    new Token(TokenType.Comma, ","),
                ],
                "Parser not in the expected Complete state after consuming all tokens!"
            ),
        ];

        yield return [
            new JsonParserTestData(
                [
                    new Token(TokenType.LeftBrace, "{"),

                    new Token(TokenType.String, "Age"),
                    new Token(TokenType.Colon, ":"),
                    new Token(TokenType.PrimitiveValue, "1"),
                    new Token(TokenType.Comma, ","),

                    new Token(TokenType.LeftBrace, "{"),
                ],
                "Unexpected token received at position 8. Token: {. Expected Token: String."
            ),
        ];

        yield return [
            new JsonParserTestData(
                [
                    new Token(TokenType.LeftBrace, "{"),

                    new Token(TokenType.String, "Age"),
                    new Token(TokenType.Colon, ":"),
                    new Token(TokenType.PrimitiveValue, "1"),
                    new Token(TokenType.Comma, ","),

                    new Token(TokenType.RightBrace, "}"),
                ],
                "Unexpected token received at position 8. Token: }. Expected Token: String."
            ),
        ];

        yield return [
            new JsonParserTestData(
                [
                    new Token(TokenType.LeftBrace, "{"),

                    new Token(TokenType.String, "Age"),
                    new Token(TokenType.Colon, ":"),
                    new Token(TokenType.PrimitiveValue, "1"),
                    new Token(TokenType.Comma, ","),

                    new Token(TokenType.LeftBrace, "{"),
                ],
                "Unexpected token received at position 8. Token: {. Expected Token: String."
            ),
        ];

        yield return [
            new JsonParserTestData(
                [
                    new Token(TokenType.LeftBrace, "{"),

                    new Token(TokenType.String, "Age"),
                    new Token(TokenType.Colon, ":"),
                    new Token(TokenType.PrimitiveValue, "1"),
                    new Token(TokenType.Comma, ","),

                    new Token(TokenType.LeftBrace, "{"),
                ],
                "Unexpected token received at position 8. Token: {. Expected Token: String."
            ),
        ];

        yield return [
            new JsonParserTestData(
                [
                    new Token(TokenType.LeftBrace, "{"),

                    new Token(TokenType.String, "Age"),
                    new Token(TokenType.Colon, ":"),
                    new Token(TokenType.PrimitiveValue, "1"),
                    new Token(TokenType.Comma, ","),

                    new Token(TokenType.Colon, ":"),
                ],
                "Unexpected token received at position 8. Token: :. Expected Token: String."
            ),
        ];

        yield return [
            new JsonParserTestData(
                [
                    new Token(TokenType.LeftBrace, "{"),

                    new Token(TokenType.String, "Age"),
                    new Token(TokenType.Colon, ":"),
                    new Token(TokenType.PrimitiveValue, "1"),
                    new Token(TokenType.Comma, ","),

                    new Token(TokenType.Comma, ","),
                ],
                "Unexpected token received at position 8. Token: ,. Expected Token: String."
            ),
        ];

        yield return [
            new JsonParserTestData(
                [
                    new Token(TokenType.LeftBrace, "{"),

                    new Token(TokenType.String, "Age"),
                    new Token(TokenType.Colon, ":"),
                    new Token(TokenType.PrimitiveValue, "1"),
                    new Token(TokenType.Comma, ","),

                    new Token(TokenType.PrimitiveValue, "2"),
                ],
                "Unexpected token received at position 8. Token: 2. Expected Token: String."
            ),
        ];

        yield return [
            new JsonParserTestData(
                [
                    new Token(TokenType.LeftBrace, "{"),

                    new Token(TokenType.String, "Age"),
                    new Token(TokenType.Colon, ":"),
                    new Token(TokenType.PrimitiveValue, "1"),
                    new Token(TokenType.Comma, ","),

                    new Token(TokenType.String, "Name"),
                ],
                "Parser not in the expected Complete state after consuming all tokens!"
            ),
        ];

        yield return [
            new JsonParserTestData(
                [
                    new Token(TokenType.LeftBrace, "{"),

                    new Token(TokenType.String, "Age"),
                    new Token(TokenType.Colon, ":"),
                    new Token(TokenType.PrimitiveValue, "1"),
                    new Token(TokenType.Comma, ","),

                    new Token(TokenType.String, "Pincode"),
                    new Token(TokenType.Colon, ":"),
                    new Token(TokenType.String, "400 001"),
                ],
                "Parser not in the expected Complete state after consuming all tokens!"
            ),
        ];
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}