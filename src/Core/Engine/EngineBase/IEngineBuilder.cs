namespace Encoo.ProcessMining.Engine;

public interface IEngineBuilder
{
    IEngine Build(IServiceProvider serviceProvider);
}
