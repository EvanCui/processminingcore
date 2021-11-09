namespace Encoo.ProcessMining.Engine;

public interface IProcessInstanceDetector
{
    Task<ExecuteUnitResult> DetectAsync(CancellationToken token);
}
