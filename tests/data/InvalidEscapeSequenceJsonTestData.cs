using System.Collections;

namespace JsonParser.Tests.TestData;

public class InvalidEscapeSequenceJsonTestData : IEnumerable<object[]>
{
    public IEnumerator<object[]> GetEnumerator()
    {
        yield return [
            new JsonTestData("""{ "key": "hello \q" }""", [], "Invalid escape sequence '\\q' at position 17")
        ];
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}