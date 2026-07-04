using System.Collections;
using JsonParser.App.Lexer;

namespace JsonParser.Tests.TestData;

public class ValidJsonTestData : IEnumerable<object[]>
{
    public IEnumerator<object[]> GetEnumerator()
    {
        yield return [
            new JsonTestData("", [])
        ];

        yield return [
            new JsonTestData("""{}""", [
                new Token(TokenType.LeftBrace, "{"),
                new Token(TokenType.RightBrace, "}")
            ])
        ];

        yield return [
            new JsonTestData("""{ "Name": "John Doe" }""", [
                new Token(TokenType.LeftBrace, "{"),

                new Token(TokenType.String, "Name"),
                new Token(TokenType.Colon, ":"),
                new Token(TokenType.String, "John Doe"),

                new Token(TokenType.RightBrace, "}"),
            ])
        ];

        yield return [
           new JsonTestData("""{ "Name": "" }""", [
                new Token(TokenType.LeftBrace, "{"),

                new Token(TokenType.String, "Name"),
                new Token(TokenType.Colon, ":"),
                new Token(TokenType.String, ""),

                new Token(TokenType.RightBrace, "}"),
            ])
        ];

        yield return [
           new JsonTestData("""{ "Name": "John \"Doe\"" }""", [
                new Token(TokenType.LeftBrace, "{"),

                new Token(TokenType.String, "Name"),
                new Token(TokenType.Colon, ":"),
                new Token(TokenType.String, "John \"Doe\""),

                new Token(TokenType.RightBrace, "}"),
            ])
        ];

        yield return [
           new JsonTestData("""{"key": true     }""", [
                new Token(TokenType.LeftBrace, "{"),

                new Token(TokenType.String, "key"),
                new Token(TokenType.Colon, ":"),
                new Token(TokenType.PrimitiveValue, "true"),

                new Token(TokenType.RightBrace, "}"),
            ])
        ];

        yield return [
           new JsonTestData("""
           {
            "key": "value",
            "key2": "value"
           }
           """, [
                new Token(TokenType.LeftBrace, "{"),

                new Token(TokenType.String, "key"),
                new Token(TokenType.Colon, ":"),
                new Token(TokenType.String, "value"),
                new Token(TokenType.Comma, ","),

                new Token(TokenType.String, "key2"),
                new Token(TokenType.Colon, ":"),
                new Token(TokenType.String, "value"),

                new Token(TokenType.RightBrace, "}"),
           ])
        ];

        yield return [
            new JsonTestData("""
            {
                "key1": true,
                "key2": false,
                "key3": null,
                "key4": "value",
                "key5": 101
            }
            """, [
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
            ])
        ];
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}