using JsonParser.App.Lexer;
using JsonParser.App.Parser;

namespace JsonParser.App.Executor;

public sealed class Executor
{
    private readonly JsonLexer _lexer = new();
    private readonly CustomJsonParser _parser = new();

    public async Task<int> Execute(string[] arguments)
    {
        var argument = arguments.Length > 0 ? arguments.First() : string.Empty;
        if (!ValidateArgument(argument))
        {
            return 1;
        }

        var jsonFilePath = Path.IsPathFullyQualified(argument)
            ? argument
            : Path.Combine(Directory.GetCurrentDirectory(), argument);

        if (!File.Exists(jsonFilePath))
        {
            Console.Error.WriteLine($"{Path.GetFileName(jsonFilePath)} doesn't exist");
            return 1;
        }

        var cancellationToken = new CancellationToken();
        var content = await File.ReadAllTextAsync(jsonFilePath, cancellationToken);
        if (content.Length == 0)
        {
            Console.Error.WriteLine("Empty file received");
            return 1;
        }

        try
        {
            var tokens = _lexer.Tokenize(content);
            return _parser.Parse(tokens) ? 0 : 1;
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine(ex.Message);

            return 1;
        }
    }

    private static bool ValidateArgument(string argument)
    {
        if (string.IsNullOrEmpty(argument))
        {
            Console.Error.WriteLine("No argument passed");
            return false;
        }

        if (!argument.EndsWith(".json", StringComparison.OrdinalIgnoreCase))
        {
            Console.Error.WriteLine("Argument should end with .json extension");
            return false;
        }

        return true;
    }
}