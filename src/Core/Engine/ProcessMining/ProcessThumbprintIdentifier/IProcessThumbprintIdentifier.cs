namespace Encoo.ProcessMining.Engine;

public interface IProcessThumbprintIdentifier
{
    Task<ExecuteUnitResult> IdentifyAsync(CancellationToken token);
}
