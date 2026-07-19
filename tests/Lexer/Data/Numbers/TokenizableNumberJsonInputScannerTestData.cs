using System.Collections;
using JsonParser.App.TokenModel;

namespace JsonParser.Tests.Lexer.TestData.Numbers;

public class TokenizableNumberJsonInputScannerTestData : IEnumerable<object[]>
{
    public IEnumerator<object[]> GetEnumerator()
    {
        yield return new object[]
        {
            new JsonInputScannerTestData(
                "0",
                [
                    new Token(TokenType.Number, "0", 1),
                    new Token(TokenType.EndOfStream, string.Empty, 2)
                ]
            )
        };

        yield return new object[]
        {
            new JsonInputScannerTestData(
                "-0",
                [
                    new Token(TokenType.Number, "-0", 1),
                    new Token(TokenType.EndOfStream, string.Empty, 3)
                ]
            )
        };

        yield return new object[]
        {
            new JsonInputScannerTestData(
                "12",
                [
                    new Token(TokenType.Number, "12", 1),
                    new Token(TokenType.EndOfStream, string.Empty, 3)
                ]
            )
        };

        yield return new object[]
        {
            new JsonInputScannerTestData(
                "120",
                [
                    new Token(TokenType.Number, "120", 1),
                    new Token(TokenType.EndOfStream, string.Empty, 4)
                ]
            )
        };

        yield return new object[]
        {
            new JsonInputScannerTestData(
                "-12",
                [
                    new Token(TokenType.Number, "-12", 1),
                    new Token(TokenType.EndOfStream, string.Empty, 4)
                ]
            )
        };

        yield return new object[]
        {
            new JsonInputScannerTestData(
                "0.1012",
                [
                    new Token(TokenType.Number, "0.1012", 1),
                    new Token(TokenType.EndOfStream, string.Empty, 7)
                ]
            )
        };

        yield return new object[]
        {
            new JsonInputScannerTestData(
                "-0.1012",
                [
                    new Token(TokenType.Number, "-0.1012", 1),
                    new Token(TokenType.EndOfStream, string.Empty, 8)
                ]
            )
        };

        yield return new object[]
        {
            new JsonInputScannerTestData(
                "0.0123",
                [
                    new Token(TokenType.Number, "0.0123", 1),
                    new Token(TokenType.EndOfStream, string.Empty, 7)
                ]
            )
        };

        yield return new object[]
        {
            new JsonInputScannerTestData(
                "12.123",
                [
                    new Token(TokenType.Number, "12.123", 1),
                    new Token(TokenType.EndOfStream, string.Empty, 7)
                ]
            )
        };

        yield return new object[]
        {
            new JsonInputScannerTestData(
                "-12.123",
                [
                    new Token(TokenType.Number, "-12.123", 1),
                    new Token(TokenType.EndOfStream, string.Empty, 8)
                ]
            )
        };

        yield return new object[]
        {
            new JsonInputScannerTestData(
                "0e+1",
                [
                    new Token(TokenType.Number, "0e+1", 1),
                    new Token(TokenType.EndOfStream, string.Empty, 5)
                ]
            )
        };

        yield return new object[]
        {
            new JsonInputScannerTestData(
                "0E-0",
                [
                    new Token(TokenType.Number, "0E-0", 1),
                    new Token(TokenType.EndOfStream, string.Empty, 5)
                ]
            )
        };

        yield return new object[]
        {
            new JsonInputScannerTestData(
                "-0E1",
                [
                    new Token(TokenType.Number, "-0E1", 1),
                    new Token(TokenType.EndOfStream, string.Empty, 5)
                ]
            )
        };

        yield return new object[]
        {
            new JsonInputScannerTestData(
                "-0e+1",
                [
                    new Token(TokenType.Number, "-0e+1", 1),
                    new Token(TokenType.EndOfStream, string.Empty, 6)
                ]
            )
        };

        yield return new object[]
        {
            new JsonInputScannerTestData(
                "-12e+123",
                [
                    new Token(TokenType.Number, "-12e+123", 1),
                    new Token(TokenType.EndOfStream, string.Empty, 9)
                ]
            )
        };

        yield return new object[]
        {
            new JsonInputScannerTestData(
                "-122E-334",
                [
                    new Token(TokenType.Number, "-122E-334", 1),
                    new Token(TokenType.EndOfStream, string.Empty, 10)
                ]
            )
        };

        yield return new object[]
        {
            new JsonInputScannerTestData(
                "0.123e+12",
                [
                    new Token(TokenType.Number, "0.123e+12", 1),
                    new Token(TokenType.EndOfStream, string.Empty, 10)
                ]
            )
        };

        yield return new object[]
        {
            new JsonInputScannerTestData(
                "-0.123e-12",
                [
                    new Token(TokenType.Number, "-0.123e-12", 1),
                    new Token(TokenType.EndOfStream, string.Empty, 11)
                ]
            )
        };

        yield return new object[]
        {
            new JsonInputScannerTestData(
                "12.123E+14",
                [
                    new Token(TokenType.Number, "12.123E+14", 1),
                    new Token(TokenType.EndOfStream, string.Empty, 11)
                ]
            )
        };

        yield return new object[]
        {
            new JsonInputScannerTestData(
                "-12.123E-14",
                [
                    new Token(TokenType.Number, "-12.123E-14", 1),
                    new Token(TokenType.EndOfStream, string.Empty, 12)
                ]
            )
        };
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}