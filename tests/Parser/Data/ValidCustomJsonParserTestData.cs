using System.Collections;
using JsonParser.App.TokenModel;

namespace JsonParser.Tests.Parser.TestData;

public class ValidCustomJsonParserTestData : IEnumerable<object[]>
{
    public IEnumerator<object[]> GetEnumerator()
    {
        // Top-level string
        yield return
        [
            new JsonParserTestData(
                [
                    new Token(TokenType.String, "Hello", 1),
                    new Token(TokenType.EndOfStream, string.Empty, 8),
                ]
            ),
        ];

        // Top-level number
        yield return
        [
            new JsonParserTestData(
                [
                    new Token(TokenType.Number, "123", 1),
                    new Token(TokenType.EndOfStream, string.Empty, 4),
                ]
            ),
        ];

        // Top-level identifier
        yield return
        [
            new JsonParserTestData(
                [
                    new Token(TokenType.Identifier, "true", 1),
                    new Token(TokenType.EndOfStream, string.Empty, 5),
                ]
            ),
        ];

        // Empty object: {}
        yield return
        [
            new JsonParserTestData(
                [
                    new Token(TokenType.LeftBrace, "{", 1),
                    new Token(TokenType.RightBrace, "}", 2),
                    new Token(TokenType.EndOfStream, string.Empty, 3),
                ]
            ),
        ];

        // Empty array: []
        yield return
        [
            new JsonParserTestData(
                [
                    new Token(TokenType.LeftBracket, "[", 1),
                    new Token(TokenType.RightBracket, "]", 2),
                    new Token(TokenType.EndOfStream, string.Empty, 3),
                ]
            ),
        ];

        // Object containing all primitive value types:
        // {"Name":"John","Age":30,"Active":true,"Details":null}
        yield return
        [
            new JsonParserTestData(
                [
                    new Token(TokenType.LeftBrace, "{", 1),

                    new Token(TokenType.String, "Name", 2),
                    new Token(TokenType.Colon, ":", 8),
                    new Token(TokenType.String, "John", 9),

                    new Token(TokenType.Comma, ",", 15),

                    new Token(TokenType.String, "Age", 16),
                    new Token(TokenType.Colon, ":", 21),
                    new Token(TokenType.Number, "30", 22),

                    new Token(TokenType.Comma, ",", 24),

                    new Token(TokenType.String, "Active", 25),
                    new Token(TokenType.Colon, ":", 33),
                    new Token(TokenType.Identifier, "true", 34),

                    new Token(TokenType.Comma, ",", 38),

                    new Token(TokenType.String, "Details", 39),
                    new Token(TokenType.Colon, ":", 48),
                    new Token(TokenType.Identifier, "null", 49),

                    new Token(TokenType.RightBrace, "}", 53),
                    new Token(TokenType.EndOfStream, string.Empty, 54),
                ]
            ),
        ];

        // Nested object:
        // {"Person":{"Name":"John","Age":30}}
        yield return
        [
            new JsonParserTestData(
                [
                    new Token(TokenType.LeftBrace, "{", 1),

                    new Token(TokenType.String, "Person", 2),
                    new Token(TokenType.Colon, ":", 10),

                    new Token(TokenType.LeftBrace, "{", 11),

                    new Token(TokenType.String, "Name", 12),
                    new Token(TokenType.Colon, ":", 18),
                    new Token(TokenType.String, "John", 19),

                    new Token(TokenType.Comma, ",", 25),

                    new Token(TokenType.String, "Age", 26),
                    new Token(TokenType.Colon, ":", 31),
                    new Token(TokenType.Number, "30", 32),

                    new Token(TokenType.RightBrace, "}", 34),
                    new Token(TokenType.RightBrace, "}", 35),

                    new Token(TokenType.EndOfStream, string.Empty, 36),
                ]
            ),
        ];

        // Array containing mixed primitive values:
        // ["John",30,true,null]
        yield return
        [
            new JsonParserTestData(
                [
                    new Token(TokenType.LeftBracket, "[", 1),

                    new Token(TokenType.String, "John", 2),
                    new Token(TokenType.Comma, ",", 8),

                    new Token(TokenType.Number, "30", 9),
                    new Token(TokenType.Comma, ",", 11),

                    new Token(TokenType.Identifier, "true", 12),
                    new Token(TokenType.Comma, ",", 16),

                    new Token(TokenType.Identifier, "null", 17),

                    new Token(TokenType.RightBracket, "]", 21),
                    new Token(TokenType.EndOfStream, string.Empty, 22),
                ]
            ),
        ];

        // Nested arrays:
        // [["Red","Blue"],["Hammer","Saw"]]
        yield return
        [
            new JsonParserTestData(
                [
                    new Token(TokenType.LeftBracket, "[", 1),

                    new Token(TokenType.LeftBracket, "[", 2),
                    new Token(TokenType.String, "Red", 3),
                    new Token(TokenType.Comma, ",", 8),
                    new Token(TokenType.String, "Blue", 9),
                    new Token(TokenType.RightBracket, "]", 15),

                    new Token(TokenType.Comma, ",", 16),

                    new Token(TokenType.LeftBracket, "[", 17),
                    new Token(TokenType.String, "Hammer", 18),
                    new Token(TokenType.Comma, ",", 26),
                    new Token(TokenType.String, "Saw", 27),
                    new Token(TokenType.RightBracket, "]", 32),

                    new Token(TokenType.RightBracket, "]", 33),
                    new Token(TokenType.EndOfStream, string.Empty, 34),
                ]
            ),
        ];

        // Array of objects:
        // [{"Name":"John"},{"Name":"Jane"}]
        yield return
        [
            new JsonParserTestData(
                [
                    new Token(TokenType.LeftBracket, "[", 1),

                    new Token(TokenType.LeftBrace, "{", 2),
                    new Token(TokenType.String, "Name", 3),
                    new Token(TokenType.Colon, ":", 9),
                    new Token(TokenType.String, "John", 10),
                    new Token(TokenType.RightBrace, "}", 16),

                    new Token(TokenType.Comma, ",", 17),

                    new Token(TokenType.LeftBrace, "{", 18),
                    new Token(TokenType.String, "Name", 19),
                    new Token(TokenType.Colon, ":", 25),
                    new Token(TokenType.String, "Jane", 26),
                    new Token(TokenType.RightBrace, "}", 32),

                    new Token(TokenType.RightBracket, "]", 33),
                    new Token(TokenType.EndOfStream, string.Empty, 34),
                ]
            ),
        ];

        // Object containing an array:
        // {"Colours":["Red","Blue","Green"]}
        yield return
        [
            new JsonParserTestData(
                [
                    new Token(TokenType.LeftBrace, "{", 1),

                    new Token(TokenType.String, "Colours", 2),
                    new Token(TokenType.Colon, ":", 11),

                    new Token(TokenType.LeftBracket, "[", 12),
                    new Token(TokenType.String, "Red", 13),
                    new Token(TokenType.Comma, ",", 18),
                    new Token(TokenType.String, "Blue", 19),
                    new Token(TokenType.Comma, ",", 25),
                    new Token(TokenType.String, "Green", 26),
                    new Token(TokenType.RightBracket, "]", 33),

                    new Token(TokenType.RightBrace, "}", 34),
                    new Token(TokenType.EndOfStream, string.Empty, 35),
                ]
            ),
        ];

        // Complex nested structure:
        // {"Person":{"Name":"John","Skills":["C#",".NET"]},"Active":true}
        yield return
        [
            new JsonParserTestData(
                [
                    new Token(TokenType.LeftBrace, "{", 1),

                    new Token(TokenType.String, "Person", 2),
                    new Token(TokenType.Colon, ":", 10),

                    new Token(TokenType.LeftBrace, "{", 11),

                    new Token(TokenType.String, "Name", 12),
                    new Token(TokenType.Colon, ":", 18),
                    new Token(TokenType.String, "John", 19),

                    new Token(TokenType.Comma, ",", 25),

                    new Token(TokenType.String, "Skills", 26),
                    new Token(TokenType.Colon, ":", 34),

                    new Token(TokenType.LeftBracket, "[", 35),
                    new Token(TokenType.String, "C#", 36),
                    new Token(TokenType.Comma, ",", 40),
                    new Token(TokenType.String, ".NET", 41),
                    new Token(TokenType.RightBracket, "]", 47),

                    new Token(TokenType.RightBrace, "}", 48),

                    new Token(TokenType.Comma, ",", 49),

                    new Token(TokenType.String, "Active", 50),
                    new Token(TokenType.Colon, ":", 58),
                    new Token(TokenType.Identifier, "true", 59),

                    new Token(TokenType.RightBrace, "}", 63),
                    new Token(TokenType.EndOfStream, string.Empty, 64),
                ]
            ),
        ];
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}