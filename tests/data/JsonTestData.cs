using JsonParser.App.Lexer;

namespace JsonParser.Tests.TestData;

public sealed record JsonTestData(string Input, IReadOnlyList<Token> ExpectedTokens, string ExceptionMessage = "");