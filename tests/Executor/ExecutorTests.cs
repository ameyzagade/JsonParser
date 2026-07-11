namespace JsonParser.App.UnitTests.Executor;

using Executor = App.Executor.Executor;

public class ExecutorTests
{
    private readonly Executor _sut = new();

    private sealed class ExecutorContext
    {
        public string FilePath { get; set; } = string.Empty;
        public int ExecutionStatus { get; set; } = 0;
    }

    [Fact]
    public async Task WhenValidJsonFile_ShouldReturnOne()
    {
        ExecutorContext[] contexts = [
            new ExecutorContext
            {
                FilePath = "Executor/Data/Files/1/valid.json",
            },
            new ExecutorContext
            {
                FilePath = "Executor/Data/Files/2/valid.json"
            },
            new ExecutorContext
            {
                FilePath = "Executor/Data/Files/2/valid2.json"
            },
            new ExecutorContext
            {
                FilePath = "Executor/Data/Files/3/valid.json"
            },
            new ExecutorContext
            {
                FilePath = "Executor/Data/Files/4/valid.json"
            },
            new ExecutorContext
            {
                FilePath = "Executor/Data/Files/4/valid2.json"
            },
        ];

        foreach (var context in contexts)
        {
            context.ExecutionStatus = await _sut.Execute([context.FilePath]);
        }

        Assert.Contains(contexts, x => x.ExecutionStatus == 0);
    }

    [Fact]
    public async Task WhenInvalidJsonFile_ShouldReturnZero()
    {
        ExecutorContext[] contexts = [
            new ExecutorContext
            {
                FilePath = "Executor/Data/Files/1/invalid.json",
            },
            new ExecutorContext
            {
                FilePath = "Executor/Data/Files2/invalid.json"
            },
            new ExecutorContext
            {
                FilePath = "Executor/Data/Files/2/invalid2.json"
            },
            new ExecutorContext
            {
                FilePath = "Executor/Data/Files/3/invalid.json"
            },
            new ExecutorContext
            {
                FilePath = "Executor/Data/Files/4/invalid.json"
            },
        ];

        foreach (var context in contexts)
        {
            context.ExecutionStatus = await _sut.Execute([context.FilePath]);
        }

        Assert.Contains(contexts, x => x.ExecutionStatus == 1);
    }
}