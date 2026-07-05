using System.Collections;

namespace JsonParser.Tests.Lexer.TestData;

public class InvalidEscapeSequenceJsonLexerTestData : IEnumerable<object[]>
{
    public IEnumerator<object[]> GetEnumerator()
    {
        yield return [
            new JsonLexerTestData("""{ "key": "hello \q" }""", [], "Invalid escape sequence '\\q' at position 17")
        ];
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}