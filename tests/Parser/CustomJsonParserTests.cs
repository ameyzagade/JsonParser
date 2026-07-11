using JsonParser.App.Parser;
using JsonParser.Tests.Parser.TestData;

namespace JsonParser.Tests.Parser;

public class CustomJsonParserTests
{
    private readonly CustomJsonParser _sut = new();

    [Theory]
    [ClassData(typeof(InvalidCustomJsonParserTestData))]
    public void WhenInvalidGrammar_ShouldThrowException(JsonParserTestData data)
    {
        var exception = Assert.Throws<CustomJsonParserException>(() => _sut.Parse(data.Tokens));

        Assert.Equal(data.ExceptionMessage, exception.Message);
    }

    [Theory]
    [ClassData(typeof(ValidCustomJsonParserTestData))]
    public void WhenValidGrammar_ShouldNotThrowAnyException(JsonParserTestData data)
    {
        var exception = Record.Exception(() => _sut.Parse(data.Tokens));

        Assert.Null(exception);
    }
}