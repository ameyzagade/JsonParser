using System.Collections;
using JsonParser.App.TokenModel;

namespace JsonParser.Tests.Lexer.TestData.Numbers;

public class InvalidNumberJsonInputScannerTestData : IEnumerable<object[]>
{
    public IEnumerator<object[]> GetEnumerator()
    {
        yield return new object[]
        {
            new JsonInputScannerTestData(
                "+0",
                [],
                "Expected - (minus) or [0-9] (digit) at position 1, but received '+'."
            )
        };

        yield return new object[]
        {
            new JsonInputScannerTestData(
                "+12",
                [],
                "Expected - (minus) or [0-9] (digit) at position 1, but received '+'."
            )
        };

        yield return new object[]
        {
            new JsonInputScannerTestData(
                "-",
                [],
                "Expected [0-9] (digit) after a - (minus) at position 2, but reached the end of input."
            )
        };

        yield return new object[]
        {
            new JsonInputScannerTestData(
                "-.",
                [],
                "Expected [0-9] (digit) after a - (minus) at position 2, but received '.'."
            )
        };

        yield return new object[]
        {
            new JsonInputScannerTestData(
                "01",
                [],
                "Expected . (period) or e Or E (exponent sign) after a 0 (zero) at position 2, but received '1'."
            )
        };

        yield return new object[]
        {
            new JsonInputScannerTestData(
                "-01",
                [],
                "Expected . (period) or e Or E (exponent sign) after a 0 (zero) at position 3, but received '1'."
            )
        };

        yield return new object[]
        {
            new JsonInputScannerTestData(
                "3.",
                [],
                "Expected [0-9] (digit) after a . (period sign) at position 3, but reached the end of input."
            )
        };

        yield return new object[]
        {
            new JsonInputScannerTestData(
                "1e",
                [],
                "Expected either + (plus) or - (minus) after e or E (exponential sign) at position 3, but reached the end of input."
            )
        };

        yield return new object[]
        {
            new JsonInputScannerTestData(
                "-1E",
                [],
                "Expected either + (plus) or - (minus) after e or E (exponential sign) at position 4, but reached the end of input."
            )
        };

        yield return new object[]
        {
            new JsonInputScannerTestData(
                "-1E.",
                [],
                "Expected either + (plus) or - (minus) after e or E (exponential sign) at position 4, but received '.'."
            )
        };

        yield return new object[]
        {
            new JsonInputScannerTestData(
                "-1E+",
                [],
                "Expected [0-9] (digit) after + (plus sign) or - (minus sign) in the exponent at position 5, but reached the end of input."
            )
        };

        yield return new object[]
        {
            new JsonInputScannerTestData(
                "1e-",
                [],
                "Expected [0-9] (digit) after + (plus sign) or - (minus sign) in the exponent at position 4, but reached the end of input."
            )
        };
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
