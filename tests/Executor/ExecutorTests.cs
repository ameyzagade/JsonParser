namespace JsonParser.Api.UnitTests.Executor;

using Executor = App.Executor.Executor;

public class ExecutorTests
{
    private readonly Executor _executor = new();

    private sealed class ExecutorContext
    {
        public string FilePath { get; set; } = string.Empty;
        public int ExecutionStatus { get; set; } = 0;
    }

    [Fact]
    public async Task WhenValidJsonShouldReturnOne()
    {
        ExecutorContext[] contexts = [
            new ExecutorContext
            {
                FilePath = "data/1/valid.json",
            },
            new ExecutorContext
            {
                FilePath = "data/2/valid.json"
            },
            new ExecutorContext
            {
                FilePath = "data/2/valid2.json"
            },
        ];

        foreach (var context in contexts)
        {
            context.ExecutionStatus = await _executor.Execute([context.FilePath]);
        }

        Assert.Contains(contexts, x => x.ExecutionStatus == 0);
    }

    [Fact]
    public async Task WhenValidJsonShouldReturnZero()
    {
        ExecutorContext[] contexts = [
            new ExecutorContext
            {
                FilePath = "data/1/invalid.json",
            },
            new ExecutorContext
            {
                FilePath = "data/2/invalid.json"
            },
            new ExecutorContext
            {
                FilePath = "data/2/invalid2.json"
            },
        ];

        foreach (var context in contexts)
        {
            context.ExecutionStatus = await _executor.Execute([context.FilePath]);
        }

        Assert.Contains(contexts, x => x.ExecutionStatus == 1);
    }
}