using System.Collections;

namespace JsonParser.Tests.Lexer.TestData;

public class InvalidEscapeSequenceJsonLexerTestData : IEnumerable<object[]>
{
    public IEnumerator<object[]> GetEnumerator()
    {
        yield return [
            new JsonLexerTestData("""{ "key": "hello \q" }""", [], "Invalid escape sequence '\\q' at position 17")
        ];

        yield return [
            new JsonLexerTestData("""{ "key": true abc }""", [], "Expected ',' or '}' after primitive value at position 15.")
        ];

        yield return [
            new JsonLexerTestData("""{ "key": "hello }""", [], "Lexer not in the expected Normal state after consuming all characters!")
        ];

        yield return [
            new JsonLexerTestData("""{ "key": "hello\ }""", [], "Invalid escape sequence '\\ ' at position 16")
        ];
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}