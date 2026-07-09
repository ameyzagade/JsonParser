using System.Collections;

namespace JsonParser.Tests.Lexer.TestData;

public class InvalidJsonLexerTestData : IEnumerable<object[]>
{
    public IEnumerator<object[]> GetEnumerator()
    {
        yield return [
            new JsonLexerTestData("{\"", [], "Lexer input abruptly terminated at position: 2 in the input JSON. No character encountered in the input after double quote!")
        ];

        yield return [
            new JsonLexerTestData("{ \"Key\":\"", [], "Lexer input abruptly terminated at position: 9 in the input JSON. No character encountered in the input after double quote!")
        ];

        yield return [
            new JsonLexerTestData("""{ "key""", [], "Lexer input abruptly terminated at position: 6 in the input JSON. No closing double quote encountered!")
        ];

        yield return [
            new JsonLexerTestData("""{ "key": "valu""", [], "Lexer input abruptly terminated at position: 14 in the input JSON. No closing double quote encountered!")
        ];

        yield return [
            new JsonLexerTestData("""{ "k\""", [], "Lexer input abruptly terminated at position: 5 in the input JSON. No character encountered in the input after the escape sequence!")
        ];

        yield return [
            new JsonLexerTestData("""{ "key": "\tHello\""", [], "Lexer input abruptly terminated at position: 18 in the input JSON. No character encountered in the input after the escape sequence!")
        ];

        yield return [
            new JsonLexerTestData("""{ "ke\yy": "value" } """, [], "Unknown escape character encountered at position: 7 in the input JSON. Character: y")
        ];

        yield return [
            new JsonLexerTestData("""{ "key": "\tHello\world""", [], "Unknown escape character encountered at position: 19 in the input JSON. Character: w")
        ];
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}