using System.Collections;
using JsonParser.App.Lexer;

namespace JsonParser.Tests.Parser.TestData;

public class ValidCustomJsonParserTestData : IEnumerable<object[]>
{
    public IEnumerator<object[]> GetEnumerator()
    {
        yield return [
            new JsonParserTestData(
                [
                    new Token(TokenType.LeftBrace, "{"),
                    new Token(TokenType.RightBrace, "}")
                ]
            ),
        ];

        yield return [
            new JsonParserTestData(
                [
                    new Token(TokenType.LeftBrace, "{"),

                    new Token(TokenType.String, "key"),
                    new Token(TokenType.Colon, ":"),
                    new Token(TokenType.String, ""),

                    new Token(TokenType.RightBrace, "}")
                ]
            ),
        ];

        yield return [
            new JsonParserTestData(
                [
                    new Token(TokenType.LeftBrace, "{"),

                    new Token(TokenType.String, "key"),
                    new Token(TokenType.Colon, ":"),
                    new Token(TokenType.String, "Hello \"World\""),

                    new Token(TokenType.RightBrace, "}")
                ]
            ),
        ];

        yield return [
            new JsonParserTestData(
                [
                    new Token(TokenType.LeftBrace, "{"),

                    new Token(TokenType.String, "path"),
                    new Token(TokenType.Colon, ":"),
                    new Token(TokenType.String, "C:\\Users\\John"),

                    new Token(TokenType.RightBrace, "}")
                ]
            ),
        ];

        yield return [
            new JsonParserTestData(
                [
                    new Token(TokenType.LeftBrace, "{"),

                    new Token(TokenType.String, "Name"),
                    new Token(TokenType.Colon, ":"),
                    new Token(TokenType.String, "John Doe"),

                    new Token(TokenType.RightBrace, "}"),
                ]
            ),
        ];

        yield return [
            new JsonParserTestData(
                [
                    new Token(TokenType.LeftBrace, "{"),

                    new Token(TokenType.String, "Name"),
                    new Token(TokenType.Colon, ":"),
                    new Token(TokenType.String, ""),

                    new Token(TokenType.RightBrace, "}"),
                ]
            ),
        ];

        yield return [
            new JsonParserTestData(
                [
                    new Token(TokenType.LeftBrace, "{"),

                    new Token(TokenType.String, "Name"),
                    new Token(TokenType.Colon, ":"),
                    new Token(TokenType.String, "John \"Doe\""),

                    new Token(TokenType.RightBrace, "}"),
                ]
            ),
        ];

        yield return [
            new JsonParserTestData(
                [
                    new Token(TokenType.LeftBrace, "{"),

                    new Token(TokenType.String, "key"),
                    new Token(TokenType.Colon, ":"),
                    new Token(TokenType.PrimitiveValue, "true"),

                    new Token(TokenType.RightBrace, "}"),
                ]
            ),
        ];

        yield return [
            new JsonParserTestData(
                [
                    new Token(TokenType.LeftBrace, "{"),

                    new Token(TokenType.String, "key"),
                    new Token(TokenType.Colon, ":"),
                    new Token(TokenType.String, "value"),
                    new Token(TokenType.Comma, ","),

                    new Token(TokenType.String, "key2"),
                    new Token(TokenType.Colon, ":"),
                    new Token(TokenType.String, "value"),

                    new Token(TokenType.RightBrace, "}"),
                ]
            ),
        ];

        yield return [
            new JsonParserTestData(
                [
                    new Token(TokenType.LeftBrace, "{"),

                    new Token(TokenType.String, "key1"),
                    new Token(TokenType.Colon, ":"),
                    new Token(TokenType.PrimitiveValue, "true"),
                    new Token(TokenType.Comma, ","),

                    new Token(TokenType.String, "key2"),
                    new Token(TokenType.Colon, ":"),
                    new Token(TokenType.PrimitiveValue, "false"),
                    new Token(TokenType.Comma, ","),

                    new Token(TokenType.String, "key3"),
                    new Token(TokenType.Colon, ":"),
                    new Token(TokenType.PrimitiveValue, "null"),
                    new Token(TokenType.Comma, ","),

                    new Token(TokenType.String, "key4"),
                    new Token(TokenType.Colon, ":"),
                    new Token(TokenType.String, "value"),
                    new Token(TokenType.Comma, ","),

                    new Token(TokenType.String, "key5"),
                    new Token(TokenType.Colon, ":"),
                    new Token(TokenType.PrimitiveValue, "101"),

                    new Token(TokenType.RightBrace, "}"),
                ]
            ),
        ];
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}