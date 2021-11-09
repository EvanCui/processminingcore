namespace Encoo.ProcessMining.Engine;

internal interface IEngine
{
    Task StartAsync(CancellationToken token);
    Task StopAsync(CancellationToken token);
    void TriggerExecute();
}
