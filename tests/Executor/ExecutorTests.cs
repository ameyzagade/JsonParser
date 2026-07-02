namespace JsonParser.Api.UnitTests.Executor;

using Executor = App.Executor.Executor;

public class ExecutorTests
{
    private readonly Executor _executor = new();

    private sealed class ExecutorContext
    {
        public string FilePath { get; set; }
        public int ExecutionStatus { get; set; }
    }

    [Fact]
    public async Task WhenValidJsonShouldReturnOne()
    {
        ExecutorContext[] contexts = [
            new ExecutorContext
            {
                FilePath = "data/1/valid.json",
            }
        ];

        foreach (var context in contexts)
        {
            context.ExecutionStatus = await _executor.Execute([context.FilePath]);
        }

        Assert.Contains(contexts, x => x.ExecutionStatus == 0);
    }
}