using JsonParser.App.Lexer;

namespace JsonParser.Tests.Parser.TestData;

public sealed record JsonParserTestData(IReadOnlyList<Token> Tokens, string ExceptionMessage = "");
