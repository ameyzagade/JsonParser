using System.Collections;
using JsonParser.App.TokenModel;

namespace JsonParser.Tests.Lexer.TestData;

public class TokenizableJsonInputScannerTestData : IEnumerable<object[]>
{
    public IEnumerator<object[]> GetEnumerator()
    {
        // Empty input
        yield return
        [
            new JsonInputScannerTestData("", [
                new Token(TokenType.EndOfStream, string.Empty, 1),
            ]),
        ];

        // Symbols
        yield return
        [
            new JsonInputScannerTestData("{}[],:", [
                new Token(TokenType.LeftBrace, "{", 1),
                new Token(TokenType.RightBrace, "}", 2),
                new Token(TokenType.LeftBracket, "[", 3),
                new Token(TokenType.RightBracket, "]", 4),
                new Token(TokenType.Comma, ",", 5),
                new Token(TokenType.Colon, ":", 6),
                new Token(TokenType.EndOfStream, string.Empty, 7),
            ]),
        ];

        // Whitespace is ignored
        yield return
        [
            new JsonInputScannerTestData("  {  }  ", [
                new Token(TokenType.LeftBrace, "{", 3),
                new Token(TokenType.RightBrace, "}", 6),
                new Token(TokenType.EndOfStream, string.Empty, 9),
            ]),
        ];

        // Empty and regular strings
        yield return
        [
            new JsonInputScannerTestData("""["","Hello World"]""", [
                new Token(TokenType.LeftBracket, "[", 1),
                new Token(TokenType.String, string.Empty, 2),
                new Token(TokenType.Comma, ",", 4),
                new Token(TokenType.String, "Hello World", 5),
                new Token(TokenType.RightBracket, "]", 18),
                new Token(TokenType.EndOfStream, string.Empty, 19),
            ]),
        ];

        // Unicode string support
        yield return
        [
            new JsonInputScannerTestData(
                """ { "Value": "\u4e16\u754c \ud83c\udf0d" } """, [
                    new Token(TokenType.LeftBrace, "{", 2),
                    new Token(TokenType.String, "Value", 4),
                    new Token(TokenType.Colon, ":", 11),
                    new Token(TokenType.String, "世界 🌍", 13),
                    new Token(TokenType.RightBrace, "}", 41),
                    new Token(TokenType.EndOfStream, string.Empty, 43),
                ]
            ),
        ];

        // Escaped characters in strings
        yield return
        [
            new JsonInputScannerTestData(
                """["Hello \"World\"","C:\\Users\\John"]""",
                [
                    new Token(TokenType.LeftBracket, "[", 1),
                    new Token(TokenType.String, "Hello \"World\"", 2),
                    new Token(TokenType.Comma, ",", 19),
                    new Token(TokenType.String, "C:\\Users\\John", 20),
                    new Token(TokenType.RightBracket, "]", 37),
                    new Token(TokenType.EndOfStream, string.Empty, 38),
                ])
        ];

        // Identifiers
        yield return
        [
            new JsonInputScannerTestData("[true,false,null]", [
                new Token(TokenType.LeftBracket, "[", 1),
                new Token(TokenType.Identifier, "true", 2),
                new Token(TokenType.Comma, ",", 6),
                new Token(TokenType.Identifier, "false", 7),
                new Token(TokenType.Comma, ",", 12),
                new Token(TokenType.Identifier, "null", 13),
                new Token(TokenType.RightBracket, "]", 17),
                new Token(TokenType.EndOfStream, string.Empty, 18),
            ]),
        ];

        // Numbers currently supported by the lexer
        yield return
        [
            new JsonInputScannerTestData("[123,-456,12.34]", [
                new Token(TokenType.LeftBracket, "[", 1),
                new Token(TokenType.Number, "123", 2),
                new Token(TokenType.Comma, ",", 5),
                new Token(TokenType.Number, "-456", 6),
                new Token(TokenType.Comma, ",", 10),
                new Token(TokenType.Number, "12.34", 11),
                new Token(TokenType.RightBracket, "]", 16),
                new Token(TokenType.EndOfStream, string.Empty, 17),
            ]),
        ];

        // Invalid token
        yield return
        [
            new JsonInputScannerTestData("[invalid]", [
                new Token(TokenType.LeftBracket, "[", 1),
                new Token(TokenType.Invalid, "invalid", 2),
                new Token(TokenType.RightBracket, "]", 9),
                new Token(TokenType.EndOfStream, string.Empty, 10),
            ]),
        ];

        // Simple object containing different value types
        yield return
        [
            new JsonInputScannerTestData(
                """{"Name":"John Doe","Age":30,"Active":true}""",
                [
                    new Token(TokenType.LeftBrace, "{", 1),
                    new Token(TokenType.String, "Name", 2),
                    new Token(TokenType.Colon, ":", 8),
                    new Token(TokenType.String, "John Doe", 9),
                    new Token(TokenType.Comma, ",", 19),
                    new Token(TokenType.String, "Age", 20),
                    new Token(TokenType.Colon, ":", 25),
                    new Token(TokenType.Number, "30", 26),
                    new Token(TokenType.Comma, ",", 28),
                    new Token(TokenType.String, "Active", 29),
                    new Token(TokenType.Colon, ":", 37),
                    new Token(TokenType.Identifier, "true", 38),
                    new Token(TokenType.RightBrace, "}", 42),
                    new Token(TokenType.EndOfStream, string.Empty, 43),
                ])
        ];

        // Nested object and array
        yield return
        [
            new JsonInputScannerTestData(
                """{"Person":{"Name":"John","Numbers":[1,2,3]}}""",
                [
                    new Token(TokenType.LeftBrace, "{", 1),
                    new Token(TokenType.String, "Person", 2),
                    new Token(TokenType.Colon, ":", 10),
                    new Token(TokenType.LeftBrace, "{", 11),
                    new Token(TokenType.String, "Name", 12),
                    new Token(TokenType.Colon, ":", 18),
                    new Token(TokenType.String, "John", 19),
                    new Token(TokenType.Comma, ",", 25),
                    new Token(TokenType.String, "Numbers", 26),
                    new Token(TokenType.Colon, ":", 35),
                    new Token(TokenType.LeftBracket, "[", 36),
                    new Token(TokenType.Number, "1", 37),
                    new Token(TokenType.Comma, ",", 38),
                    new Token(TokenType.Number, "2", 39),
                    new Token(TokenType.Comma, ",", 40),
                    new Token(TokenType.Number, "3", 41),
                    new Token(TokenType.RightBracket, "]", 42),
                    new Token(TokenType.RightBrace, "}", 43),
                    new Token(TokenType.RightBrace, "}", 44),
                    new Token(TokenType.EndOfStream, string.Empty, 45),
                ])
        ];
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
