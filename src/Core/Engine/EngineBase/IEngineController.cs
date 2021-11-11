namespace Encoo.ProcessMining.Engine;

public interface IEngineController
{
    IEngineBuilder EngineBuilder { get; }

    Task StartAsync(CancellationToken token);

    Task StopAsync(CancellationToken token);

    void TriggerExecute();
}
