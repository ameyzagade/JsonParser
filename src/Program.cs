using JsonParser.App.Executor;

var status = await new Executor().Execute(args);
Console.Out.WriteLine(status);

return;