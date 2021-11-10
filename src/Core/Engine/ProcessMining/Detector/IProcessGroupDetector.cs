namespace Encoo.ProcessMining.Engine;

public interface IProcessGroupDetector
{
    Task<ExecuteUnitResult> DetectAsync(CancellationToken token);
}
