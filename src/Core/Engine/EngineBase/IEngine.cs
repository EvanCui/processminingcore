namespace Encoo.ProcessMining.Engine;

public interface IEngine
{
    IList<IEngineComponent> EngineComponents { get; }

    Task<RunResult> RunAsync(CancellationToken token);
}
