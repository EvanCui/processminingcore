using Microsoft.Extensions.DependencyInjection;

namespace Encoo.ProcessMining.Engine;

public class ProcessMiningEngineBuilder : IEngineBuilder
{
    public IEngine Build(IServiceProvider serviceProvider) => serviceProvider.GetRequiredService<ProcessMiningEngine>();
}
