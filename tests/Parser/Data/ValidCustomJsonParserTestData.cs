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

        yield return [
            new JsonParserTestData([
                new Token(TokenType.LeftBrace, "{"),

                new Token(TokenType.String, "Person"),
                new Token(TokenType.Colon, ":"),

                new Token(TokenType.LeftBrace, "{"),

                new Token(TokenType.String, "Name"),
                new Token(TokenType.Colon, ":"),
                new Token(TokenType.String, "John Doe"),

                new Token(TokenType.RightBrace, "}"),

                new Token(TokenType.RightBrace, "}")
            ])
        ];

        yield return [
            new JsonParserTestData([
                    new Token(TokenType.LeftBrace, "{"),

                    new Token(TokenType.String, "Person"),
                    new Token(TokenType.Colon, ":"),

                    new Token(TokenType.LeftBrace, "{"),

                    new Token(TokenType.String, "Name"),
                    new Token(TokenType.Colon, ":"),
                    new Token(TokenType.String, "John Doe"),
                    new Token(TokenType.Comma, ","),

                    new Token(TokenType.String, "Age"),
                    new Token(TokenType.Colon, ":"),
                    new Token(TokenType.PrimitiveValue, "30"),
                    new Token(TokenType.Comma, ","),

                    new Token(TokenType.String, "Citizenship"),
                    new Token(TokenType.Colon, ":"),
                    new Token(TokenType.PrimitiveValue, "false"),
                    new Token(TokenType.Comma, ","),

                    new Token(TokenType.String, "OtherDetails"),
                    new Token(TokenType.Colon, ":"),
                    new Token(TokenType.PrimitiveValue, "null"),

                    new Token(TokenType.RightBrace, "}"),

                    new Token(TokenType.RightBrace, "}")
                ])
        ];

        yield return [
            new JsonParserTestData( [
                    new Token(TokenType.LeftBrace, "{"),

                    new Token(TokenType.String, "PersonalDetails"),
                    new Token(TokenType.Colon, ":"),

                    new Token(TokenType.LeftBrace, "{"),

                    new Token(TokenType.String, "Name"),
                    new Token(TokenType.Colon, ":"),
                    new Token(TokenType.String, "John Doe"),
                    new Token(TokenType.Comma, ","),

                    new Token(TokenType.String, "Age"),
                    new Token(TokenType.Colon, ":"),
                    new Token(TokenType.PrimitiveValue, "30"),
                    new Token(TokenType.Comma, ","),

                    new Token(TokenType.String, "Citizenship"),
                    new Token(TokenType.Colon, ":"),
                    new Token(TokenType.PrimitiveValue, "false"),
                    new Token(TokenType.Comma, ","),

                    new Token(TokenType.String, "OtherDetails"),
                    new Token(TokenType.Colon, ":"),
                    new Token(TokenType.PrimitiveValue, "null"),

                    new Token(TokenType.RightBrace, "}"),
                    new Token(TokenType.Comma, ","),

                    new Token(TokenType.String, "BankDetails"),
                    new Token(TokenType.Colon, ":"),

                    new Token(TokenType.LeftBrace, "{"),

                    new Token(TokenType.String, "AccountNumber"),
                    new Token(TokenType.Colon, ":"),
                    new Token(TokenType.String, "100-23435454"),
                    new Token(TokenType.Comma, ","),

                    new Token(TokenType.String, "Bank"),
                    new Token(TokenType.Colon, ":"),
                    new Token(TokenType.String, "New Bank"),
                    new Token(TokenType.Comma, ","),

                    new Token(TokenType.String, "Active"),
                    new Token(TokenType.Colon, ":"),
                    new Token(TokenType.PrimitiveValue, "true"),

                    new Token(TokenType.RightBrace, "}"),

                    new Token(TokenType.RightBrace, "}")
                ])
        ];

        yield return [
            new JsonParserTestData([
                    new Token(TokenType.LeftBrace, "{"),

                    new Token(TokenType.String, "PersonalDetails"),
                    new Token(TokenType.Colon, ":"),

                    new Token(TokenType.LeftBrace, "{"),

                    new Token(TokenType.String, "Name"),
                    new Token(TokenType.Colon, ":"),
                    new Token(TokenType.String, "John Doe"),
                    new Token(TokenType.Comma, ","),

                    new Token(TokenType.String, "Age"),
                    new Token(TokenType.Colon, ":"),
                    new Token(TokenType.PrimitiveValue, "30"),
                    new Token(TokenType.Comma, ","),

                    new Token(TokenType.String, "Citizenship"),
                    new Token(TokenType.Colon, ":"),
                    new Token(TokenType.PrimitiveValue, "false"),
                    new Token(TokenType.Comma, ","),

                    new Token(TokenType.String, "Contact Number"),
                    new Token(TokenType.Colon, ":"),
                    new Token(TokenType.String, "+1 123232"),
                    new Token(TokenType.Comma, ","),

                    new Token(TokenType.String, "OtherDetails"),
                    new Token(TokenType.Colon, ":"),

                    new Token(TokenType.LeftBrace, "{"),

                    new Token(TokenType.String, "Blood Group"),
                    new Token(TokenType.Colon, ":"),
                    new Token(TokenType.String, "O+"),
                    new Token(TokenType.Comma, ","),

                    new Token(TokenType.String, "Visa Expired"),
                    new Token(TokenType.Colon, ":"),
                    new Token(TokenType.PrimitiveValue, "false"),
                    new Token(TokenType.Comma, ","),

                    new Token(TokenType.String, "Emergency Contact Number"),
                    new Token(TokenType.Colon, ":"),
                    new Token(TokenType.String, "+1 234355"),

                    new Token(TokenType.RightBrace, "}"),

                    new Token(TokenType.RightBrace, "}"),
                    new Token(TokenType.Comma, ","),

                    new Token(TokenType.String, "BankDetails"),
                    new Token(TokenType.Colon, ":"),

                    new Token(TokenType.LeftBrace, "{"),

                    new Token(TokenType.String, "AccountNumber"),
                    new Token(TokenType.Colon, ":"),
                    new Token(TokenType.String, "100-23435454"),
                    new Token(TokenType.Comma, ","),

                    new Token(TokenType.String, "Bank"),
                    new Token(TokenType.Colon, ":"),
                    new Token(TokenType.String, "New Bank"),
                    new Token(TokenType.Comma, ","),

                    new Token(TokenType.String, "Active"),
                    new Token(TokenType.Colon, ":"),
                    new Token(TokenType.PrimitiveValue, "true"),

                    new Token(TokenType.RightBrace, "}"),

                    new Token(TokenType.RightBrace, "}")
                ])
        ];
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}