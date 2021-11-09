namespace Encoo.ProcessMining.Engine;

public interface IProcessAnalyzer
{
    Task<ExecuteUnitResult> AnalyzeAsync(CancellationToken token);
}
