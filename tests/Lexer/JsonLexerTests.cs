using JsonParser.App.Lexer.Exceptions;
using JsonParser.Tests.Lexer.TestData;

namespace JsonParser.Tests.Lexer;

public class JsonLexerTests
{
    private readonly App.Lexer.JsonLexer _sut = new();

    [Theory]
    [ClassData(typeof(InvalidJsonLexerTestData))]
    public void WhenUnsuccessfulScan_ShouldThrowException(JsonLexerTestData data)
    {
        var exception = Assert.Throws<JsonLexerException>(() => _sut.Tokenize(data.Input));
        Assert.Equal(data.ExceptionMessage, exception.Message);
    }

    [Theory]
    [ClassData(typeof(ValidJsonLexerTestData))]
    public void WhenSuccessfulScan_ShouldReturnTokens(JsonLexerTestData data)
    {
        var actualTokens = _sut.Tokenize(data.Input);

        Assert.Equal(data.ExpectedTokens.Count, actualTokens.Count);
        for (int i = 0; i < data.ExpectedTokens.Count; i++)
        {
            var expectedToken = data.ExpectedTokens[i];
            var actualToken = actualTokens[i];

            Assert.Equal(expectedToken.TokenType, actualToken.TokenType);
            Assert.Equal(expectedToken.Value, actualToken.Value);
        }
    }
}