namespace Encoo.ProcessMining.Engine;

public interface IEngineComponent
{
    string Name { get; }

    Task<RunResult> RunAsync(int batchSize, CancellationToken token);
}
