namespace Encoo.ProcessMining.Engine;

public interface IProcessClassifier
{
    Task<ExecuteUnitResult> ClassifyAsync(CancellationToken token);
}
