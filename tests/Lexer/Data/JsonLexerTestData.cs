using JsonParser.App.Lexer;

namespace JsonParser.Tests.Lexer.TestData;

public sealed record JsonLexerTestData(string Input, IReadOnlyList<Token> ExpectedTokens, string ExceptionMessage = "");