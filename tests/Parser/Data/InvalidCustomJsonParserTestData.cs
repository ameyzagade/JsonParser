using System.Collections;
using JsonParser.App.TokenModel;

namespace JsonParser.Tests.Parser.TestData;

public class InvalidCustomJsonParserTestData : IEnumerable<object[]>
{
    public IEnumerator<object[]> GetEnumerator()
    {
        // Empty JSON: EndOfStream cannot be parsed as a value
        yield return
        [
            new JsonParserTestData(
                [
                    new Token(TokenType.EndOfStream, string.Empty, 1),
                ],
                "Unexpected token at position 1. Expected a string, number, identifier, object or array!"
            ),
        ];

        // Invalid top-level token: ,
        yield return
        [
            new JsonParserTestData(
                [
                    new Token(TokenType.Comma, ",", 1),
                    new Token(TokenType.EndOfStream, string.Empty, 2),
                ],
                "Unexpected token at position 1. Expected a string, number, identifier, object or array!"
            ),
        ];

        // Extra token after a complete value:
        // true false
        yield return
        [
            new JsonParserTestData(
                [
                    new Token(TokenType.Identifier, "true", 1),
                    new Token(TokenType.Identifier, "false", 6),
                    new Token(TokenType.EndOfStream, string.Empty, 11),
                ],
                "A token of type EndOfStream was expected but a token of type Identifier was received at position 6!"
            ),
        ];

        // Invalid identifier
        yield return
        [
            new JsonParserTestData(
                [
                    new Token(TokenType.Identifier, "True", 1),
                    new Token(TokenType.EndOfStream, string.Empty, 5),
                ],
                "Unexpected token at position 1. Expected true, false or null!"
            ),
        ];

        // Invalid number
        yield return
        [
            new JsonParserTestData(
                [
                    new Token(TokenType.Number, "12.2.2", 1),
                    new Token(TokenType.EndOfStream, string.Empty, 7),
                ],
                "Number is in a incorrect format at position: 1. Received 12.2.2."
            ),
        ];

        // Object missing closing brace:
        // {"Name":"John"
        yield return
        [
            new JsonParserTestData(
                [
                    new Token(TokenType.LeftBrace, "{", 1),
                    new Token(TokenType.String, "Name", 2),
                    new Token(TokenType.Colon, ":", 8),
                    new Token(TokenType.String, "John", 9),
                    new Token(TokenType.EndOfStream, string.Empty, 15),
                ],
                "A token of type RightBrace was expected but a token of type EndOfStream was received at position 15!"
            ),
        ];

        // Object key must be a string:
        // {true:"John"}
        yield return
        [
            new JsonParserTestData(
                [
                    new Token(TokenType.LeftBrace, "{", 1),
                    new Token(TokenType.Identifier, "true", 2),
                    new Token(TokenType.Colon, ":", 6),
                    new Token(TokenType.String, "John", 7),
                    new Token(TokenType.RightBrace, "}", 13),
                    new Token(TokenType.EndOfStream, string.Empty, 14),
                ],
                "A token of type String was expected but a token of type Identifier was received at position 2!"
            ),
        ];

        // Missing colon:
        // {"Name" "John"}
        yield return
        [
            new JsonParserTestData(
                [
                    new Token(TokenType.LeftBrace, "{", 1),
                    new Token(TokenType.String, "Name", 2),
                    new Token(TokenType.String, "John", 9),
                    new Token(TokenType.RightBrace, "}", 15),
                    new Token(TokenType.EndOfStream, string.Empty, 16),
                ],
                "A token of type Colon was expected but a token of type String was received at position 9!"
            ),
        ];

        // Missing value:
        // {"Name":}
        yield return
        [
            new JsonParserTestData(
                [
                    new Token(TokenType.LeftBrace, "{", 1),
                    new Token(TokenType.String, "Name", 2),
                    new Token(TokenType.Colon, ":", 8),
                    new Token(TokenType.RightBrace, "}", 9),
                    new Token(TokenType.EndOfStream, string.Empty, 10),
                ],
                "Unexpected token at position 9. Expected a string, number, identifier, object or array!"
            ),
        ];

        // Missing comma between object pairs:
        // {"Name":"John" "Age":30}
        yield return
        [
            new JsonParserTestData(
                [
                    new Token(TokenType.LeftBrace, "{", 1),

                    new Token(TokenType.String, "Name", 2),
                    new Token(TokenType.Colon, ":", 8),
                    new Token(TokenType.String, "John", 9),

                    new Token(TokenType.String, "Age", 16),
                    new Token(TokenType.Colon, ":", 21),
                    new Token(TokenType.Number, "30", 22),

                    new Token(TokenType.RightBrace, "}", 24),
                    new Token(TokenType.EndOfStream, string.Empty, 25),
                ],
                "A token of type RightBrace was expected but a token of type String was received at position 16!"
            ),
        ];

        // Trailing comma in object:
        // {"Name":"John",}
        yield return
        [
            new JsonParserTestData(
                [
                    new Token(TokenType.LeftBrace, "{", 1),
                    new Token(TokenType.String, "Name", 2),
                    new Token(TokenType.Colon, ":", 8),
                    new Token(TokenType.String, "John", 9),
                    new Token(TokenType.Comma, ",", 15),
                    new Token(TokenType.RightBrace, "}", 16),
                    new Token(TokenType.EndOfStream, string.Empty, 17),
                ],
                "A token of type String was expected but a token of type RightBrace was received at position 16!"
            ),
        ];

        // Array missing closing bracket:
        // [1,2,3
        yield return
        [
            new JsonParserTestData(
                [
                    new Token(TokenType.LeftBracket, "[", 1),
                    new Token(TokenType.Number, "1", 2),
                    new Token(TokenType.Comma, ",", 3),
                    new Token(TokenType.Number, "2", 4),
                    new Token(TokenType.Comma, ",", 5),
                    new Token(TokenType.Number, "3", 6),
                    new Token(TokenType.EndOfStream, string.Empty, 7),
                ],
                "A token of type RightBracket was expected but a token of type EndOfStream was received at position 7!"
            ),
        ];

        // Missing comma between array values:
        // [1 2]
        yield return
        [
            new JsonParserTestData(
                [
                    new Token(TokenType.LeftBracket, "[", 1),
                    new Token(TokenType.Number, "1", 2),
                    new Token(TokenType.Number, "2", 4),
                    new Token(TokenType.RightBracket, "]", 5),
                    new Token(TokenType.EndOfStream, string.Empty, 6),
                ],
                "A token of type RightBracket was expected but a token of type Number was received at position 4!"
            ),
        ];

        // Trailing comma in array:
        // [1,2,]
        yield return
        [
            new JsonParserTestData(
                [
                    new Token(TokenType.LeftBracket, "[", 1),
                    new Token(TokenType.Number, "1", 2),
                    new Token(TokenType.Comma, ",", 3),
                    new Token(TokenType.Number, "2", 4),
                    new Token(TokenType.Comma, ",", 5),
                    new Token(TokenType.RightBracket, "]", 6),
                    new Token(TokenType.EndOfStream, string.Empty, 7),
                ],
                "Unexpected token at position 6. Expected a string, number, identifier, object or array!"
            ),
        ];

        // Mismatched object delimiter:
        // {"Name":"John"]
        yield return
        [
            new JsonParserTestData(
                [
                    new Token(TokenType.LeftBrace, "{", 1),
                    new Token(TokenType.String, "Name", 2),
                    new Token(TokenType.Colon, ":", 8),
                    new Token(TokenType.String, "John", 9),
                    new Token(TokenType.RightBracket, "]", 15),
                    new Token(TokenType.EndOfStream, string.Empty, 16),
                ],
                "A token of type RightBrace was expected but a token of type RightBracket was received at position 15!"
            ),
        ];

        // Mismatched array delimiter:
        // [1,2,3}
        yield return
        [
            new JsonParserTestData(
                [
                    new Token(TokenType.LeftBracket, "[", 1),
                    new Token(TokenType.Number, "1", 2),
                    new Token(TokenType.Comma, ",", 3),
                    new Token(TokenType.Number, "2", 4),
                    new Token(TokenType.Comma, ",", 5),
                    new Token(TokenType.Number, "3", 6),
                    new Token(TokenType.RightBrace, "}", 7),
                    new Token(TokenType.EndOfStream, string.Empty, 8),
                ],
                "A token of type RightBracket was expected but a token of type RightBrace was received at position 7!"
            ),
        ];
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}