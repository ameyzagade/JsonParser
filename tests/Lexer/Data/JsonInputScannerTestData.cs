using JsonParser.App.TokenModel;

namespace JsonParser.Tests.Lexer.TestData;

public sealed record JsonInputScannerTestData(string Input, IReadOnlyList<Token> ExpectedTokens, string ExceptionMessage = "");