namespace Encoo.ProcessMining.Engine;

public record RunResult(RunResultType Type, int Progress = 0, Exception Exception = null)
{
    public static RunResult NoWorkToDo { get; set; } = new(RunResultType.NoWorkToDo);

    public static RunResult MoreWorkToDo { get; set; } = new(RunResultType.MoreWorkToDo);

    public static RunResult Completed { get; set; } = new(RunResultType.NoWorkToDo, 100);

    public static RunResult CreateErroredResult(Exception ex, int progress = 0) => new(RunResultType.ExceptionHappened, progress, ex);
}
