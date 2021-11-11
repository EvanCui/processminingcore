namespace Encoo.ProcessMining.Engine;

public class ProcessAnalyzer : IProcessAnalyzer
{
    public string Name => nameof(ProcessAnalyzer);

    public Task<RunResult> RunAsync(int batchSize, CancellationToken token)
    {
        throw new NotImplementedException();
    }
}
