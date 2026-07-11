using System.Collections;

namespace JsonParser.Tests.Lexer.TestData;

public class InvalidJsonInputScannerTestData : IEnumerable<object[]>
{
    public IEnumerator<object[]> GetEnumerator()
    {
        yield return [
            new JsonInputScannerTestData("\"", [], "Input terminated after position: 1 without an ending double quote!")
        ];

        yield return [
            new JsonInputScannerTestData("{\"", [], "Input terminated after position: 2 without an ending double quote!")
        ];

        yield return [
            new JsonInputScannerTestData("{ \"Key\":\"", [], "Input terminated after position: 9 without an ending double quote!")
        ];

        yield return [
            new JsonInputScannerTestData("""{ "key""", [], "Input terminated after position: 3 without an ending double quote!")
        ];

        yield return [
            new JsonInputScannerTestData("""{ "key": "valu""", [], "Input terminated after position: 10 without an ending double quote!")
        ];

        yield return [
            new JsonInputScannerTestData("""{ "k\""", [], "Input terminated after position: 5 in middle of an escape sequence!")
        ];

        yield return [
            new JsonInputScannerTestData("\"\\x\"", [], "Unknown escape character encountered at position: 3. Received character: x")
        ];

        yield return [
            new JsonInputScannerTestData("""{ "key": "\tHello\" """, [], "Input terminated after position: 10 without an ending double quote!")
        ];

        yield return [
            new JsonInputScannerTestData("""{ "ke\yy": "value" } """, [], "Unknown escape character encountered at position: 7. Received character: y")
        ];

        yield return [
            new JsonInputScannerTestData("""{ "key": "\tHello\world""", [], "Unknown escape character encountered at position: 19. Received character: w")
        ];

        yield return [
            new JsonInputScannerTestData(""" "hello\q" """, [], "Unknown escape character encountered at position: 9. Received character: q")
        ];

        yield return [
            new JsonInputScannerTestData(""" "hello\nworld\t\q" """, [], "Unknown escape character encountered at position: 18. Received character: q")
        ];
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}