using JsonParser.App.Lexer.Exceptions;
using JsonParser.Tests.Lexer.TestData;
using JsonParser.Tests.Lexer.TestData.Numbers;

namespace JsonParser.Tests.Lexer;

public class JsonInputScannerTests
{
    private readonly App.Lexer.JsonInputScanner _sut = new();

    [Theory]
    [ClassData(typeof(InvalidJsonInputScannerTestData))]
    public void WhenUnsuccessfulScan_ShouldThrowException(JsonInputScannerTestData data)
    {
        var exception = Assert.Throws<JsonInputScannerException>(() => _sut.Tokenize(data.Input));
        Assert.Equal(data.ExceptionMessage, exception.Message);
    }

    [Theory]
    [ClassData(typeof(TokenizableJsonInputScannerTestData))]
    public void WhenSuccessfulScan_ShouldReturnTokens(JsonInputScannerTestData data)
    {
        var actualTokens = _sut.Tokenize(data.Input);

        Assert.Equal(data.ExpectedTokens.Count, actualTokens.Count);
        for (int i = 0; i < data.ExpectedTokens.Count; i++)
        {
            var expectedToken = data.ExpectedTokens[i];
            var actualToken = actualTokens[i];

            Assert.Equal(expectedToken.TokenType, actualToken.TokenType);
            Assert.Equal(expectedToken.Value, actualToken.Value);
            Assert.Equal(expectedToken.StartIndex, actualToken.StartIndex);
        }
    }

    [Theory]
    [ClassData(typeof(TokenizableNumberJsonInputScannerTestData))]
    public void WhenSuccessfulNumberTokenization_ShouldReturnTokens(JsonInputScannerTestData data)
    {
        var actualTokens = _sut.Tokenize(data.Input);

        for (int i = 0; i < actualTokens.Count; i++)
        {
            var expectedToken = data.ExpectedTokens[i];
            var actualToken = actualTokens[i];

            Assert.Equal(expectedToken.TokenType, actualToken.TokenType);
            Assert.Equal(expectedToken.Value, actualToken.Value);
            Assert.Equal(expectedToken.StartIndex, actualToken.StartIndex);
        }
    }

    [Theory]
    [ClassData(typeof(InvalidNumberJsonInputScannerTestData))]
    public void WhenInvalidNumberInput_ShouldThrowException(JsonInputScannerTestData data)
    {
        var exception = Assert.Throws<JsonInputScannerException>(() => _sut.Tokenize(data.Input));
        Assert.Equal(data.ExceptionMessage, exception.Message);
    }
}